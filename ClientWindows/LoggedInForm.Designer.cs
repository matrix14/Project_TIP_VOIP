
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
            this.signedIn_Text = new System.Windows.Forms.ToolStripStatusLabel();
            this.signedInLogin_Text = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.closeApp_button = new System.Windows.Forms.ToolStripButton();
            this.addFriend_button = new System.Windows.Forms.ToolStripButton();
            this.friendList_label = new System.Windows.Forms.Label();
            this.openFriend_button = new System.Windows.Forms.Button();
            this.friendsList = new System.Windows.Forms.ListBox();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.signedIn_Text,
            this.signedInLogin_Text});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
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
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFriend_button,
            this.closeApp_button});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.friendsList);
            this.splitContainer1.Panel1.Controls.Add(this.openFriend_button);
            this.splitContainer1.Panel1.Controls.Add(this.friendList_label);
            this.splitContainer1.Size = new System.Drawing.Size(800, 403);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 2;
            // 
            // closeApp_button
            // 
            this.closeApp_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.closeApp_button.Image = ((System.Drawing.Image)(resources.GetObject("closeApp_button.Image")));
            this.closeApp_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.closeApp_button.Name = "closeApp_button";
            this.closeApp_button.Size = new System.Drawing.Size(55, 22);
            this.closeApp_button.Text = "Zakończ";
            // 
            // addFriend_button
            // 
            this.addFriend_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addFriend_button.Image = ((System.Drawing.Image)(resources.GetObject("addFriend_button.Image")));
            this.addFriend_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addFriend_button.Name = "addFriend_button";
            this.addFriend_button.Size = new System.Drawing.Size(104, 22);
            this.addFriend_button.Text = "Dodaj znajomego";
            // 
            // friendList_label
            // 
            this.friendList_label.AutoSize = true;
            this.friendList_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.friendList_label.Location = new System.Drawing.Point(13, 13);
            this.friendList_label.Name = "friendList_label";
            this.friendList_label.Size = new System.Drawing.Size(81, 25);
            this.friendList_label.TabIndex = 0;
            this.friendList_label.Text = "Znajomi";
            // 
            // openFriend_button
            // 
            this.openFriend_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.openFriend_button.Location = new System.Drawing.Point(3, 366);
            this.openFriend_button.Name = "openFriend_button";
            this.openFriend_button.Size = new System.Drawing.Size(260, 34);
            this.openFriend_button.TabIndex = 1;
            this.openFriend_button.Text = "Otwórz";
            this.openFriend_button.UseVisualStyleBackColor = true;
            this.openFriend_button.Click += new System.EventHandler(this.openFriend_button_Click);
            // 
            // friendsList
            // 
            this.friendsList.FormattingEnabled = true;
            this.friendsList.Items.AddRange(new object[] {
            "Wczytywanie..."});
            this.friendsList.Location = new System.Drawing.Point(3, 42);
            this.friendsList.Name = "friendsList";
            this.friendsList.Size = new System.Drawing.Size(260, 316);
            this.friendsList.TabIndex = 2;
            // 
            // LoggedInForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "LoggedInForm";
            this.Text = "TIP_VOIP Client";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
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
    }
}