
namespace ClientWindows
{
    partial class AddFriendForm
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
            this.username_input = new System.Windows.Forms.TextBox();
            this.isUserExists_label = new System.Windows.Forms.Label();
            this.addFriend_button = new System.Windows.Forms.Button();
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
            // username_input
            // 
            this.username_input.Enabled = false;
            this.username_input.Location = new System.Drawing.Point(16, 29);
            this.username_input.Name = "username_input";
            this.username_input.Size = new System.Drawing.Size(263, 20);
            this.username_input.TabIndex = 1;
            this.username_input.KeyUp += new System.Windows.Forms.KeyEventHandler(this.username_input_KeyUp);
            // 
            // isUserExists_label
            // 
            this.isUserExists_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.isUserExists_label.ForeColor = System.Drawing.Color.Red;
            this.isUserExists_label.Location = new System.Drawing.Point(13, 52);
            this.isUserExists_label.Name = "isUserExists_label";
            this.isUserExists_label.Size = new System.Drawing.Size(263, 13);
            this.isUserExists_label.TabIndex = 2;
            this.isUserExists_label.Text = "Nie znaleziono użytkownika!";
            this.isUserExists_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // addFriend_button
            // 
            this.addFriend_button.Enabled = false;
            this.addFriend_button.Location = new System.Drawing.Point(16, 69);
            this.addFriend_button.Name = "addFriend_button";
            this.addFriend_button.Size = new System.Drawing.Size(263, 23);
            this.addFriend_button.TabIndex = 3;
            this.addFriend_button.Text = "Dodaj znajomego";
            this.addFriend_button.UseVisualStyleBackColor = true;
            this.addFriend_button.Click += new System.EventHandler(this.addFriend_button_Click);
            // 
            // AddFriendForm
            // 
            this.AcceptButton = this.addFriend_button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 111);
            this.Controls.Add(this.addFriend_button);
            this.Controls.Add(this.isUserExists_label);
            this.Controls.Add(this.username_input);
            this.Controls.Add(this.username_label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AddFriendForm";
            this.Text = "Dodaj znajomego";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddFriendForm_FormClosing);
            this.Load += new System.EventHandler(this.AddFriendForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label username_label;
        private System.Windows.Forms.TextBox username_input;
        private System.Windows.Forms.Label isUserExists_label;
        private System.Windows.Forms.Button addFriend_button;
    }
}