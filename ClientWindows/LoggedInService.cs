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
            //do
            //{

            //} while (logoutNotFinished);
            //String msg = "Pomyślnie wylogowano!";
            //MessageBoxButtons buttons = MessageBoxButtons.OK;
            //string title = "Wylogowanie";
            //MessageBox.Show(msg, title, buttons);
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
                    Program.isLoggedIn = false;
                    logoutNotFinished = false;
                    msg = "Pomyślnie wylogowano!";
                    MessageBox.Show(msg, title, buttons);
                    break;
            }
        }

        public static void getFriends()
        {
            ServerProcessing.processSendMessage(MessageProccesing.CreateMessage(Options.GET_FRIENDS));
        }

        public static void getFriendsReply(String message)
        {
            String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            ErrorCodes error = (ErrorCodes)int.Parse(replySplit[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1]);
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            string title = "Lista znajomych";
            String msg = "";
            switch (error)
            {
                case ErrorCodes.NO_ERROR:
                    Program.isLoggedIn = false;
                    logoutNotFinished = false;
                    msg = "Pomyślnie uzyskano liste znajomych! "+message;
                    MessageBox.Show(msg, title, buttons);
                    break;
            }
        }

        public static void addFriend(String user)
        {
            ServerProcessing.processSendMessage(MessageProccesing.CreateMessage(Options.ADD_FRIEND, new Username(user)));
        }

        public static void addFriendReply(String message)
        {
            String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            ErrorCodes error = (ErrorCodes)int.Parse(replySplit[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1]);
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            string title = "Dodanie znajomego";
            String msg = "";
            switch (error)
            {
                case ErrorCodes.NO_ERROR:
                    msg = "Pomyślnie wysłano zaproszenie!";
                    break;
                case ErrorCodes.ALREADY_FRIENDS:
                    msg = "Jesteście już znajomymi!";
                    break;
                case ErrorCodes.SELF_INVITE_ERROR:
                    msg = "Nie możesz zaprosić samego siebie";
                    break;
                case ErrorCodes.NOT_LOGGED_IN:
                    msg = "Jesteś nie zalogowany!";
                    break;
                case ErrorCodes.INVITATION_ALREADY_EXIST:
                    msg = "Już wysłałeś zaproszenie do tego użytkownika!";
                    break;
            }
            MessageBox.Show(msg, title, buttons);
        }

        public static void incomingInvitation(String message)
        {
            String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            Options opt = (Options)int.Parse(replySplit[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1]);

            MessageBoxButtons buttons = MessageBoxButtons.OK;
            string title = "Przychodzące zaproszenie";

            MessageBox.Show(message, title, buttons);
        }
    }
}
