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
using Shared;

namespace ClientWindows
{

    public delegate void GetFriendsCallback(String message);
    public partial class LoggedInForm : Form
    {
        public static ListBox callbackFriendsList;
        public LoggedInForm()
        {
            InitializeComponent();
            this.signedInLogin_Text.Text = Program.username;
            callbackFriendsList = friendsList;

            GetFriendsCallback callback = writeToFriendList;     // now OK
            LoggedInService.getFriendsCallback = callback;

            LoggedInService.getFriends();
        }

        public void writeToFriendList(String message)
        {
            if (null == callbackFriendsList) return;

            // also make this threadsafe:
            if (callbackFriendsList.InvokeRequired)
            {
                callbackFriendsList.Invoke(new MethodInvoker(() => { writeToFriendList(message); }));
            }
            else
            {
                String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                String frListStr = replySplit[1].Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries)[1];
                List<Friend> friends = MessageProccesing.DeserializeObject(Options.GET_FRIENDS, frListStr) as List<Friend>;
                callbackFriendsList.Items.Clear();
                if (friends == null || friends.Count == 0)
                {
                    callbackFriendsList.Items.Add("Nie masz znajomych");
                    return;
                }

                foreach (Friend fr in friends)
                {
                    callbackFriendsList.Items.Add(fr.username);
                }
                //callbackFriendsList.Items.Add(s);
                //callbackFriendsList.TopIndex = callbackFriendsList.Items.Count - (callbackFriendsList.Height / callbackFriendsList.ItemHeight);
            }
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

        }
    }
}