using NAudio.CoreAudioApi;
using NAudio.Wave;
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
    public partial class SettingsForm : Form
    {
        private int actualIOOutputDev;
        private int actualIOInputDev;
        private String actualIP;

        public SettingsForm()
        {
            InitializeComponent();

            /*if(!Program.setServ.checkIOHash())
            {
                this.inputDevices_Combo.BackColor = Color.Red;
                this.outputDevices_Combo.BackColor = Color.Red;
            }*/

            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            int j = 0;
            foreach (MMDevice device in enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active))
            {
                //Console.WriteLine("{0}, {1}", device.FriendlyName, device.State);
                this.inputDevices_Combo.Items.Add(String.Format("{0}: {1}", j, device.FriendlyName));
                j++;
            }

            /*int availableInputDevicesCount = WaveIn.DeviceCount;
            for (int i = 0; i < availableInputDevicesCount; i++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(i);
                this.inputDevices_Combo.Items.Add(String.Format("{0}: {1}", i, deviceInfo.ProductName));
            }*/

            MMDeviceEnumerator enumerator2 = new MMDeviceEnumerator();
            int j2 = 0;
            foreach (MMDevice device in enumerator2.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
            {
                //Console.WriteLine("{0}, {1}", device.FriendlyName, device.State);
                this.outputDevices_Combo.Items.Add(String.Format("{0}: {1}", j2, device.FriendlyName));
                j2++;
            }

            /*int availableOutputDevicesCount = WaveOut.DeviceCount;
            for (int i = 0; i < availableOutputDevicesCount; i++)
            {
                WaveOutCapabilities deviceInfo = WaveOut.GetCapabilities(i);
                this.outputDevices_Combo.Items.Add(String.Format("{0}: {1}", i, deviceInfo.ProductName));
            }*/

            this.inputDevices_Combo.SelectedIndex = 0;
            this.outputDevices_Combo.SelectedIndex = 0;

            //this.serverAddress_Input.Text = Shared.IP.serverIp;
            this.serverAddress_Input.Text = Program.setServ.getServerIP();
            this.actualIP = Program.setServ.getServerIP();

            this.inputDevices_Combo.SelectedIndex = Program.setServ.getIOInputDevice();
            this.actualIOInputDev = Program.setServ.getIOInputDevice();

            this.outputDevices_Combo.SelectedIndex = Program.setServ.getIOOutputDevice();
            this.actualIOOutputDev = Program.setServ.getIOOutputDevice();

            this.serverAddress_Input.Enabled = !Program.isLoggedIn;
            if (this.serverAddress_Input.Enabled)
                this.serverAddress_label.Text = "Adres IP Serwera";
            else
                this.serverAddress_label.Text = "Wyloguj się aby zmienić IP";
        }

        private void inputDevice_label_Click(object sender, EventArgs e)
        {

        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {

        }

        private void saveSettings_button_Click(object sender, EventArgs e)
        {
            if (!actualIP.Equals(this.serverAddress_Input.Text))
            {
                //TODO: verify if IP is ok
                Program.setServ.setServerIP(this.serverAddress_Input.Text);
                MessageBox.Show("Zmieniono adres IP serwera, zrestartuj aplikacje!");
                System.Windows.Forms.Application.Exit();
            }
            if (!actualIOInputDev.Equals(this.inputDevices_Combo.SelectedIndex)|| !actualIOOutputDev.Equals(this.outputDevices_Combo.SelectedIndex))
            {
                Program.setServ.saveIODevices(this.inputDevices_Combo.SelectedIndex, this.outputDevices_Combo.SelectedIndex);
            }
            if(Program.spGlobal!=null)
                Program.spGlobal.updateIODevices();
            this.Close();
        }

        private void discardSettings_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
