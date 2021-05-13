using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;
using NAudio.CoreAudioApi;

namespace ClientWindows
{
    class SettingsService
    {
        /*
              <add key="serverAddress" value="127.0.0.1" />
              <add key="inputDevice" value="0" />
              <add key="outputDevice" value="0" />
              <add key="ioDevicesHash" value="0" />
        */
        private AppSettingsSection appSettings;
        private Configuration config;


        public SettingsService()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            appSettings = config.AppSettings;
            Shared.IP.serverIp = this.getServerIP();
            if (!this.checkIOHash())
            {
                MessageBox.Show("Zmieniono urządzenia Audio, ustawiono domyślne urządzenia!");
                saveIODevices(0, 0);
                saveActualIOHash();
            }
                
        }

        private String calculateIODev()
        {
            StringBuilder sb = new StringBuilder();
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            int j = 0;
            foreach (MMDevice device in enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active))
            {
                sb.Append(String.Format("{0}: {1}", j, device.FriendlyName));
                j++;
            }

            MMDeviceEnumerator enumerator2 = new MMDeviceEnumerator();
            int j2 = 0;
            foreach (MMDevice device in enumerator2.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
            {
                sb.Append(String.Format("{0}: {1}", j2, device.FriendlyName));
                j2++;
            }
            return sb.ToString();
        }

        public String getServerIP()
        {
            return appSettings.Settings["serverAddress"].Value;
        }

        public Boolean setServerIP(String ip)
        {
            this.appSettings.Settings["serverAddress"].Value = ip;
            saveConfigurationFile();
            return true;
        }

        private String calculateIOHash(String io)
        {
            StringBuilder sb = new StringBuilder();
            byte[] hashBytes = new byte[32];

            using (HashAlgorithm algorithm = SHA256.Create())
                hashBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(io));

            foreach (byte b in hashBytes)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        private void setIOHash(String io)
        {
            this.appSettings.Settings["ioDevicesHash"].Value = this.calculateIOHash(io);
        }

        public Boolean checkIOHash()
        {
            if (this.appSettings.Settings["ioDevicesHash"].Value.Equals(this.calculateIOHash(calculateIODev())))
                return true;
            else
                return false;
        }

        public void saveActualIOHash()
        {
            this.appSettings.Settings["ioDevicesHash"].Value = this.calculateIOHash(calculateIODev());
            saveConfigurationFile();
        }

        private void saveConfigurationFile()
        {
            this.config.Save();
        }

        public void saveIODevices(int inputDev, int outputDev)
        {
            this.appSettings.Settings["inputDevice"].Value = inputDev.ToString();
            this.appSettings.Settings["outputDevice"].Value = outputDev.ToString();
            saveActualIOHash();
        }

        public int getIOInputDevice()
        {
            return int.Parse(this.appSettings.Settings["inputDevice"].Value);
        }

        public int getIOOutputDevice()
        {
            return int.Parse(this.appSettings.Settings["outputDevice"].Value);
        }
    }
}
