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
            string title = "Login";
            String msg = "";
            switch (error)
            {
                case ErrorCodes.NO_ERROR:
                    msg = "Succesful login!";
                    MessageBox.Show(msg, title, buttons);
                    Program.isLoggedIn = true;
                    
                    //((LoginForm)Program.GetActiveForm()).onLoginFinished();
                    break;
                case ErrorCodes.USER_NOT_FOUND:
                    msg = "User not found!";
                    MessageBox.Show(msg, title, buttons);
                    break;
                case ErrorCodes.INCORRECT_PASSWORD:
                    msg = "Incorrect password!";
                    MessageBox.Show(msg, title, buttons);
                    break;
                case ErrorCodes.USER_ALREADY_LOGGED_IN:
                    msg = "User already logged in!";
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
            string title = "Register";
            String msg = "";
            switch (error)
            {
                case ErrorCodes.NO_ERROR:
                    msg = "Succesful register!";
                    MessageBox.Show(msg, title, buttons);
                    break;
                case ErrorCodes.USER_ALREADY_EXISTS:
                    msg = "User already exists!";
                    MessageBox.Show(msg, title, buttons);
                    break;
            }
        }
    }
}
