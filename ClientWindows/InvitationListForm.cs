using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;

namespace ClientWindows
{
    public partial class InvitationListForm : Form
    {
        private List<Invitation> processingInvitation = new List<Invitation>();
        private List<Invitation> invitationContainer = new List<Invitation>();

        private static ManualResetEvent processingInvitationList = new ManualResetEvent(true);
        private static ManualResetEvent processingProcessingList = new ManualResetEvent(true);

        private Boolean isActive = false;
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
            processingInvitationList.WaitOne();
            processingInvitationList.Reset();
            foreach (Invitation i in invitationContainer)
            {
                Button acceptButton = new Button();
                acceptButton.Text = "Zaakceptuj";
                Button declineButton = new Button();
                declineButton.Text = "Odrzuc";
                this.invitationsDataGrid.Rows.Add(i);
            }
            processingInvitationList.Set();
        }

        public InvitationListForm(List<Invitation> invitations)
        {
            InitializeComponent();
            processingInvitationList.WaitOne();
            processingInvitationList.Reset();
            this.invitationContainer = invitations;
            processingInvitationList.Set();
            updateInvitations();
        }

        private void invitationsDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == invitationsDataGrid.Columns["acceptButtons"].Index)
            {
                Invitation inv = (Invitation)invitationsDataGrid[0, e.RowIndex].Value;
                if(processingInvitation.Contains(inv))
                    return;
                processingProcessingList.WaitOne();
                processingProcessingList.Reset();
                processingInvitation.Add(inv);
                processingProcessingList.Set();
                LoggedInService.acceptInvitation(inv);
                removeInvitationList(inv);
            }
            else if (e.ColumnIndex == invitationsDataGrid.Columns["declineButtons"].Index)
            {
                Invitation inv = (Invitation)invitationsDataGrid[0, e.RowIndex].Value;
                if (processingInvitation.Contains(inv))
                    return;
                processingProcessingList.WaitOne();
                processingProcessingList.Reset();
                processingInvitation.Add(inv);
                processingProcessingList.Set();
                LoggedInService.declineInvitation(inv);
                removeInvitationList(inv);
            }
        }

        private void removeInvitationList(Invitation inv)
        {
            
            if (this.invitationContainer.Contains(inv))
            {
                processingInvitationList.WaitOne();
                processingInvitationList.Reset();
                this.invitationContainer.Remove(inv);
                processingInvitationList.Set();
                updateInvitations();
            }
        }

        public void updateInvitationList(List<Invitation> invitations)
        {
            if (!isActive)
                return;
            List<int> toRemove = new List<int>();
            processingProcessingList.WaitOne();
            processingProcessingList.Reset();
            for(int j = 0; j<processingInvitation.Count; j++)
            {
                Invitation i = processingInvitation.ElementAt(j);
                if (!invitations.Contains(i))
                    toRemove.Add(j);
                    //processingInvitation.Remove(i);
            }
            foreach (int i in toRemove)
            {
                processingInvitation.RemoveAt(i);
            }
            processingProcessingList.Set();
            processingInvitationList.WaitOne();
            processingInvitationList.Reset();
            this.invitationContainer = invitations;
            processingInvitationList.Set();
            updateInvitations();
        }

        private void InvitationListForm_Load(object sender, EventArgs e)
        {
            isActive = true;
        }

        private void InvitationListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isActive = false;
        }
    }
}
