
namespace ClientWindows
{
    partial class IncomingCallForm
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
            this.acceptCall_button = new System.Windows.Forms.Button();
            this.declineCall_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.username_label = new System.Windows.Forms.Label();
            this.usersList_label = new System.Windows.Forms.Label();
            this.callId = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // acceptCall_button
            // 
            this.acceptCall_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.acceptCall_button.Location = new System.Drawing.Point(12, 122);
            this.acceptCall_button.Name = "acceptCall_button";
            this.acceptCall_button.Size = new System.Drawing.Size(139, 49);
            this.acceptCall_button.TabIndex = 0;
            this.acceptCall_button.Text = "Zaakceptuj";
            this.acceptCall_button.UseVisualStyleBackColor = true;
            this.acceptCall_button.Click += new System.EventHandler(this.acceptCall_button_Click);
            // 
            // declineCall_button
            // 
            this.declineCall_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.declineCall_button.Location = new System.Drawing.Point(157, 122);
            this.declineCall_button.Name = "declineCall_button";
            this.declineCall_button.Size = new System.Drawing.Size(139, 49);
            this.declineCall_button.TabIndex = 1;
            this.declineCall_button.Text = "Odrzuć";
            this.declineCall_button.UseVisualStyleBackColor = true;
            this.declineCall_button.Click += new System.EventHandler(this.declineCall_button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(281, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Przychodzące połączenie";
            // 
            // username_label
            // 
            this.username_label.AutoSize = true;
            this.username_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.username_label.Location = new System.Drawing.Point(18, 42);
            this.username_label.Name = "username_label";
            this.username_label.Size = new System.Drawing.Size(118, 24);
            this.username_label.TabIndex = 3;
            this.username_label.Text = "Użytkownicy:";
            // 
            // usersList_label
            // 
            this.usersList_label.Location = new System.Drawing.Point(15, 66);
            this.usersList_label.Name = "usersList_label";
            this.usersList_label.Size = new System.Drawing.Size(275, 44);
            this.usersList_label.TabIndex = 4;
            this.usersList_label.Text = "username1, username2, username3, username4, username1, username2, username3, user" +
    "name4, username1, username2, username3, username4";
            // 
            // callId
            // 
            this.callId.AutoSize = true;
            this.callId.Location = new System.Drawing.Point(9, 0);
            this.callId.Name = "callId";
            this.callId.Size = new System.Drawing.Size(13, 13);
            this.callId.TabIndex = 5;
            this.callId.Text = "0";
            // 
            // IncomingCallForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 183);
            this.Controls.Add(this.callId);
            this.Controls.Add(this.usersList_label);
            this.Controls.Add(this.username_label);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.declineCall_button);
            this.Controls.Add(this.acceptCall_button);
            this.Name = "IncomingCallForm";
            this.Text = "Przychodzące połączenie";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptCall_button;
        private System.Windows.Forms.Button declineCall_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label username_label;
        private System.Windows.Forms.Label usersList_label;
        private System.Windows.Forms.Label callId;
    }
}