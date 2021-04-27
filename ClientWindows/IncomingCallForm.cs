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
        public IncomingCallForm()
        {
            InitializeComponent();
        }

        public IncomingCallForm(Call call)
        {
            InitializeComponent();
            this.call = call;
            this.callId.Text = call.callId.ToString(); //TODO: temporary
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
            LoggedInService.acceptCall(this.call);
            //TODO open call window
            this.Close();
        }

        private void declineCall_button_Click(object sender, EventArgs e)
        {
            if (this.call == null) return;
            LoggedInService.declineCall(this.call);
            this.Close();
        }
    }
}
