using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientWindows
{
    public partial class AddFriendForm : Form
    {
        private System.Timers.Timer checkUsernameTimer = new System.Timers.Timer();
        private Boolean checkUsernameTimerStopped = true;
        public AddFriendForm()
        {
            checkUsernameTimer.Interval = 150;
            checkUsernameTimer.Elapsed += usernameCheckOnTimerElapsed;
            checkUsernameTimer.AutoReset = false;
            BooleanCallback callback = usernameCheckUpdateInfo;
            LoggedInService.CheckUsernameCallback = callback;
            InitializeComponent();

            this.addFriend_button.Enabled = false;
            this.username_input.Enabled = true;
            this.ActiveControl = this.username_input;
        }

        private void addFriend_button_Click(object sender, EventArgs e)
        {
            LoggedInService.addFriend(this.username_input.Text);
            this.addFriend_button.Enabled = false;
            this.Close();
        }
        private void usernameCheckOnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (checkUsernameTimerStopped == false)
            {
                checkUsernameTimer.Stop();
                checkUsernameTimerStopped = true;
                LoggedInService.checkIsUserExist(this.username_input.Text);

            }
        }

        private void username_input_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.username_input.Text.Length >= 3)
            {
                checkUsernameTimer.Stop();
                checkUsernameTimer.Start();
                checkUsernameTimerStopped = false;
            }
        }

        public void usernameCheckUpdateInfo(Boolean exist) //TODO: sometimes it get stuck, and not showing if user exist
        {
            if(this.isUserExists_label.InvokeRequired)
            {
                this.isUserExists_label.Invoke(new MethodInvoker(() => { usernameCheckUpdateInfo(exist); }));
                return;
            }
            if (exist)
            { 
                this.isUserExists_label.Text = "Znaleziono użytkownika!";
                this.isUserExists_label.ForeColor = Color.Green;
                this.addFriend_button.Enabled = true;
            }
            else
            {
                this.isUserExists_label.Text = "Nie znaleziono użytkownika!";
                this.isUserExists_label.ForeColor = Color.Red;
                this.addFriend_button.Enabled = false;
            }
        }

        private void AddFriendForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            checkUsernameTimer.Stop();
            checkUsernameTimerStopped = true;
        }

        private void AddFriendForm_Load(object sender, EventArgs e)
        {

        }
    }
}
