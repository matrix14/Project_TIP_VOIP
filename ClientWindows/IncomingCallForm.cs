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
    public partial class IncomingCallForm : Form
    {
        private Call call = null;
        private CallCallback openC = null;
        private Boolean acceptedCall = false;
        public IncomingCallForm()
        {
            InitializeComponent();
        }

        public IncomingCallForm(Call call, CallCallback ccb)
        {
            InitializeComponent();
            this.openC = ccb;
            this.call = call;
            String usersListStr = "";
            foreach(String u in call.usernames)
            {
                if(!usersListStr.Equals(""))
                {
                    usersListStr += ", ";
                }
                usersListStr += u;
            }
            this.usersList_label.Text = usersListStr;
            if(this.call.usernames.Count==1)
            {
                this.username_label.Text = "Użytkownik:";
            } else
            {
                this.username_label.Text = "Użytkownicy:";
            }
        }

        private void acceptCall_button_Click(object sender, EventArgs e)
        {
            if (this.call == null) return;
            acceptedCall = true;
            LoggedInService.acceptCall(new Id(this.call.callId));

            openC(this.call);
            this.Close();
        }

        private void declineCall_button_Click(object sender, EventArgs e)
        {
            if (this.call == null) return;
            LoggedInService.declineCall(new Id(this.call.callId));
            this.call = null;
            this.Close();
        }

        private void IncomingCallForm_Load(object sender, EventArgs e)
        {

        }

        private void IncomingCallForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.call == null) return;
            if(!acceptedCall)
                LoggedInService.declineCall(new Id(this.call.callId));
            this.call = null;
        }
    }
}
