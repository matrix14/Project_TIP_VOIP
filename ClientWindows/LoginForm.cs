using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientWindows
{
    public partial class LoginForm : Form
    {
        private Boolean registerMode = false;

        private Boolean registerSwitch = false; //TODO temporary

        private Boolean connectionAlive = false;
        public LoginForm()
        {
            InitializeComponent();
            connectionAlive = ServerConnectorAsync.getConnectionState();
            updateConnectionState();
            Task.Run(stateChecker);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void login_textbox_KeyUp(object sender, KeyEventArgs e)
        {
            if(this.registerMode)
            {
                if (registerSwitch) { //TODO temporary
                    this.usernameFree_label.Text = "Login niedostępny!";
                    this.usernameFree_label.ForeColor = Color.Red;
                    registerSwitch = false;
                } else
                {
                    this.usernameFree_label.Text = "Login dostępny";
                    this.usernameFree_label.ForeColor = Color.Green;
                    registerSwitch = true;
                }
            }
            //TODO check if username is free
        }

        private void changeMode_button_Click(object sender, EventArgs e)
        {
            if(this.registerMode)
            {
                registerMode = false;
                this.actualMode_Label.Text = "Logowanie";
                this.confirmAction_button.Text = "Zaloguj się";
                this.changeMode_label.Text = "Nie masz konta?";
                this.changeMode_button.Text = "Zarejestruj się";
                this.usernameFree_label.Text = "";
            } else {
                registerMode = true;
                this.actualMode_Label.Text = "Rejestracja";
                this.confirmAction_button.Text = "Zarejestruj się";
                this.changeMode_label.Text = "Masz już konto?";
                this.changeMode_button.Text = "Zaloguj się";
                this.usernameFree_label.Text = "";
            }
        }

        private void confirmAction_button_Click(object sender, EventArgs e)
        {
            if (this.registerMode)
            {
                LoginService.register(this.login_textbox.Text, this.password_textbox.Text);
            }
            else
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
        }

        private void updateConnectionState()
        {
            if(connectionAlive)
            {
                this.serverConnection_Label.Text = "Połączono";
                this.serverConnection_Label.ForeColor = Color.Green;
            } else
            {
                this.serverConnection_Label.Text = "Brak połączenia!";
                this.serverConnection_Label.ForeColor = Color.Red;
            }
        }

        private void serverConnection_Label_Click(object sender, EventArgs e)
        {

        }

        private void stateChecker()
        {
            while(true)
            {
                Thread.Sleep(1000);
                connectionAlive = ServerConnectorAsync.getConnectionState();
                updateConnectionState();
            }
        }
    }
}
