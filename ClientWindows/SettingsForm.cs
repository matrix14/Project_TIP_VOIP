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
        public SettingsForm()
        {
            InitializeComponent();

            int availableInputDevicesCount = WaveIn.DeviceCount;
            for (int i = 0; i < availableInputDevicesCount; i++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(i);
                this.inputDevices_Combo.Items.Add(String.Format("{0}: {1}", i, deviceInfo.ProductName));
            }

            int availableOutputDevicesCount = WaveOut.DeviceCount;
            for (int i = 0; i < availableOutputDevicesCount; i++)
            {
                WaveOutCapabilities deviceInfo = WaveOut.GetCapabilities(i);
                this.outputDevices_Combo.Items.Add(String.Format("{0}: {1}", i, deviceInfo.ProductName));
            }

            this.inputDevices_Combo.SelectedIndex = 0;
            this.outputDevices_Combo.SelectedIndex = 0;

            this.serverAddress_Input.Text = Shared.IP.serverIp;
            this.serverAddress_Input.Enabled = !Program.isLoggedIn;
        }

        private void inputDevice_label_Click(object sender, EventArgs e)
        {

        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
