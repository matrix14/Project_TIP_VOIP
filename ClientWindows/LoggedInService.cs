using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;

namespace ClientWindows
{
    static class LoggedInService
    {
        private static Boolean logoutNotFinished = false;
        public static void logout()
        {
            ServerProcessing.processSendMessage(MessageProccesing.CreateMessage(Options.LOGOUT));
            logoutNotFinished = true;
            do
            {

            } while (logoutNotFinished);

        }

        public static void logoutReply(String message)
        {
            String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            ErrorCodes error = (ErrorCodes)int.Parse(replySplit[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1]);
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            string title = "Wylogowanie";
            String msg = "";
            switch (error)
            {
                case ErrorCodes.NO_ERROR:
                    msg = "Pomyślnie wylogowano! NOT_YET_IMPLEMENTED";
                    MessageBox.Show(msg, title, buttons);
                    Program.isLoggedIn = false;
                    break;
            }
            logoutNotFinished = false;
        }
    }
}
