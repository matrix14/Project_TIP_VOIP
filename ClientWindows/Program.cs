using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;
using Shared;

namespace ClientWindows
{
    static class Program
    {
        public static String username = "";
        public static Boolean isLoggedIn = false;
        public static Boolean isInCall = false;
        public static SettingsService setServ;
        public static SoundProcessing spGlobal = null;
        public static Call actualCall = null;


        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            OperatingSystem os = Environment.OSVersion;
            if (os.Platform == PlatformID.Win32NT)
            {
                if(!(os.Version.Major>=6))
                {
                    MessageBox.Show("Program działa tylko na systemach Windows Vista/7 i nowszych!");
                    return;
                }
            } else
            {
                MessageBox.Show("Program działa tylko na systemach Windows Vista/7 i nowszych!");
                return;
            }

            setServ = new SettingsService();

            if (args.Length==1) //Temporary change of server IP Address
            {
                //MessageBox.Show("Tymczasowo nadpisano IP serwera za pomocą argumentu! Adres IP nie zostanie zapisany w konfiguracji.");
                Shared.IP.serverIp = args[0];
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ServerConnectorAsync.StartConnection();

            Task.Run(() => ServerConnectorAsync.ReceiveWhile());
            Application.Run(new LoginForm());
            ServerConnectorAsync.closingApp = true;
            ServerConnectorAsync.StopConnection();
        }
    }
}
