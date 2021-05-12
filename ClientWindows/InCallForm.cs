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
using Shared;

namespace ClientWindows
{
    public delegate void ByteCallback(byte[] b);
    public partial class InCallForm : Form
    {

        private Id callId = null;
        private Call call = null;

        private System.Timers.Timer packetsCounterTimer = new System.Timers.Timer();
        private int packetsCounter = 0;
        private int packetsCounter2 = 0;

        CancellationTokenSource tokenSource = null;
        CancellationToken token;

        private Boolean micStatus = true;
        private Boolean micStatusBeforeSpkMute = false;

        private SoundProcessing sp = null;
        public InCallForm()
        {
            InitializeComponent();
        }

        private void updateUsersInCall()
        {
            if(callUsersList_label.InvokeRequired)
            {
                this.callUsersList_label.Invoke(new MethodInvoker(() => { updateUsersInCall(); }));
                return;
            }
            String usersListStr = "";
            foreach (String u in call.usernames)
            {
                if (!usersListStr.Equals(""))
                {
                    usersListStr += ", ";
                }
                usersListStr += u;
            }
            this.callUsersList_label.Text = usersListStr;
            if(sp!=null)
                sp.updateUsersCount(this.call.usernames);
        }

        public InCallForm(Call c)
        {
            
            this.call = c;
            this.callId = new Id(c.callId);
            InitializeComponent();
            this.Text = "(" + Program.username + ") Aktywne połączenie";
            StringCallback callback2 = addUser;
            StringCallback callback3 = removeUser;
            LoggedInService.AddUserToCall = callback2;
            LoggedInService.RemoveUserFromCall = callback3;

            updateUsersInCall();
            ByteCallback callback = incomingTraffic;
            CallProcessing.ReceiveMsgCallback = callback;
            CallProcessing.Start();
            Program.isInCall = true;

            packetsCounterTimer.Interval = 1000;
            packetsCounterTimer.Elapsed += packetsCounterTimer_OnTimerElapsed;
            packetsCounterTimer.AutoReset = true;
            packetsCounterTimer.Start();

            ByteCallback sendCb = sendSound;
            sp = new SoundProcessing(sendCb);

            tokenSource = new System.Threading.CancellationTokenSource();
            token = tokenSource.Token;

            Task.Run(() => sp.startUp(this.call.usernames, token), token);
        }

        public void removeUser(string username)
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => { removeUser(username); }));
                return;
            }
            this.call.removeUser(username);
            updateUsersInCall();
            if (this.call.usernames.Count == 0)
            {
                this.Close();
            }
        }

        private void packetsCounterTimer_OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (this.incomingPackets_label.InvokeRequired)
                {
                    this.incomingPackets_label.Invoke(new MethodInvoker(() => { packetsCounterTimer_OnTimerElapsed(sender, e); }));
                    return;
                }
                if (packetsCounterTimer.Enabled == false)
                    return;
                String message = packetsCounter.ToString();
                for (int i = 0; i < (packetsCounter2 + 1); i++)
                {
                    message += ".";
                }
                packetsCounter2++;
                if (packetsCounter2 == 4)
                    packetsCounter2 = 0;
                packetsCounter = 0;
                incomingPackets_label.Text = message;
            } catch (ObjectDisposedException)
            {
                return;
            }
        }

        public void addUser(string username)
        {
            this.call.addUser(username);
            updateUsersInCall();
        }

        private void InCallForm_Load(object sender, EventArgs e)
        {

        }

        private void leaveCall_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InCallForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.callId == null) return;
            packetsCounterTimer.Stop();
            tokenSource.Cancel();
            sp.stop();
            CallProcessing.SendMessages(BitConverter.GetBytes(callId.id));
            CallProcessing.SendMessages(BitConverter.GetBytes(callId.id));
            CallProcessing.SendMessages(BitConverter.GetBytes(callId.id));
            CallProcessing.Stop();
            LoggedInService.declineCall(this.callId);
            Program.isInCall = false;
        }

        private void incomingTraffic_label_Click(object sender, EventArgs e)
        {
            
        }

        public void incomingTraffic(byte[] b)
        {
            if (incomingMsg_label.InvokeRequired)
            {
                incomingMsg_label.Invoke(new MethodInvoker(() => { incomingTraffic(b); })); //TODO: ObjectDisposedException - not exist "Label"
            }
            else
            {
                packetsCounter++;
                incomingMsg_label.Text = Encoding.ASCII.GetString(b);
                if (sp != null)
                    sp.incomingEncodedSound(b);
            }
        }

        /*public void sentBytes()
        {
            do
            {
                char b = 'Z';
                switch (i)
                {
                    case 0:
                        b = 'A';
                        break;
                    case 1:
                        b = 'B';
                        break;
                    case 2:
                        b = 'C';
                        break;
                    case 3:
                        b = 'D';
                        break;
                }

                CallProcessing.SendMessages(Encoding.ASCII.GetBytes(Program.username + ": " + b));
                i++;
                if (i == 4)
                {
                    i = 0;
                }
                System.Threading.Thread.Sleep(250);
            } while (callStopped == false);
        }*/

        public void sendSound(byte[] sound)
        {
            CallProcessing.SendMessages(sound);
        }

        private void callUsersList_label_Click(object sender, EventArgs e)
        {

        }

        private void muteMicrophone_button_Click(object sender, EventArgs e)
        {
            if (sp == null)
                return;
            micStatus = sp.switchMicrophone();
            if (micStatus)
            {
                this.microState_label.Text = "ON";
                this.microState_label.ForeColor = Color.Red;
            }
            else
            {
                this.microState_label.Text = "OFF";
                this.microState_label.ForeColor = Color.Green;
            }
        }

        private void muteSound_button_Click(object sender, EventArgs e)
        {
            if (sp == null)
                return;
            Boolean speStat = sp.switchSpeaker();
            if (speStat)
            {
                if (micStatusBeforeSpkMute&&!micStatus)
                {
                    this.muteMicrophone_button.PerformClick();
                }

                this.soundState_label.Text = "ON";
                this.soundState_label.ForeColor = Color.Red;
            }
            else
            {
                if (micStatus)
                {
                    this.muteMicrophone_button.PerformClick();
                    micStatusBeforeSpkMute = true;
                }
                else
                {
                    micStatusBeforeSpkMute = false;
                }
                this.soundState_label.Text = "OFF";
                this.soundState_label.ForeColor = Color.Green;
            }
        }

        private void soundSettings_button_Click(object sender, EventArgs e)
        {
            SettingsForm sf = new SettingsForm();
            sf.ShowDialog();
        }
    }
}
