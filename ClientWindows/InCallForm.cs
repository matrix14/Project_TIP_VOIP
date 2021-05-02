using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        private int i = 0;


        public InCallForm()
        {
            InitializeComponent();
        }

        public InCallForm(Call c)
        {
            this.call = c;
            this.callId = new Id(c.callId);
            InitializeComponent();
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
            Task.Run(progBarIncrease);

            ByteCallback callback = incomingTraffic;
            CallProcessing.ReceiveMsgCallback = callback;
            CallProcessing.Start();
            Task.Run(sentBytes);
        }

        public InCallForm(Id id, Username user)
        {
            this.callId = id;
            InitializeComponent();
            this.callUsersList_label.Text = user.username;
            Task.Run(progBarIncrease);

            ByteCallback callback = incomingTraffic;
            CallProcessing.ReceiveMsgCallback = callback;
            CallProcessing.Start();
            Task.Run(sentBytes);
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
            LoggedInService.declineCall(this.callId);
            CallProcessing.Stop();
        }

        private void incomingTraffic_label_Click(object sender, EventArgs e)
        {
            
        }

        private void progBarIncrease()
        {
            incomingTraffic_bar.Visible = true;
            incomingTraffic_bar.Minimum = 0;
            incomingTraffic_bar.Maximum = 1;
            incomingTraffic_bar.Value = 1;
            incomingTraffic_bar.Step = 1;
            while(true)
            {
                if(incomingTraffic_bar.Value==1)
                    incomingTraffic_bar.Value = 0;
                else
                    incomingTraffic_bar.PerformStep();
                System.Threading.Thread.Sleep(1000);
            }
            
        }

        public void incomingTraffic(byte[] b)
        {
            if (incomingMsg_label.InvokeRequired)
            {
                incomingMsg_label.Invoke(new MethodInvoker(() => { incomingTraffic(b); }));
            }
            else
            {
                incomingMsg_label.Text = b[0].ToString();
            }
        }

        public void sentBytes()
        {
            byte b = (byte)'Z';
            switch (i)
            {
                case 0:
                    b = (byte)'A';
                    break;
                case 1:
                    b = (byte)'B';
                    break;
                case 2:
                    b = (byte)'C';
                    break;
                case 3:
                    b = (byte)'D';
                    break;
            }

            CallProcessing.SendMessages(new byte[] { b });
            i++;
            if(i==4)
            {
                i = 0;
            }
            System.Threading.Thread.Sleep(100);
        }
    }
}
