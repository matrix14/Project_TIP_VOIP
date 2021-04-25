
namespace ClientWindows
{
    partial class SelectInvitationForm
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
            this.username_label = new System.Windows.Forms.Label();
            this.acceptFriend_button = new System.Windows.Forms.Button();
            this.userNameLabel = new System.Windows.Forms.Label();
            this.declineFriend_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // username_label
            // 
            this.username_label.AutoSize = true;
            this.username_label.Location = new System.Drawing.Point(13, 13);
            this.username_label.Name = "username_label";
            this.username_label.Size = new System.Drawing.Size(102, 13);
            this.username_label.TabIndex = 0;
            this.username_label.Text = "Nazwa użytkownika";
            // 
            // acceptFriend_button
            // 
            this.acceptFriend_button.Location = new System.Drawing.Point(16, 53);
            this.acceptFriend_button.Name = "acceptFriend_button";
            this.acceptFriend_button.Size = new System.Drawing.Size(130, 23);
            this.acceptFriend_button.TabIndex = 3;
            this.acceptFriend_button.Text = "Zaakceptuj";
            this.acceptFriend_button.UseVisualStyleBackColor = true;
            this.acceptFriend_button.Click += new System.EventHandler(this.acceptFriend_button_Click);
            // 
            // userNameLabel
            // 
            this.userNameLabel.AutoSize = true;
            this.userNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.userNameLabel.Location = new System.Drawing.Point(16, 30);
            this.userNameLabel.Name = "userNameLabel";
            this.userNameLabel.Size = new System.Drawing.Size(94, 20);
            this.userNameLabel.TabIndex = 4;
            this.userNameLabel.Text = "NO_NAME";
            // 
            // declineFriend_button
            // 
            this.declineFriend_button.Location = new System.Drawing.Point(149, 53);
            this.declineFriend_button.Name = "declineFriend_button";
            this.declineFriend_button.Size = new System.Drawing.Size(130, 23);
            this.declineFriend_button.TabIndex = 5;
            this.declineFriend_button.Text = "Odrzuć";
            this.declineFriend_button.UseVisualStyleBackColor = true;
            this.declineFriend_button.Click += new System.EventHandler(this.declineFriend_button_Click);
            // 
            // SelectInvitationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 81);
            this.Controls.Add(this.declineFriend_button);
            this.Controls.Add(this.userNameLabel);
            this.Controls.Add(this.acceptFriend_button);
            this.Controls.Add(this.username_label);
            this.Name = "SelectInvitationForm";
            this.Text = "Przychodzące zaproszenie";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label username_label;
        private System.Windows.Forms.Button acceptFriend_button;
        private System.Windows.Forms.Label userNameLabel;
        private System.Windows.Forms.Button declineFriend_button;
    }
}