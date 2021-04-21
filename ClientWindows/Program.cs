using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientWindows
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ServerConnectorAsync.StartConnection();

            Task.Run(() => ServerConnectorAsync.ReceiveWhile());
            Application.Run(new LoginForm());
            ServerConnectorAsync.StopConnection();
        }
    }
}
