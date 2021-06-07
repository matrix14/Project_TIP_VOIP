﻿using Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

namespace ClientWindows
{

    public delegate void StringCallback(String message);
    public delegate void InvitationCallback(Invitation inv);
    public delegate void UsernameCallback(Username u);
    public delegate void IdCallback(Id id);
    public delegate void CallCallback(Call c);
    public delegate void FriendCallback(Friend fr);
    public delegate void NoneCallback();
    public partial class LoggedInForm : Form
    {
        private static List<Friend> friendsContainer = new List<Friend>();
        private static List<Invitation> invitationContainer = new List<Invitation>();
        private static Id lastCallId = null;
        private static Username lastCallUsername = null;
        private InvitationListForm ilf = null;

        private static ManualResetEvent invitationContainerLock = new ManualResetEvent(true);

        private static Friend actualFriendView = null;
        public LoggedInForm()
        {
            InitializeComponent();
            invitationContainer.Clear();
            friendsContainer.Clear();

            UsernameCallback callback4 = newInactiveFriendFunc;
            UsernameCallback callback5 = newActiveFriendFunc;
            IdCallback callback6 = callUserReply;
            BooleanCallback callback7 = callUserReplyFromUser;
            CallCallback callback8 = openInCallWindow;
            FriendCallback callback9 = addToFriendContainer;
            LoggedInService.AddToFriendList = callback9;
            LoggedInService.NewInactiveFriend = callback4;
            LoggedInService.NewActiveFriend = callback5;
            LoggedInService.InviteToConversationReplyOk = callback6;
            LoggedInService.InviteToConversationReplyFromUser = callback7;
            LoggedInService.OpenInCallForm = callback8;


            StringCallback callback2 = writeToInvitingList;
            LoggedInService.NewInvitationCallback = callback2;
            this.signedInLogin_Text.Text = Program.username;

            StringCallback callback = writeToFriendContainer;
            LoggedInService.GetFriendsCallback = callback;

            InvitationCallback callback3 = removeFromInvitingList;
            LoggedInService.InvitationProcessedCallback = callback3;

            LoggedInService.getFriends();
        }

        public void updateInvitationButton()//TODO: update invitationListView also
        {
            if (this.invitingList_button.InvokeRequired)
            {
                this.invitingList_button.Invoke(new MethodInvoker(() => { updateInvitationButton(); }));
                return;
            }
            else
            {
                int amount = 0;
                invitationContainerLock.WaitOne();
                invitationContainerLock.Reset();
                foreach (Invitation inv in invitationContainer)
                {
                    if (inv.status < 2)
                    {
                        amount++;
                    }
                }
                invitationContainerLock.Set();
                this.invitingList_button.Text = "Zaproszenia (" + amount.ToString() + ")";
            }
            if (ilf != null&&ilf.IsDisposed==false)
            {
                invitationContainerLock.WaitOne();
                invitationContainerLock.Reset();
                ilf.updateInvitationList(invitationContainer);
                invitationContainerLock.Set();
            }
        }

        private static int compareFriendList(Friend x, Friend y)
        {
            if (x == null)
            {
                if (y == null)
                    return 0;
                else
                    return -1;
            }
            else
            {
                if (y == null)
                    return 1;
                else
                {
                    if (x.active > y.active)
                        return -1;
                    else if(x.active < y.active)
                        return 1;
                    else
                    {
                        //Do nothing
                    }
                    return x.username.CompareTo(y.username);
                }
            }
        }

        public void updateFriendList()
        {
            if (friendsList.InvokeRequired)
            {
                friendsList.Invoke(new MethodInvoker(() => { updateFriendList(); }));
                return;
            }
            else
            {
                friendsContainer.Sort(compareFriendList);

                friendsList.Items.Clear();
                int i = 1;
                if (friendsContainer == null || friendsContainer.Count == 0)
                {
                    i++;
                }
                else
                {
                    foreach (Friend fr in friendsContainer)
                    {
                        i++;
                        friendsList.Items.Add(fr);
                    }
                }
                return;
            }
        }

        public void writeToFriendContainer(String message)
        {
            String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            String frListStr = replySplit[1].Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries)[1];
            List<Friend> friends = MessageProccesing.DeserializeObjectOnErrorCode(Options.GET_FRIENDS, frListStr) as List<Friend>;
            friendsContainer.Clear();
            foreach (Friend fr in friends)
            {
                friendsContainer.Add(fr);
            }
            updateFriendList();
        }

        public void addToFriendContainer(Friend fr)
        {
            friendsContainer.Add(fr);
            updateFriendList();
        }

        public void writeToInvitingList(String message)
        {
            String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            String frListStr = replySplit[1].Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries)[1];
            List<Invitation> invitations = MessageProccesing.DeserializeObject(Options.FRIEND_INVITATIONS, frListStr) as List<Invitation>;
            foreach (Invitation inv in invitations)
            {
                if(inv.status<2)
                {
                    invitationContainerLock.WaitOne();
                    invitationContainerLock.Reset();
                    invitationContainer.Add(inv);
                    invitationContainerLock.Set();
                } else if(inv.status==2)
                {
                    addToFriendContainer(new Friend(inv.inviteeUsername, 1));
                }
            }
            updateInvitationButton();
        }

        public void removeFromInvitingList(Invitation inv)
        {
            invitationContainerLock.WaitOne();
            invitationContainerLock.Reset();
            invitationContainer.Remove(inv);
            invitationContainerLock.Set();
            /*if(inv.status==2)
            {
                addToFriendContainer(new Friend(inv.username, 1)); //TODO: when accepting invitation it shows active user
            }*/
            updateInvitationButton();
        }

        public void newActiveFriendFunc(Username u)
        {
            foreach(Friend fr in friendsContainer)
            {
                if (fr.username == u.username)
                    fr.active = 1;
            }
            updateFriendList();
            updateFriendWindowStatus();
        }

        public void newInactiveFriendFunc(Username u)
        {
            foreach (Friend fr in friendsContainer)
            {
                if (fr.username == u.username)
                    fr.active = 0;
            }
            updateFriendList();
            updateFriendWindowStatus();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void openFriend_button_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void friendList_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index == -1) return;
            ListBox lb = sender as ListBox;
            if (lb.Items.Count < 1) return;
            
            Friend fr = lb.Items[e.Index] as Friend;
            if (fr == null) return;

            SizeF txt_size = e.Graphics.MeasureString(fr.username, this.Font);

            e.ItemHeight = (int)txt_size.Height + 2*5;
            e.ItemWidth = (int)txt_size.Width;
        }

        private void friendList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1) return;
            ListBox lst = sender as ListBox;
            if (lst.Items.Count < 1) return;
            Friend fr = lst.Items[e.Index] as Friend;
            if (fr == null) return;

            SolidBrush redDot = new SolidBrush(Color.Red);
            SolidBrush greenDot = new SolidBrush(Color.Green);

            e.DrawBackground();

            e.DrawFocusRectangle();

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                if(fr.active==1)
                    e.Graphics.FillEllipse(greenDot, 4, e.Bounds.Top+4, 15, 15);
                else
                    e.Graphics.FillEllipse(redDot, 4, e.Bounds.Top+4, 15, 15);

                e.Graphics.DrawString(fr.username, this.Font, SystemBrushes.HighlightText, 23, e.Bounds.Top+5);
            }
            else
            {
                using (SolidBrush br = new SolidBrush(e.ForeColor))
                {
                    if (fr.active == 1)
                        e.Graphics.FillEllipse(greenDot, 4, e.Bounds.Top+4, 15, 15);
                    else
                        e.Graphics.FillEllipse(redDot, 4, e.Bounds.Top+4, 15, 15);

                    e.Graphics.DrawString(fr.username, this.Font, br, 23, e.Bounds.Top+5);
                }
            }

            e.DrawFocusRectangle();
        }

        private void closeApp_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoggedInForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                if ((Application.OpenForms[i].Name != "LoginForm")&&(Application.OpenForms[i].Name != "LoggedInForm"))
                    Application.OpenForms[i].Close(); //TODO: invoke ???
            }
            LoggedInService.logout();
        }

        private void addFriend_button_Click(object sender, EventArgs e)
        {
            AddFriendForm aff = new AddFriendForm();
            aff.ShowDialog();
        }

        private void showFriendContext()
        {
            activeUserWindow.Visible = true;
            activeFriendStatus_Label.Visible = true;
            callUser.Visible = true;
        }

        public void updateCallStatus(Friend fr) //TODO: uzytkownik w rozmowie = false
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => { updateCallStatus(fr); }));
                return;
            }
            if (this.activeUserWindow.Text != fr.username)
                return;

            if(fr.active==0)
            {
                this.callUser.Enabled = false;
                this.callUser.Text = "Użytkownik nie jest aktywny";
                return;
            }

            if (Program.isInCall)
            {
                if (Program.actualCall == null)
                    return;
                if (Program.actualCall.usernames.Contains(fr.username))
                {
                    this.callUser.Enabled = false;
                    this.callUser.Text = "Uzytkownik w rozmowie";
                }
                else
                {
                    this.callUser.Enabled = true;
                    this.callUser.Text = "Dodaj do rozmowy";
                }
            }
            else {
                this.callUser.Enabled = true;
                this.callUser.Text = "Zadzwoń";
            }
        }

        private void friendsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.SelectedIndex == -1) return;
            if (lb.Items[lb.SelectedIndex].Equals("Wczytywanie...")) return;
            Friend fr = (Friend)lb.Items[lb.SelectedIndex];
            actualFriendView = fr;
            activeUserWindow.Text = fr.username;
            this.callingStatusLabel.Visible = false;
            updateCallStatus(fr);
            //callUser.Enabled = (fr.active == 1);
            if (callUser.Enabled)
            {
                activeFriendStatus_Label.Text = "Aktywny";
                activeFriendStatus_Label.ForeColor = Color.Green;
            }
            else
            {
                activeFriendStatus_Label.Text = "Nieaktywny";
                activeFriendStatus_Label.ForeColor = Color.Red;
            }
            showFriendContext();
        }

        private void invitingList_button_Click(object sender, EventArgs e)
        {
            invitationContainerLock.WaitOne();
            invitationContainerLock.Reset();
            ilf = new InvitationListForm(invitationContainer);
            ilf.Show();
            invitationContainerLock.Set();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void updateFriendWindowStatus()
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => { updateFriendWindowStatus(); }));
                return;
            }
            foreach (Friend fr in friendsContainer)
            {
                if(fr.username==activeUserWindow.Text)
                {
                    callUser.Enabled = (fr.active == 1);
                    if(callUser.Enabled)
                    {
                        activeFriendStatus_Label.Text = "Aktywny";
                        activeFriendStatus_Label.ForeColor = Color.Green;
                        //TODO asd
                    } else
                    {
                        activeFriendStatus_Label.Text = "Nieaktywny";
                        activeFriendStatus_Label.ForeColor = Color.Red;
                    }
                    updateCallStatus(fr);
                    return;
                }
            }
        }

        public void updateFriendViewOnCallClosing()
        {
            if(actualFriendView!=null)
                updateCallStatus(actualFriendView);
        }

        private void callUser_Click(object sender, EventArgs e)
        {
            LoggedInService.inviteToConversation(new Username(activeUserWindow.Text));
            lastCallUsername = new Username(activeUserWindow.Text);
        }

        public void callUserReply(Id callId)
        {
            if (callingStatusLabel.InvokeRequired)
            {
                callingStatusLabel.Invoke(new MethodInvoker(() => { callUserReply(callId); }));
            }
            else
            {
                lastCallId = callId;
                callUser.Enabled = false;
                callingStatusLabel.Visible = true;
                callingStatusLabel.Text = "Oczekuje na odpowiedź...";
                callingStatusLabel.ForeColor = Color.Green;
            }
        }

        public void callUserReplyFromUser(Boolean reply)
        {
            if (callingStatusLabel.InvokeRequired)
            {
                callingStatusLabel.Invoke(new MethodInvoker(() => { callUserReplyFromUser(reply); }));
            }
            else
            {
                callingStatusLabel.Visible = true;
                if (reply==true)
                {
                    callUser.Enabled = false;
                    callingStatusLabel.Text = "";
                    callingStatusLabel.ForeColor = Color.Green;
                    if (lastCallId != null && lastCallUsername != null)
                    {
                        Call c = new Call(lastCallId.id, new List<string> { lastCallUsername.username });
                        NoneCallback ncb = updateFriendViewOnCallClosing;
                        InCallForm icf = new InCallForm(c, ncb);
                        icf.Show();
                        updateCallStatus(new Friend(lastCallUsername, 1));
                    }
                } else
                {
                    callUser.Enabled = true;
                    callingStatusLabel.Text = "Odrzucono połączenie!";
                    callingStatusLabel.ForeColor = Color.Red;
                    if(!Program.isInCall) {
                        LoggedInService.declineCall(lastCallId);
                    }
                }
            }
        }

        public void openInCallWindow(Call c)
        {
            if (callingStatusLabel.InvokeRequired)
            {
                callingStatusLabel.Invoke(new MethodInvoker(() => { openInCallWindow(c); }));
            }
            else
            {
                NoneCallback ncb = updateFriendViewOnCallClosing;
                InCallForm icf = new InCallForm(c, ncb);
                icf.Show();
            }
        }

        private void LoggedInForm_Load(object sender, EventArgs e)
        {

        }

        private void settingsButton_Button_Click(object sender, EventArgs e)
        {
            SettingsForm sf = new SettingsForm();
            sf.ShowDialog();
        }
    }
}