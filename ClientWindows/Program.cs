using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;

namespace ClientWindows
{
    static class Program
    {
        public static String username = "";
        public static Boolean isLoggedIn = false;
        public static Boolean isInCall = false;
        public static SettingsService setServ;


        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            setServ = new SettingsService();

            if (args.Length==1) //Temporary change of server IP Address
            {
                MessageBox.Show("Tymczasowo nadpisano IP serwera za pomocą argumentu! Adres IP nie zostanie zapisany w konfiguracji.");
                Shared.IP.serverIp = args[0];
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ServerConnectorAsync.StartConnection();

            Task.Run(() => ServerConnectorAsync.ReceiveWhile());
            Application.Run(new LoginForm());
            ServerConnectorAsync.StopConnection();
        }
    }
}
