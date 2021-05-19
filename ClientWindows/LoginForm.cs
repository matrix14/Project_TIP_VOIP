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
        private Boolean registerMode = false;

        private System.Timers.Timer checkUsernameTimer = new System.Timers.Timer();
        private System.Timers.Timer checkConnectionTimer = new System.Timers.Timer();
        private Boolean checkUsernameTimerStopped = true;
        private Boolean firstConnectionDone = false;

        private Boolean connectionAlive = false;
        public LoginForm()
        {
            InitializeComponent();

            this.ActiveControl = this.login_textbox;

            checkUsernameTimer.Interval = 150;
            checkUsernameTimer.Elapsed += usernameCheckOnTimerElapsed;
            checkUsernameTimer.AutoReset = false;

            checkConnectionTimer.Interval = 1000;
            checkConnectionTimer.Elapsed += checkConnectionOnTimerElapsed;
            checkConnectionTimer.AutoReset = true;

            BooleanCallback callback = usernameCheckUpdateInfo;
            LoginService.CheckUsernameCallback = callback;

            BooleanCallback callback2 = onLogin;
            LoginService.OnLoginCallback = callback2;

            BooleanCallback callback3 = onRegistered;
            LoginService.OnRegisterCallback = callback3;

            checkConnectionTimer.Start();
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

        private void checkConnectionOnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            connectionAlive = ServerConnectorAsync.getConnectionState();
            updateConnectionState();
        }

        private void login_textbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (registerMode == false) return;
            if (this.login_textbox.Text.Length >= 3)
            {
                checkUsernameTimer.Stop();
                checkUsernameTimer.Start();
                checkUsernameTimerStopped = false;
            } else
            {
                this.usernameFree_label.Visible = false;
            }
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
                    this.usernameFree_label.Visible = true;
                    this.usernameFree_label.ForeColor = Color.Red;
                    this.confirmAction_button.Enabled = false;
                }
                else
                {
                    this.usernameFree_label.Text = "Login dostępny";
                    this.usernameFree_label.Visible = true;
                    this.usernameFree_label.ForeColor = Color.Green;
                    this.confirmAction_button.Enabled = true;
                }
                return;
            }
        }

        public void onRegistered(Boolean succesfull)
        {
            if (usernameFree_label.InvokeRequired)
            {
                usernameFree_label.Invoke(new MethodInvoker(() => { usernameCheckUpdateInfo(succesfull); }));
                return;
            }
            else
            {
                if (succesfull)
                {
                    this.usernameFree_label.Text = "Login niedostępny!";
                    this.usernameFree_label.ForeColor = Color.Red;
                    this.usernameFree_label.Visible = true;
                    this.confirmAction_button.Enabled = false;
                } else
                {
                    this.login_textbox.Text = "";
                    this.password_textbox.Text = "";
                    this.usernameFree_label.Visible = false;
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
                this.confirmAction_button.Enabled = true;
            } else {
                registerMode = true;
                this.actualMode_Label.Text = "Rejestracja";
                this.confirmAction_button.Text = "Zarejestruj się";
                this.changeMode_label.Text = "Masz już konto?";
                this.changeMode_button.Text = "Zaloguj się";
                this.usernameFree_label.Visible = true;
                this.usernameFree_label.Text = "";
                this.confirmAction_button.Enabled = false;
                if(this.login_textbox.Text.Length>=3)
                {
                    LoginService.checkIsUserExist(this.login_textbox.Text);
                }
            }
        }

        private void confirmAction_button_Click(object sender, EventArgs e)
        {
            if (this.registerMode)
            {
                if (this.login_textbox.Text.Length < 3)
                {
                    MessageBox.Show("Login nie może być krótszy niż 3 znaki!");
                    return;
                }
                if (this.password_textbox.Text.Length < 3)
                {
                    MessageBox.Show("Hasło nie może być krótsze niż 3 znaki!");
                    return;
                }
                LoginService.register(this.login_textbox.Text, this.password_textbox.Text);
                this.confirmAction_button.Enabled = false;
            }
            else
            {
                Program.username = this.login_textbox.Text;
                LoginService.login(this.login_textbox.Text, this.password_textbox.Text);
                
                /*if (Program.isLoggedIn == true)
                {
                    LoggedInForm lif = new LoggedInForm();
                    lif.Location = this.Location;
                    lif.StartPosition = FormStartPosition.Manual;
                    lif.FormClosing += delegate { this.Show(); };
                    lif.Show();
                    this.Hide();
                }*/
            }
        }

        public void onLogin(Boolean loggedIn)
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => { onLogin(loggedIn); }));
                return;
            }
            if (loggedIn == true)
            {
                LoggedInForm lif = new LoggedInForm();
                lif.Location = this.Location;
                lif.StartPosition = FormStartPosition.Manual;
                lif.FormClosing += delegate { showForm(); };
                lif.Show();
                this.Hide();
            }
        }

        public void showForm()
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => { showForm(); }));
                return;
            } else
            {
                this.Show();
            }
        }

        private void updateConnectionState()
        {
            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.Invoke(new MethodInvoker(() => { updateConnectionState(); }));
                return;
            }
            else
            {
                if (connectionAlive)
                {
                    if (firstConnectionDone==false)
                    {
                        this.changeMode_button.Enabled = true;
                        this.confirmAction_button.Enabled = true;
                        firstConnectionDone = true;
                    }
                    this.serverConnection_Label.Text = "Połączono";
                    this.serverConnection_Label.ForeColor = Color.Green;
                }
                else
                {
                    this.serverConnection_Label.Text = "Brak połączenia!";
                    this.serverConnection_Label.ForeColor = Color.Red;
                }
            }
        }

        private void serverConnection_Label_Click(object sender, EventArgs e)
        {

        }

        private void LoginForm_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible)
            {
                checkConnectionTimer.Start();
            } else
            {
                checkConnectionTimer.Stop();
            }
        }

        private void settingsButton_Button_Click(object sender, EventArgs e)
        {
            SettingsForm sf = new SettingsForm();
            sf.ShowDialog();
        }

        private void actualMode_Label_Click(object sender, EventArgs e)
        {

        }
    }
}
