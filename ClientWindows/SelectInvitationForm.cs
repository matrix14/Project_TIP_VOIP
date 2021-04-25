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
    public partial class SelectInvitationForm : Form
    {
        private Invitation invitation;
        public SelectInvitationForm()
        {
            InitializeComponent();
        }

        public SelectInvitationForm(Invitation inv)
        {
            this.invitation = inv;
            InitializeComponent();
            this.userNameLabel.Text = this.invitation.username;
        }

        private void acceptFriend_button_Click(object sender, EventArgs e)
        {
            if (this.invitation != null)
            {
                LoggedInService.acceptInvitation(this.invitation);
                //LoggedInForm.removeFromInvitingList(this.invitation);
            }
            this.Close();
        }

        private void declineFriend_button_Click(object sender, EventArgs e)
        {
            if (this.invitation != null)
            {
                LoggedInService.declineInvitation(this.invitation);
                //LoggedInForm.removeFromInvitingList(this.invitation);
            }
            this.Close();
        }
    }
}
