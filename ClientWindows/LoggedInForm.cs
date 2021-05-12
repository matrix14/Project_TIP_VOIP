using Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace ClientWindows
{

    public delegate void StringCallback(String message);
    public delegate void InvitationCallback(Invitation inv);
    public delegate void UsernameCallback(Username u);
    public delegate void IdCallback(Id id);
    public delegate void CallCallback(Call c);
    public partial class LoggedInForm : Form
    {
        private static List<Friend> friendsContainer = new List<Friend>();
        private static List<Invitation> invitationContainer = new List<Invitation>();
        private static Id lastCallId = null;
        private static Username lastCallUsername = null;
        private InvitationListForm ilf = null;
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

        public void updateInvitationButton()
        {
            if (this.invitingList_button.InvokeRequired)
            {
                friendsList.Invoke(new MethodInvoker(() => { updateInvitationButton(); }));
                return;
            }
            else
            {
                int amount = 0;
                foreach (Invitation inv in invitationContainer)
                {
                    if (inv.status < 2)
                    {
                        amount++;
                    }
                }
                this.invitingList_button.Text = "Zaproszenia (" + amount.ToString() + ")";
            }
            if (ilf != null)
                ilf.updateInvitationList(invitationContainer);
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
                    invitationContainer.Add(inv);
                } else if(inv.status==2)
                {
                    Friend newFr = new Friend(inv.inviteeUsername, 1);
                    friendsContainer.Add(newFr);
                }
            }
            updateInvitationButton();
        }

        public void removeFromInvitingList(Invitation inv)
        {
            invitationContainer.Remove(inv);
            if(inv.status==2)
            {
                addToFriendContainer(new Friend(inv.username, 1)); //TODO: when accepting invitation it shows active user
            }
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

        public void updateCallStatus()
        {
            if(this.callUser.InvokeRequired)
            {
                this.callUser.Invoke(new MethodInvoker(() => { updateCallStatus(); }));
                return;
            }
            if (Program.isInCall)
                this.callUser.Text = "Dodaj do rozmowy";
            else
                this.callUser.Text = "Zadzwoń";
        }

        private void friendsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.SelectedIndex == -1) return;
            Friend fr = (Friend)lb.Items[lb.SelectedIndex];
            activeUserWindow.Text = fr.username;
            callUser.Enabled = (fr.active==1);
            this.callingStatusLabel.Visible = false;
            if (Program.isInCall)
                this.callUser.Text = "Dodaj do rozmowy";
            else
                this.callUser.Text = "Zadzwoń";
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
            ilf = new InvitationListForm(invitationContainer);
            ilf.ShowDialog();
            ilf = null;
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
                    } else
                    {
                        activeFriendStatus_Label.Text = "Nieaktywny";
                        activeFriendStatus_Label.ForeColor = Color.Red;
                    }
                    return;
                }
            }
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
                    callingStatusLabel.Text = "Zaakceptowano!";
                    callingStatusLabel.ForeColor = Color.Green;
                    Call c = new Call(lastCallId.id, new List<string> { lastCallUsername.username }); //TODO: lastCallId = null NullReferenceException
                    InCallForm icf = new InCallForm(c);
                    icf.Show();
                } else
                {
                    callUser.Enabled = true;
                    callingStatusLabel.Text = "Odrzucono połączenie!";
                    callingStatusLabel.ForeColor = Color.Red;
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
                InCallForm icf = new InCallForm(c);
                icf.Show();
            }
        }

        private void LoggedInForm_Load(object sender, EventArgs e)
        {

        }
    }
}