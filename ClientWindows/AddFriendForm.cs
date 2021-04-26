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
        private Boolean registerSwitch = false; //TODO temporary
        public AddFriendForm()
        {
            InitializeComponent();
        }

        private void addFriend_button_Click(object sender, EventArgs e)
        {
            LoggedInService.addFriend(this.username_input.Text);
            this.Close();
        }

        private void username_input_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.username_input.Text.Length >= 3)
            {
                if (registerSwitch)
                { //TODO temporary
                    this.isUserExists_label.Text = "Nie znaleziono użytkownika!";
                    this.isUserExists_label.ForeColor = Color.Red;
                    registerSwitch = false;
                }
                else
                {
                    this.isUserExists_label.Text = "Znaleziono użytkownika!";
                    this.isUserExists_label.ForeColor = Color.Green;
                    registerSwitch = true;
                }
            }
        }

        public void usernameCheckUpdateInfo(Boolean exist)
        {
            if (exist)
            { 
                this.isUserExists_label.Text = "Znaleziono użytkownika!";
                this.isUserExists_label.ForeColor = Color.Green;
            }
            else
            {
                this.isUserExists_label.Text = "Nie znaleziono użytkownika!";
                this.isUserExists_label.ForeColor = Color.Red;
            }
        }
    }
}
