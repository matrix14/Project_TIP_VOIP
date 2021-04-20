
namespace ClientWindows
{
    partial class LoginForm
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.login_button = new System.Windows.Forms.Button();
            this.login_label = new System.Windows.Forms.Label();
            this.login_textbox = new System.Windows.Forms.TextBox();
            this.password_textbox = new System.Windows.Forms.TextBox();
            this.pass_label = new System.Windows.Forms.Label();
            this.register_button = new System.Windows.Forms.Button();
            this.register_info_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // login_button
            // 
            this.login_button.Location = new System.Drawing.Point(12, 96);
            this.login_button.Name = "login_button";
            this.login_button.Size = new System.Drawing.Size(200, 25);
            this.login_button.TabIndex = 0;
            this.login_button.Text = "Zaloguj";
            this.login_button.UseVisualStyleBackColor = true;
            this.login_button.Click += new System.EventHandler(this.login_button_Click);
            // 
            // login_label
            // 
            this.login_label.AutoSize = true;
            this.login_label.Location = new System.Drawing.Point(12, 15);
            this.login_label.Name = "login_label";
            this.login_label.Size = new System.Drawing.Size(33, 13);
            this.login_label.TabIndex = 1;
            this.login_label.Text = "Login";
            // 
            // login_textbox
            // 
            this.login_textbox.Location = new System.Drawing.Point(12, 31);
            this.login_textbox.Name = "login_textbox";
            this.login_textbox.Size = new System.Drawing.Size(200, 20);
            this.login_textbox.TabIndex = 2;
            // 
            // password_textbox
            // 
            this.password_textbox.Location = new System.Drawing.Point(12, 70);
            this.password_textbox.Name = "password_textbox";
            this.password_textbox.Size = new System.Drawing.Size(200, 20);
            this.password_textbox.TabIndex = 3;
            // 
            // pass_label
            // 
            this.pass_label.AutoSize = true;
            this.pass_label.Location = new System.Drawing.Point(12, 54);
            this.pass_label.Name = "pass_label";
            this.pass_label.Size = new System.Drawing.Size(36, 13);
            this.pass_label.TabIndex = 4;
            this.pass_label.Text = "Hasło";
            // 
            // register_button
            // 
            this.register_button.Location = new System.Drawing.Point(12, 161);
            this.register_button.Name = "register_button";
            this.register_button.Size = new System.Drawing.Size(200, 25);
            this.register_button.TabIndex = 5;
            this.register_button.Text = "Zarejestruj się";
            this.register_button.UseVisualStyleBackColor = true;
            this.register_button.Click += new System.EventHandler(this.register_button_Click);
            // 
            // register_info_label
            // 
            this.register_info_label.AutoSize = true;
            this.register_info_label.Location = new System.Drawing.Point(12, 132);
            this.register_info_label.Name = "register_info_label";
            this.register_info_label.Size = new System.Drawing.Size(163, 26);
            this.register_info_label.TabIndex = 6;
            this.register_info_label.Text = "Nie masz konta?\rWprowadz dane powyżej i kliknij:";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(227, 200);
            this.Controls.Add(this.register_info_label);
            this.Controls.Add(this.register_button);
            this.Controls.Add(this.pass_label);
            this.Controls.Add(this.password_textbox);
            this.Controls.Add(this.login_textbox);
            this.Controls.Add(this.login_label);
            this.Controls.Add(this.login_button);
            this.Name = "LoginForm";
            this.Text = "TIP_VOIP Client";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button login_button;
        private System.Windows.Forms.Label login_label;
        private System.Windows.Forms.TextBox login_textbox;
        private System.Windows.Forms.TextBox password_textbox;
        private System.Windows.Forms.Label pass_label;
        private System.Windows.Forms.Button register_button;
        private System.Windows.Forms.Label register_info_label;
    }
}

