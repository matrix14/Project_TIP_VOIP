using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientWindows
{
    static class Program
    {
        public static String username = "";
        public static Boolean isLoggedIn = false;
        public static Boolean isInCall = false;


        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if(args.Length==1)
            {
                Shared.IP.serverIp = args[0];
            }
           /* else
            {
                Shared.IP.serverIp = "127.0.0.1";
            }*/

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ServerConnectorAsync.StartConnection();

            Task.Run(() => ServerConnectorAsync.ReceiveWhile());
            Application.Run(new LoginForm());
            ServerConnectorAsync.StopConnection();
        }
    }
}
