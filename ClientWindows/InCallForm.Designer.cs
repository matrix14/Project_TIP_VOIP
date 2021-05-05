
namespace ClientWindows
{
    partial class InCallForm
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
            this.activeCallWith_label = new System.Windows.Forms.Label();
            this.callUsersList_label = new System.Windows.Forms.Label();
            this.leaveCall_button = new System.Windows.Forms.Button();
            this.muteMicrophone_button = new System.Windows.Forms.Button();
            this.microState_label = new System.Windows.Forms.Label();
            this.soundState_label = new System.Windows.Forms.Label();
            this.muteSound_button = new System.Windows.Forms.Button();
            this.soundSettings_button = new System.Windows.Forms.Button();
            this.addUserCall_button = new System.Windows.Forms.Button();
            this.incomingTraffic_label = new System.Windows.Forms.Label();
            this.incomingTraffic_bar = new System.Windows.Forms.ProgressBar();
            this.incomingMsg_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // activeCallWith_label
            // 
            this.activeCallWith_label.AutoSize = true;
            this.activeCallWith_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.activeCallWith_label.Location = new System.Drawing.Point(13, 13);
            this.activeCallWith_label.Name = "activeCallWith_label";
            this.activeCallWith_label.Size = new System.Drawing.Size(220, 25);
            this.activeCallWith_label.TabIndex = 0;
            this.activeCallWith_label.Text = "Aktywne połączenie z";
            // 
            // callUsersList_label
            // 
            this.callUsersList_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.callUsersList_label.Location = new System.Drawing.Point(14, 48);
            this.callUsersList_label.Name = "callUsersList_label";
            this.callUsersList_label.Size = new System.Drawing.Size(219, 88);
            this.callUsersList_label.TabIndex = 1;
            this.callUsersList_label.Text = "username1, username2, username3, username4, username1, username2, username3, user" +
    "name4, username1, username2, username3, username4, username1, username2, usernam" +
    "e3, username4, ";
            this.callUsersList_label.Click += new System.EventHandler(this.callUsersList_label_Click);
            // 
            // leaveCall_button
            // 
            this.leaveCall_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.leaveCall_button.Location = new System.Drawing.Point(239, 13);
            this.leaveCall_button.Name = "leaveCall_button";
            this.leaveCall_button.Size = new System.Drawing.Size(155, 33);
            this.leaveCall_button.TabIndex = 2;
            this.leaveCall_button.Text = "Rozłącz się";
            this.leaveCall_button.UseVisualStyleBackColor = true;
            this.leaveCall_button.Click += new System.EventHandler(this.leaveCall_button_Click);
            // 
            // muteMicrophone_button
            // 
            this.muteMicrophone_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.muteMicrophone_button.Location = new System.Drawing.Point(239, 52);
            this.muteMicrophone_button.Name = "muteMicrophone_button";
            this.muteMicrophone_button.Size = new System.Drawing.Size(155, 33);
            this.muteMicrophone_button.TabIndex = 3;
            this.muteMicrophone_button.Text = "Mikrofon";
            this.muteMicrophone_button.UseVisualStyleBackColor = true;
            // 
            // microState_label
            // 
            this.microState_label.AutoSize = true;
            this.microState_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.microState_label.ForeColor = System.Drawing.Color.Red;
            this.microState_label.Location = new System.Drawing.Point(400, 56);
            this.microState_label.Name = "microState_label";
            this.microState_label.Size = new System.Drawing.Size(41, 24);
            this.microState_label.TabIndex = 4;
            this.microState_label.Text = "ON";
            // 
            // soundState_label
            // 
            this.soundState_label.AutoSize = true;
            this.soundState_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.soundState_label.ForeColor = System.Drawing.Color.Red;
            this.soundState_label.Location = new System.Drawing.Point(400, 95);
            this.soundState_label.Name = "soundState_label";
            this.soundState_label.Size = new System.Drawing.Size(41, 24);
            this.soundState_label.TabIndex = 6;
            this.soundState_label.Text = "ON";
            // 
            // muteSound_button
            // 
            this.muteSound_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.muteSound_button.Location = new System.Drawing.Point(239, 91);
            this.muteSound_button.Name = "muteSound_button";
            this.muteSound_button.Size = new System.Drawing.Size(155, 33);
            this.muteSound_button.TabIndex = 5;
            this.muteSound_button.Text = "Dźwięk";
            this.muteSound_button.UseVisualStyleBackColor = true;
            // 
            // soundSettings_button
            // 
            this.soundSettings_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.soundSettings_button.Location = new System.Drawing.Point(239, 130);
            this.soundSettings_button.Name = "soundSettings_button";
            this.soundSettings_button.Size = new System.Drawing.Size(155, 33);
            this.soundSettings_button.TabIndex = 7;
            this.soundSettings_button.Text = "Ustawienia";
            this.soundSettings_button.UseVisualStyleBackColor = true;
            // 
            // addUserCall_button
            // 
            this.addUserCall_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.addUserCall_button.Location = new System.Drawing.Point(239, 169);
            this.addUserCall_button.Name = "addUserCall_button";
            this.addUserCall_button.Size = new System.Drawing.Size(155, 33);
            this.addUserCall_button.TabIndex = 8;
            this.addUserCall_button.Text = "Dodaj uczestnika";
            this.addUserCall_button.UseVisualStyleBackColor = true;
            // 
            // incomingTraffic_label
            // 
            this.incomingTraffic_label.AutoSize = true;
            this.incomingTraffic_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.incomingTraffic_label.Location = new System.Drawing.Point(12, 175);
            this.incomingTraffic_label.Name = "incomingTraffic_label";
            this.incomingTraffic_label.Size = new System.Drawing.Size(124, 20);
            this.incomingTraffic_label.TabIndex = 9;
            this.incomingTraffic_label.Text = "Stan połączenia";
            this.incomingTraffic_label.Click += new System.EventHandler(this.incomingTraffic_label_Click);
            // 
            // incomingTraffic_bar
            // 
            this.incomingTraffic_bar.Location = new System.Drawing.Point(142, 175);
            this.incomingTraffic_bar.MarqueeAnimationSpeed = 23;
            this.incomingTraffic_bar.Maximum = 1;
            this.incomingTraffic_bar.MaximumSize = new System.Drawing.Size(23, 23);
            this.incomingTraffic_bar.MinimumSize = new System.Drawing.Size(23, 23);
            this.incomingTraffic_bar.Name = "incomingTraffic_bar";
            this.incomingTraffic_bar.Size = new System.Drawing.Size(23, 23);
            this.incomingTraffic_bar.Step = 1;
            this.incomingTraffic_bar.TabIndex = 0;
            // 
            // incomingMsg_label
            // 
            this.incomingMsg_label.Location = new System.Drawing.Point(13, 149);
            this.incomingMsg_label.Name = "incomingMsg_label";
            this.incomingMsg_label.Size = new System.Drawing.Size(200, 13);
            this.incomingMsg_label.TabIndex = 10;
            this.incomingMsg_label.Text = "label1";
            // 
            // InCallForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 212);
            this.Controls.Add(this.incomingMsg_label);
            this.Controls.Add(this.incomingTraffic_bar);
            this.Controls.Add(this.incomingTraffic_label);
            this.Controls.Add(this.addUserCall_button);
            this.Controls.Add(this.soundSettings_button);
            this.Controls.Add(this.soundState_label);
            this.Controls.Add(this.muteSound_button);
            this.Controls.Add(this.microState_label);
            this.Controls.Add(this.muteMicrophone_button);
            this.Controls.Add(this.leaveCall_button);
            this.Controls.Add(this.callUsersList_label);
            this.Controls.Add(this.activeCallWith_label);
            this.Name = "InCallForm";
            this.Text = "Aktywne połączenie";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InCallForm_FormClosing);
            this.Load += new System.EventHandler(this.InCallForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label activeCallWith_label;
        private System.Windows.Forms.Label callUsersList_label;
        private System.Windows.Forms.Button leaveCall_button;
        private System.Windows.Forms.Button muteMicrophone_button;
        private System.Windows.Forms.Label microState_label;
        private System.Windows.Forms.Label soundState_label;
        private System.Windows.Forms.Button muteSound_button;
        private System.Windows.Forms.Button soundSettings_button;
        private System.Windows.Forms.Button addUserCall_button;
        private System.Windows.Forms.Label incomingTraffic_label;
        private System.Windows.Forms.ProgressBar incomingTraffic_bar;
        private System.Windows.Forms.Label incomingMsg_label;
    }
}