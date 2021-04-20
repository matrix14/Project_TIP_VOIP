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
            ServerConnectorSync serverConn = new ServerConnectorSync();
            String reply = serverConn.sendMessageAndGetReply(MessageProccesing.CreateMessage(Options.LOGIN, login));
            string title = "Login reply";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBox.Show(reply, title, buttons);
            /*
            if(username.Length>3&&pass.Length>3) //TODO: change it
            {
                string msg = "You did not enter a server name. Cancel this operation?"+username+" "+pass;
                string title = "Wrong data!";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(msg, title, buttons);
            } else
            {
                return;
            }
            */

        }

        public static void register(String username, String pass)
        {
            Login login = new Login(username, pass);
            ServerConnectorSync serverConn = new ServerConnectorSync();
            String reply = serverConn.sendMessageAndGetReply(MessageProccesing.CreateMessage(Options.CREATE_USER, login));
            string title = "Register reply";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBox.Show(reply, title, buttons);
        }
    }
}
