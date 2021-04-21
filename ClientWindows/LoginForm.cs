using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientWindows
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void register_button_Click(object sender, EventArgs e)
        {
            LoginService.register(this.login_textbox.Text, this.password_textbox.Text);
        }

        private void login_button_Click(object sender, EventArgs e)
        {
            Program.username = this.login_textbox.Text;
            LoginService.login(this.login_textbox.Text, this.password_textbox.Text);
            if (Program.isLoggedIn == true)
            {
                LoggedInForm lif = new LoggedInForm();
                lif.Location = this.Location;
                lif.StartPosition = FormStartPosition.Manual;
                lif.FormClosing += delegate { this.Show(); };
                lif.Show();
                this.Hide();
            }
        }

        public void onLoginFinished()
        {
            if (Program.isLoggedIn == true)
            {
                LoggedInForm lif = new LoggedInForm();
                lif.Location = this.Location;
                lif.StartPosition = FormStartPosition.Manual;
                lif.FormClosing += delegate { this.Show(); };
                lif.Show();
                this.Hide();
            }
        }
    }
}
