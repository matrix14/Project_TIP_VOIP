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
        public static Form activeForm;
        
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
            activeForm = new LoginForm();
            Application.Run(activeForm);
            ServerConnectorAsync.StopConnection();
        }

        delegate Form GetActiveFormCallback();
        public static Form GetActiveForm()
        {
            if (activeForm.InvokeRequired)
            {
                Form a = null;
                Action showMethod = delegate () { a = activeForm; };
                return a;
            }
            else
            {
                return activeForm;
            }
        }
    }
}
