
namespace ClientWindows
{
    partial class LoggedInForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoggedInForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.connectionStatus_label = new System.Windows.Forms.ToolStripStatusLabel();
            this.signedIn_Text = new System.Windows.Forms.ToolStripStatusLabel();
            this.signedInLogin_Text = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.addFriend_button = new System.Windows.Forms.ToolStripButton();
            this.closeApp_button = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.invitingList_button = new System.Windows.Forms.Button();
            this.friendsList = new System.Windows.Forms.ListBox();
            this.openFriend_button = new System.Windows.Forms.Button();
            this.friendList_label = new System.Windows.Forms.Label();
            this.activeFriendStatus_Label = new System.Windows.Forms.Label();
            this.callUser = new System.Windows.Forms.Button();
            this.activeUserWindow = new System.Windows.Forms.Label();
            this.callingStatusLabel = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectionStatus_label,
            this.signedIn_Text,
            this.signedInLogin_Text});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(539, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // connectionStatus_label
            // 
            this.connectionStatus_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.connectionStatus_label.ForeColor = System.Drawing.Color.Green;
            this.connectionStatus_label.Name = "connectionStatus_label";
            this.connectionStatus_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.connectionStatus_label.Size = new System.Drawing.Size(64, 17);
            this.connectionStatus_label.Text = "Połączono";
            // 
            // signedIn_Text
            // 
            this.signedIn_Text.Name = "signedIn_Text";
            this.signedIn_Text.Size = new System.Drawing.Size(101, 17);
            this.signedIn_Text.Text = "Zalogowano jako:";
            // 
            // signedInLogin_Text
            // 
            this.signedInLogin_Text.Name = "signedInLogin_Text";
            this.signedInLogin_Text.Size = new System.Drawing.Size(64, 17);
            this.signedInLogin_Text.Text = "NO_NAME";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFriend_button,
            this.closeApp_button});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(539, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // addFriend_button
            // 
            this.addFriend_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addFriend_button.Image = ((System.Drawing.Image)(resources.GetObject("addFriend_button.Image")));
            this.addFriend_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addFriend_button.Name = "addFriend_button";
            this.addFriend_button.Size = new System.Drawing.Size(104, 22);
            this.addFriend_button.Text = "Dodaj znajomego";
            this.addFriend_button.Click += new System.EventHandler(this.addFriend_button_Click);
            // 
            // closeApp_button
            // 
            this.closeApp_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.closeApp_button.Image = ((System.Drawing.Image)(resources.GetObject("closeApp_button.Image")));
            this.closeApp_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.closeApp_button.Name = "closeApp_button";
            this.closeApp_button.Size = new System.Drawing.Size(55, 22);
            this.closeApp_button.Text = "Zakończ";
            this.closeApp_button.Click += new System.EventHandler(this.closeApp_button_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.MaximumSize = new System.Drawing.Size(534, 403);
            this.splitContainer1.MinimumSize = new System.Drawing.Size(534, 403);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.invitingList_button);
            this.splitContainer1.Panel1.Controls.Add(this.friendsList);
            this.splitContainer1.Panel1.Controls.Add(this.openFriend_button);
            this.splitContainer1.Panel1.Controls.Add(this.friendList_label);
            this.splitContainer1.Panel1MinSize = 265;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.callingStatusLabel);
            this.splitContainer1.Panel2.Controls.Add(this.activeFriendStatus_Label);
            this.splitContainer1.Panel2.Controls.Add(this.callUser);
            this.splitContainer1.Panel2.Controls.Add(this.activeUserWindow);
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Panel2MinSize = 265;
            this.splitContainer1.Size = new System.Drawing.Size(534, 403);
            this.splitContainer1.SplitterDistance = 265;
            this.splitContainer1.TabIndex = 2;
            // 
            // invitingList_button
            // 
            this.invitingList_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.invitingList_button.Location = new System.Drawing.Point(3, 364);
            this.invitingList_button.Name = "invitingList_button";
            this.invitingList_button.Size = new System.Drawing.Size(260, 34);
            this.invitingList_button.TabIndex = 3;
            this.invitingList_button.Text = "Zaproszenia (0)";
            this.invitingList_button.UseVisualStyleBackColor = true;
            this.invitingList_button.Click += new System.EventHandler(this.invitingList_button_Click);
            // 
            // friendsList
            // 
            this.friendsList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.friendsList.FormattingEnabled = true;
            this.friendsList.Items.AddRange(new object[] {
            "Wczytywanie..."});
            this.friendsList.Location = new System.Drawing.Point(3, 42);
            this.friendsList.Name = "friendsList";
            this.friendsList.Size = new System.Drawing.Size(260, 316);
            this.friendsList.TabIndex = 2;
            this.friendsList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.friendList_DrawItem);
            this.friendsList.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.friendList_MeasureItem);
            this.friendsList.SelectedIndexChanged += new System.EventHandler(this.friendsList_SelectedIndexChanged);
            // 
            // openFriend_button
            // 
            this.openFriend_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.openFriend_button.Location = new System.Drawing.Point(3, 326);
            this.openFriend_button.Name = "openFriend_button";
            this.openFriend_button.Size = new System.Drawing.Size(260, 34);
            this.openFriend_button.TabIndex = 1;
            this.openFriend_button.Text = "Otwórz";
            this.openFriend_button.UseVisualStyleBackColor = true;
            this.openFriend_button.Visible = false;
            this.openFriend_button.Click += new System.EventHandler(this.openFriend_button_Click);
            // 
            // friendList_label
            // 
            this.friendList_label.AutoSize = true;
            this.friendList_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.friendList_label.Location = new System.Drawing.Point(3, 13);
            this.friendList_label.Name = "friendList_label";
            this.friendList_label.Size = new System.Drawing.Size(81, 25);
            this.friendList_label.TabIndex = 0;
            this.friendList_label.Text = "Znajomi";
            // 
            // activeFriendStatus_Label
            // 
            this.activeFriendStatus_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.activeFriendStatus_Label.ForeColor = System.Drawing.Color.Red;
            this.activeFriendStatus_Label.Location = new System.Drawing.Point(3, 42);
            this.activeFriendStatus_Label.Name = "activeFriendStatus_Label";
            this.activeFriendStatus_Label.Size = new System.Drawing.Size(259, 31);
            this.activeFriendStatus_Label.TabIndex = 3;
            this.activeFriendStatus_Label.Text = "Nieaktywny";
            this.activeFriendStatus_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.activeFriendStatus_Label.Visible = false;
            // 
            // callUser
            // 
            this.callUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.callUser.Location = new System.Drawing.Point(2, 364);
            this.callUser.Name = "callUser";
            this.callUser.Size = new System.Drawing.Size(260, 34);
            this.callUser.TabIndex = 2;
            this.callUser.Text = "Zadzwoń";
            this.callUser.UseVisualStyleBackColor = true;
            this.callUser.Visible = false;
            this.callUser.Click += new System.EventHandler(this.callUser_Click);
            // 
            // activeUserWindow
            // 
            this.activeUserWindow.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.activeUserWindow.Location = new System.Drawing.Point(3, 7);
            this.activeUserWindow.Name = "activeUserWindow";
            this.activeUserWindow.Size = new System.Drawing.Size(259, 31);
            this.activeUserWindow.TabIndex = 0;
            this.activeUserWindow.Text = "Wczytywanie...";
            this.activeUserWindow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.activeUserWindow.Visible = false;
            this.activeUserWindow.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // callingStatusLabel
            // 
            this.callingStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.callingStatusLabel.ForeColor = System.Drawing.Color.Green;
            this.callingStatusLabel.Location = new System.Drawing.Point(1, 330);
            this.callingStatusLabel.Name = "callingStatusLabel";
            this.callingStatusLabel.Size = new System.Drawing.Size(259, 31);
            this.callingStatusLabel.TabIndex = 4;
            this.callingStatusLabel.Text = "Oczekuje na odpowiedź...";
            this.callingStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.callingStatusLabel.Visible = false;
            // 
            // LoggedInForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.MaximumSize = new System.Drawing.Size(555, 489);
            this.MinimumSize = new System.Drawing.Size(555, 489);
            this.Name = "LoggedInForm";
            this.Text = "TIP_VOIP Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoggedInForm_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel signedIn_Text;
        private System.Windows.Forms.ToolStripStatusLabel signedInLogin_Text;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton addFriend_button;
        private System.Windows.Forms.ToolStripButton closeApp_button;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label friendList_label;
        private System.Windows.Forms.Button openFriend_button;
        private System.Windows.Forms.ListBox friendsList;
        private System.Windows.Forms.Label activeUserWindow;
        private System.Windows.Forms.Button callUser;
        private System.Windows.Forms.Button invitingList_button;
        private System.Windows.Forms.ToolStripStatusLabel connectionStatus_label;
        private System.Windows.Forms.Label activeFriendStatus_Label;
        private System.Windows.Forms.Label callingStatusLabel;
    }
}