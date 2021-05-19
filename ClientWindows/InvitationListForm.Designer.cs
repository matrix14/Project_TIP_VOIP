
namespace ClientWindows
{
    partial class InvitationListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.invitationsDataGrid = new System.Windows.Forms.DataGridView();
            this.usernames = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.acceptButtons = new System.Windows.Forms.DataGridViewButtonColumn();
            this.declineButtons = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.invitationsDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // invitationsDataGrid
            // 
            this.invitationsDataGrid.AllowUserToAddRows = false;
            this.invitationsDataGrid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.invitationsDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.invitationsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.invitationsDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.usernames,
            this.acceptButtons,
            this.declineButtons});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.invitationsDataGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.invitationsDataGrid.Location = new System.Drawing.Point(12, 12);
            this.invitationsDataGrid.Name = "invitationsDataGrid";
            this.invitationsDataGrid.RowHeadersVisible = false;
            this.invitationsDataGrid.RowTemplate.Height = 35;
            this.invitationsDataGrid.Size = new System.Drawing.Size(387, 426);
            this.invitationsDataGrid.TabIndex = 0;
            this.invitationsDataGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.invitationsDataGrid_CellContentClick);
            // 
            // usernames
            // 
            this.usernames.HeaderText = "Nazwa użytkownika";
            this.usernames.MaxInputLength = 50;
            this.usernames.MinimumWidth = 163;
            this.usernames.Name = "usernames";
            this.usernames.ReadOnly = true;
            this.usernames.Width = 163;
            // 
            // acceptButtons
            // 
            this.acceptButtons.HeaderText = "Zaakceptuj";
            this.acceptButtons.MinimumWidth = 110;
            this.acceptButtons.Name = "acceptButtons";
            this.acceptButtons.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.acceptButtons.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.acceptButtons.Text = "Zaakceptuj";
            this.acceptButtons.UseColumnTextForButtonValue = true;
            this.acceptButtons.Width = 110;
            // 
            // declineButtons
            // 
            this.declineButtons.HeaderText = "Odrzuć";
            this.declineButtons.MinimumWidth = 110;
            this.declineButtons.Name = "declineButtons";
            this.declineButtons.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.declineButtons.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.declineButtons.Text = "Odrzuć";
            this.declineButtons.UseColumnTextForButtonValue = true;
            this.declineButtons.Width = 110;
            // 
            // InvitationListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 450);
            this.Controls.Add(this.invitationsDataGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "InvitationListForm";
            this.Text = "Zaproszenia";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InvitationListForm_FormClosing);
            this.Load += new System.EventHandler(this.InvitationListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.invitationsDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView invitationsDataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn usernames;
        private System.Windows.Forms.DataGridViewButtonColumn acceptButtons;
        private System.Windows.Forms.DataGridViewButtonColumn declineButtons;
    }
}