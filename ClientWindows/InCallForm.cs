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
    public partial class InCallForm : Form
    {

        private Id callId = null;
        private Call call = null;

        public InCallForm()
        {
            InitializeComponent();
        }

        public InCallForm(Call c)
        {
            this.call = c;
            this.callId = new Id(c.callId);
            InitializeComponent();
            String usersListStr = "";
            foreach (String u in call.usernames)
            {
                if (!usersListStr.Equals(""))
                {
                    usersListStr += ", ";
                }
                usersListStr += u;
            }
            this.callUsersList_label.Text = usersListStr;
        }

        public InCallForm(Id id, Username user)
        {
            this.callId = id;
            InitializeComponent();
            this.callUsersList_label.Text = user.username;
        }

        private void InCallForm_Load(object sender, EventArgs e)
        {

        }

        private void leaveCall_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InCallForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.callId == null) return;
            LoggedInService.declineCall(this.callId);
        }
    }
}
