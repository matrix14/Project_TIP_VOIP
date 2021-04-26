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
    public delegate void BooleanCallback(Boolean exist);
    public partial class LoginForm : Form
    {
        //TODO: password check, username length check
        private Boolean registerMode = false;

        private System.Timers.Timer checkUsernameTimer = new System.Timers.Timer();
        private Boolean checkUsernameTimerStopped = true;

        private Boolean registerSwitch = false; //TODO temporary

        private Boolean connectionAlive = false;
        public LoginForm()
        {
            InitializeComponent();

            checkUsernameTimer.Interval = 150;
            checkUsernameTimer.Elapsed += usernameCheckOnTimerElapsed;
            checkUsernameTimer.AutoReset = false;

            BooleanCallback callback = usernameCheckUpdateInfo;
            LoginService.CheckUsernameCallback = callback;

            connectionAlive = ServerConnectorAsync.getConnectionState();
            updateConnectionState();
            Task.Run(stateChecker);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void usernameCheckOnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (checkUsernameTimerStopped == false)
            {
                checkUsernameTimer.Stop();
                checkUsernameTimerStopped = true;
                LoginService.checkIsUserExist(this.login_textbox.Text);
                
            }
        }

        private void login_textbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (registerMode == false) return;
            if (this.login_textbox.Text.Length >= 3)
            {
                checkUsernameTimer.Stop();
                checkUsernameTimer.Start();
                checkUsernameTimerStopped = false;
                //LoginService.checkIsUserExist(this.login_textbox.Text);
                /*if (this.registerMode)
                {
                    if (registerSwitch)
                    { //TODO temporary
                        this.usernameFree_label.Text = "Login niedostępny!";
                        this.usernameFree_label.ForeColor = Color.Red;
                        registerSwitch = false;
                    }
                    else
                    {
                        this.usernameFree_label.Text = "Login dostępny";
                        this.usernameFree_label.ForeColor = Color.Green;
                        registerSwitch = true;
                    }
                }*/
            }
            //TODO check if username is free
        }

        public void usernameCheckUpdateInfo(Boolean exist)
        {
            if (usernameFree_label.InvokeRequired)
            {
                usernameFree_label.Invoke(new MethodInvoker(() => { usernameCheckUpdateInfo(exist); }));
                return;
            }
            else
            {
                if (exist)
                {
                    this.usernameFree_label.Text = "Login niedostępny!";
                    this.usernameFree_label.ForeColor = Color.Red;
                }
                else
                {
                    this.usernameFree_label.Text = "Login dostępny";
                    this.usernameFree_label.ForeColor = Color.Green;
                }
                return;
            }
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
                this.usernameFree_label.Visible = false;
                checkUsernameTimer.Stop();
                checkUsernameTimerStopped = true;
            } else {
                registerMode = true;
                this.actualMode_Label.Text = "Rejestracja";
                this.confirmAction_button.Text = "Zarejestruj się";
                this.changeMode_label.Text = "Masz już konto?";
                this.changeMode_button.Text = "Zaloguj się";
                this.usernameFree_label.Visible = true;
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
