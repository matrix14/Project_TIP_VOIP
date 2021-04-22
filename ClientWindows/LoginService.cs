using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;

namespace ClientWindows
{
    static class LoginService
    {
        private static Boolean loginNotFinished = false;

        public static void login(String username, String pass)
        {
            Login login = new Login(username, pass);
            ServerProcessing.processSendMessage(MessageProccesing.CreateMessage(Options.LOGIN, login));
            loginNotFinished = true;
            do
            {

            } while (loginNotFinished);

        }

        public static void loginReply(String message)
        {
            String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            ErrorCodes error = (ErrorCodes)int.Parse(replySplit[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1]);
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            string title = "Logowanie";
            String msg = "";
            switch (error)
            {
                case ErrorCodes.NO_ERROR:
                    Program.isLoggedIn = true;
                    break;
                case ErrorCodes.USER_NOT_FOUND:
                    msg = "Nie odnaleziono użytkownika!";
                    MessageBox.Show(msg, title, buttons);
                    break;
                case ErrorCodes.INCORRECT_PASSWORD:
                    msg = "Błędne hasło!";
                    MessageBox.Show(msg, title, buttons);
                    break;
                case ErrorCodes.USER_ALREADY_LOGGED_IN:
                    msg = "Użytkownik już zalogowany!";
                    MessageBox.Show(msg, title, buttons);
                    break;

            }
            loginNotFinished = false;
        }



        public static void register(String username, String pass)
        {
            Login login = new Login(username, pass);
            ServerProcessing.processSendMessage(MessageProccesing.CreateMessage(Options.CREATE_USER, login));
        }

        public static void registerReply(String message)
        {
            String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            ErrorCodes error = (ErrorCodes)int.Parse(replySplit[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1]);

            MessageBoxButtons buttons = MessageBoxButtons.OK;
            string title = "Rejestracja";
            String msg = "";
            switch (error)
            {
                case ErrorCodes.NO_ERROR:
                    msg = "Pomyślnie zarejestrowano! Proszę się zalogować.";
                    MessageBox.Show(msg, title, buttons);
                    break;
                case ErrorCodes.USER_ALREADY_EXISTS:
                    msg = "Taki użytkownik już istnieje!";
                    MessageBox.Show(msg, title, buttons);
                    break;
            }
        }
    }
}
