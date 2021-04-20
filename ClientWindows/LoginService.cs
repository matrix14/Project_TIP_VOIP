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
        public static void login(String username, String pass)
        {
            Login login = new Login(username, pass);
            ServerConnectorAsync serverConn = new ServerConnectorAsync();
            String reply = serverConn.sendMessageAndGetReply(MessageProccesing.CreateMessage(Options.LOGIN, login));

            String[] replySplit = reply.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            ErrorCodes error = (ErrorCodes)int.Parse(replySplit[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1]);
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            string title = "Login";
            String msg = "";
            switch(error)
            {
                case ErrorCodes.NO_ERROR:
                    msg = "Succesful login!";
                    title = "Login";
                    MessageBox.Show(msg, title, buttons);
                    break;
                case ErrorCodes.USER_NOT_FOUND:
                    msg = "User not found!";
                    title = "Login";
                    MessageBox.Show(msg, title, buttons);
                    break;
                case ErrorCodes.INCORRECT_PASSWORD:
                    msg = "Incorrect password!";
                    title = "Login";
                    MessageBox.Show(msg, title, buttons);
                    break;
                case ErrorCodes.USER_ALREADY_LOGGED_IN:
                    msg = "User already logged in!";
                    title = "Login";
                    MessageBox.Show(msg, title, buttons);
                    break;

            }
        }

        public static void register(String username, String pass)
        {
            Login login = new Login(username, pass);
            ServerConnectorAsync serverConn = new ServerConnectorAsync();
            String reply = serverConn.sendMessageAndGetReply(MessageProccesing.CreateMessage(Options.CREATE_USER, login));

            String[] replySplit = reply.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            ErrorCodes error = (ErrorCodes)int.Parse(replySplit[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1]);
            
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            string title = "Login";
            String msg = "";
            switch (error)
            {
                case ErrorCodes.NO_ERROR:
                    msg = "Succesful register!";
                    title = "Register";
                    MessageBox.Show(msg, title, buttons);
                    break;
                case ErrorCodes.USER_ALREADY_EXISTS:
                    msg = "User already exists!";
                    title = "Register";
                    MessageBox.Show(msg, title, buttons);
                    break;
            }
        }
    }
}
