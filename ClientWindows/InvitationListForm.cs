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
    public partial class InvitationListForm : Form
    {
        private List<Invitation> processingInvitation = new List<Invitation>();
        private List<Invitation> invitationContainer = new List<Invitation>();
        public InvitationListForm()
        {
            InitializeComponent();
        }

        private void updateInvitations()
        {
            if(this.invitationsDataGrid.InvokeRequired)
            {
                this.invitationsDataGrid.Invoke(new MethodInvoker(() => { updateInvitations(); }));
                return;
            }

            this.invitationsDataGrid.Rows.Clear();
            foreach (Invitation i in invitationContainer)
            {
                Button acceptButton = new Button();
                acceptButton.Text = "Zaakceptuj";
                Button declineButton = new Button();
                declineButton.Text = "Odrzuc";
                this.invitationsDataGrid.Rows.Add(i);
            }
        }

        public InvitationListForm(List<Invitation> invitations)
        {
            InitializeComponent();
            this.invitationContainer = invitations;
            updateInvitations();
        }

        private void invitationsDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == invitationsDataGrid.Columns["acceptButtons"].Index)
            {
                Invitation inv = (Invitation)invitationsDataGrid[0, e.RowIndex].Value;
                if(processingInvitation.Contains(inv))
                    return;
                processingInvitation.Add(inv);
                LoggedInService.acceptInvitation(inv);
                removeInvitationList(inv);
            }
            else if (e.ColumnIndex == invitationsDataGrid.Columns["declineButtons"].Index)
            {
                Invitation inv = (Invitation)invitationsDataGrid[0, e.RowIndex].Value;
                if (processingInvitation.Contains(inv))
                    return;
                processingInvitation.Add(inv);
                LoggedInService.declineInvitation(inv);
                removeInvitationList(inv);
            }
        }

        private void removeInvitationList(Invitation inv)
        {
            if (this.invitationContainer.Contains(inv))
            {
                this.invitationContainer.Remove(inv);
                updateInvitations();
            }

        }

        public void updateInvitationList(List<Invitation> invitations)
        {
            foreach(Invitation i in processingInvitation)
            {
                if (!invitations.Contains(i))
                    processingInvitation.Remove(i);
            }
            this.invitationContainer = invitations;
            updateInvitations();
        }

        private void InvitationListForm_Load(object sender, EventArgs e)
        {

        }
    }
}
