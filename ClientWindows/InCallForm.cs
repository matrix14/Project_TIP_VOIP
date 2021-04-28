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

        private Username username = null;
        private Call call = null;

        public InCallForm()
        {
            InitializeComponent();
        }

        public InCallForm(Call c)
        {
            this.call = c;
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

        public InCallForm(Username u)
        {
            this.username = u;
            InitializeComponent();
        }

        private void InCallForm_Load(object sender, EventArgs e)
        {

        }

        private void leaveCall_button_Click(object sender, EventArgs e)
        {
            if (this.call == null) return;
            LoggedInService.declineCall(this.call);
        }
    }
}
