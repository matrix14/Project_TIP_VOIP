using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace ClientWindows
{

    public delegate void StringCallback(String message);
    public delegate void InvitationCallback(Invitation inv);
    public partial class LoggedInForm : Form
    {
        //public static List<Friend> callbackFriendsContainer;

        private static List<Friend> friendsContainer = new List<Friend>();
        private static List<Invitation> invitationContainer = new List<Invitation>();
        private Hashtable friendListDetails = new Hashtable(){
            {"friendsStart", 0},
            {"friendsAmount", 0},
            {"invitationsStart", 0},
            {"invitationsAmount", 0}
        };
        public LoggedInForm()
        {
            InitializeComponent();
            StringCallback callback2 = writeToInvitingList;
            LoggedInService.NewInvitationCallback = callback2;
            this.signedInLogin_Text.Text = Program.username;

            StringCallback callback = writeToFriendContainer;
            LoggedInService.GetFriendsCallback = callback;

            InvitationCallback callback3 = removeFromInvitingList;
            LoggedInService.InvitationProcessedCallback = callback3;

            LoggedInService.getFriends();
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
                friendListDetails["friendsStart"] = 0;
                friendListDetails["friendsAmount"] = 0;
                friendListDetails["invitationsStart"] = 0;
                friendListDetails["invitationsAmount"] = 0;
                friendsList.Items.Clear();
                friendsList.Items.Add("ZNAJOMI:");
                int i = 1;
                if (friendsContainer == null || friendsContainer.Count == 0)
                {
                    i++;
                    friendsList.Items.Add("- Nie masz znajomych");
                }
                else
                {
                    foreach (Friend fr in friendsContainer)
                    {
                        i++;
                        friendListDetails["friendsAmount"] = (int)friendListDetails["friendsAmount"] + 1;
                        friendsList.Items.Add(fr);
                    }
                }
                friendListDetails["invitationsStart"] = i;
                friendsList.Items.Add("ZAPROSZENIA:");
                if (invitationContainer == null || invitationContainer.Count == 0)
                {
                    friendsList.Items.Add("- Nie masz zaproszeń");
                }
                else {
                    foreach (Invitation inv in invitationContainer)
                    {
                        friendListDetails["invitationsAmount"] = (int)friendListDetails["invitationsAmount"]+1;
                        friendsList.Items.Add(inv);
                    }
                }
                return;
            }
        }

        public void writeToFriendContainer(String message)
        {
            String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            String frListStr = replySplit[1].Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries)[1];
            List<Friend> friends = MessageProccesing.DeserializeObject(Options.GET_FRIENDS, frListStr) as List<Friend>;
            friendsContainer.Clear();
            foreach (Friend fr in friends)
            {
                friendsContainer.Add(fr);
            }

            updateFriendList();
            //MessageBox.Show("Friends updated v2");

            //if (null == friendsContainer) return;

            // also make this threadsafe:
            /*if (callbackFriendsContainer.InvokeRequired)
            {
                callbackFriendsContainer.Invoke(new MethodInvoker(() => { writeToFriendList(message); }));
            }
            else
            {
                String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                String frListStr = replySplit[1].Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries)[1];
                List<Friend> friends = MessageProccesing.DeserializeObject(Options.GET_FRIENDS, frListStr) as List<Friend>;
                callbackFriendsContainer.Items.Clear();
                friendsContainer.Clear();
                if (friends == null || friends.Count == 0)
                {
                    callbackFriendsContainer.Items.Add("Nie masz znajomych");
                    return;
                }

                foreach (Friend fr in friends)
                {
                    friendsContainer.Add(fr);
                    callbackFriendsList.Items.Add(fr.username);
                }
            */
            updateFriendList();
                //TODO: update friend list
                //callbackFriendsList.Items.Add(s);
                //callbackFriendsList.TopIndex = callbackFriendsList.Items.Count - (callbackFriendsList.Height / callbackFriendsList.ItemHeight);
            //}
        }

        public void writeToInvitingList(String message)
        {
            Boolean updateFriends = false;
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
                    updateFriends = true;
                    //if adding to friend container - it should be active or not active - Friend
                }
            }
            updateFriendList();
            if(updateFriends)
            {
                LoggedInService.getFriends();
            }
            //MessageBox.Show("Invitations updated v2");
        }

        public void removeFromInvitingList(Invitation inv)
        {
            invitationContainer.Remove(inv);
            updateFriendList();

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

        private void button1_Click_1(object sender, EventArgs e)
        {

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

        private void friendsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;

            int startFriends = (int)friendListDetails["friendsStart"]+1;
            int endFriends = (int)friendListDetails["friendsStart"] + (int)friendListDetails["friendsAmount"];
            int startInvitations = (int)friendListDetails["invitationsStart"]+1;
            int endInvitations = (int)friendListDetails["invitationsStart"] + (int)friendListDetails["invitationsAmount"];

            if (lb.SelectedIndex==0||
                lb.SelectedIndex==(int)friendListDetails["friendsStart"]||
                lb.SelectedIndex == (int)friendListDetails["invitationsStart"]) { return; }
            
            if(lb.SelectedIndex>=startFriends&&lb.SelectedIndex<=endFriends)
            {
                Friend fr = (Friend)lb.Items[lb.SelectedIndex];
                MessageBox.Show("FR " + fr.username.ToString());
            }
            if (lb.SelectedIndex >= startInvitations && lb.SelectedIndex <= endInvitations)
            {
                Invitation inv = (Invitation)lb.Items[lb.SelectedIndex];
                SelectInvitationForm sif = new SelectInvitationForm(inv);
                sif.ShowDialog();
            }
        }
    }
}