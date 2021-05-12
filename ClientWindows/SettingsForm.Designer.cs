
namespace ClientWindows
{
    partial class SettingsForm
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
            this.saveSettings_button = new System.Windows.Forms.Button();
            this.discardSettings_button = new System.Windows.Forms.Button();
            this.speakerDevice_label = new System.Windows.Forms.Label();
            this.inputDevice_label = new System.Windows.Forms.Label();
            this.inputDevices_Combo = new System.Windows.Forms.ComboBox();
            this.outputDevices_Combo = new System.Windows.Forms.ComboBox();
            this.serverAddress_label = new System.Windows.Forms.Label();
            this.serverAddress_Input = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // saveSettings_button
            // 
            this.saveSettings_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.saveSettings_button.Location = new System.Drawing.Point(17, 178);
            this.saveSettings_button.Name = "saveSettings_button";
            this.saveSettings_button.Size = new System.Drawing.Size(176, 38);
            this.saveSettings_button.TabIndex = 0;
            this.saveSettings_button.Text = "Zapisz ustawienia";
            this.saveSettings_button.UseVisualStyleBackColor = true;
            // 
            // discardSettings_button
            // 
            this.discardSettings_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.discardSettings_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.discardSettings_button.Location = new System.Drawing.Point(17, 222);
            this.discardSettings_button.Name = "discardSettings_button";
            this.discardSettings_button.Size = new System.Drawing.Size(176, 38);
            this.discardSettings_button.TabIndex = 1;
            this.discardSettings_button.Text = "Odrzuć";
            this.discardSettings_button.UseVisualStyleBackColor = true;
            // 
            // speakerDevice_label
            // 
            this.speakerDevice_label.AutoSize = true;
            this.speakerDevice_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.speakerDevice_label.Location = new System.Drawing.Point(13, 13);
            this.speakerDevice_label.Name = "speakerDevice_label";
            this.speakerDevice_label.Size = new System.Drawing.Size(66, 20);
            this.speakerDevice_label.TabIndex = 2;
            this.speakerDevice_label.Text = "Głośniki";
            // 
            // inputDevice_label
            // 
            this.inputDevice_label.AutoSize = true;
            this.inputDevice_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.inputDevice_label.Location = new System.Drawing.Point(13, 71);
            this.inputDevice_label.Name = "inputDevice_label";
            this.inputDevice_label.Size = new System.Drawing.Size(70, 20);
            this.inputDevice_label.TabIndex = 3;
            this.inputDevice_label.Text = "Mikrofon";
            this.inputDevice_label.Click += new System.EventHandler(this.inputDevice_label_Click);
            // 
            // inputDevices_Combo
            // 
            this.inputDevices_Combo.FormattingEnabled = true;
            this.inputDevices_Combo.Location = new System.Drawing.Point(17, 37);
            this.inputDevices_Combo.Name = "inputDevices_Combo";
            this.inputDevices_Combo.Size = new System.Drawing.Size(176, 21);
            this.inputDevices_Combo.TabIndex = 4;
            // 
            // outputDevices_Combo
            // 
            this.outputDevices_Combo.FormattingEnabled = true;
            this.outputDevices_Combo.Location = new System.Drawing.Point(17, 94);
            this.outputDevices_Combo.Name = "outputDevices_Combo";
            this.outputDevices_Combo.Size = new System.Drawing.Size(176, 21);
            this.outputDevices_Combo.TabIndex = 5;
            // 
            // serverAddress_label
            // 
            this.serverAddress_label.AutoSize = true;
            this.serverAddress_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.serverAddress_label.Location = new System.Drawing.Point(13, 128);
            this.serverAddress_label.Name = "serverAddress_label";
            this.serverAddress_label.Size = new System.Drawing.Size(133, 20);
            this.serverAddress_label.TabIndex = 6;
            this.serverAddress_label.Text = "Adres IP Serwera";
            // 
            // serverAddress_Input
            // 
            this.serverAddress_Input.Location = new System.Drawing.Point(17, 152);
            this.serverAddress_Input.Name = "serverAddress_Input";
            this.serverAddress_Input.Size = new System.Drawing.Size(176, 20);
            this.serverAddress_Input.TabIndex = 7;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.saveSettings_button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.discardSettings_button;
            this.ClientSize = new System.Drawing.Size(211, 273);
            this.Controls.Add(this.serverAddress_Input);
            this.Controls.Add(this.serverAddress_label);
            this.Controls.Add(this.outputDevices_Combo);
            this.Controls.Add(this.inputDevices_Combo);
            this.Controls.Add(this.inputDevice_label);
            this.Controls.Add(this.speakerDevice_label);
            this.Controls.Add(this.discardSettings_button);
            this.Controls.Add(this.saveSettings_button);
            this.Name = "SettingsForm";
            this.Text = "Ustawienia";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveSettings_button;
        private System.Windows.Forms.Button discardSettings_button;
        private System.Windows.Forms.Label speakerDevice_label;
        private System.Windows.Forms.Label inputDevice_label;
        private System.Windows.Forms.ComboBox inputDevices_Combo;
        private System.Windows.Forms.ComboBox outputDevices_Combo;
        private System.Windows.Forms.Label serverAddress_label;
        private System.Windows.Forms.TextBox serverAddress_Input;
    }
}