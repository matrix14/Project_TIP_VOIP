using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;

namespace ClientWindows
{
    static class LoggedInService
    {
        private static Boolean logoutNotFinished = false;
        private static StringCallback getFriendsCallback;
        private static StringCallback newInvitationCallback;
        private static InvitationCallback invitationProcessedCallback;
        private static BooleanCallback checkUsernameCallback;
        private static UsernameCallback newActiveFriend;
        private static UsernameCallback newInactiveFriend;
        private static DefaultCallback inviteToConversationReplyOk;
        private static BooleanCallback inviteToConversationReplyFromUser;


        private static Invitation lastProcessedInvitation = new Invitation();

        private static ManualResetEvent invitationCallbackSet = new ManualResetEvent(false);
        private static ManualResetEvent invitationProcessing = new ManualResetEvent(true);

        public static StringCallback NewInvitationCallback {
            get => newInvitationCallback; 
            set {
                newInvitationCallback = value;
                invitationCallbackSet.Set();
            } 
        }
        public static StringCallback GetFriendsCallback { get => getFriendsCallback; set => getFriendsCallback = value; }
        public static InvitationCallback InvitationProcessedCallback { get => invitationProcessedCallback; set => invitationProcessedCallback = value; }
        public static BooleanCallback CheckUsernameCallback { get => checkUsernameCallback; set => checkUsernameCallback = value; }
        public static UsernameCallback NewActiveFriend { get => newActiveFriend; set => newActiveFriend = value; }
        public static UsernameCallback NewInactiveFriend { get => newInactiveFriend; set => newInactiveFriend = value; }
        public static DefaultCallback InviteToConversationReplyOk { get => inviteToConversationReplyOk; set => inviteToConversationReplyOk = value; }
        public static BooleanCallback InviteToConversationReplyFromUser { get => inviteToConversationReplyFromUser; set => inviteToConversationReplyFromUser = value; }

        public static void logout()
        {
            ServerProcessing.processSendMessage(MessageProccesing.CreateMessage(Options.LOGOUT));
            logoutNotFinished = true;
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
                    logoutNotFinished = false;
                    Program.isLoggedIn = false;
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
                    logoutNotFinished = false;
                    msg = "Pomyślnie uzyskano liste znajomych! "+message;
                    //MessageBox.Show(msg, title, buttons);
                    getFriendsCallback(message);
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

            invitationCallbackSet.WaitOne();
            newInvitationCallback(message);
        }

        public static void newActiveFriends(String message)
        {
            String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            String userString = replySplit[1].Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries)[1];
            Username us = MessageProccesing.DeserializeObject(Options.ACTIVE_FRIENDS, userString) as Username;

            invitationCallbackSet.WaitOne();
            newActiveFriend(us);
        }

        public static void newInactiveFriends(String message)
        {
            String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            String userString = replySplit[1].Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries)[1];
            Username us = MessageProccesing.DeserializeObject(Options.INACTIVE_FRIENDS, userString) as Username;

            invitationCallbackSet.WaitOne();
            newInactiveFriend(us);
        }

        public static void acceptInvitation(Invitation inv)
        {
            invitationProcessing.WaitOne();
            invitationProcessing.Reset();
            lastProcessedInvitation = inv;
            ServerProcessing.processSendMessage(MessageProccesing.CreateMessage(Options.ACCEPT_FRIEND, new Id(inv.invitationId)));
        }

        public static void acceptInvitationReply(String message)
        {
            String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            ErrorCodes error = (ErrorCodes)int.Parse(replySplit[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1]);
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            string title = "Dodanie znajomego";
            String msg = "";
            switch (error)
            {
                case ErrorCodes.NO_ERROR:
                    msg = "Zaproszenie pomyślnie zaakceptowane!";
                    break;
                case ErrorCodes.ALREADY_FRIENDS:
                    msg = "Jesteście już znajomymi!";
                    break;
                case ErrorCodes.INVITATION_ALREADY_ACCEPTED:
                    msg = "Zaproszenie zostało już zaakceptowane!";
                    break;
                case ErrorCodes.NOT_LOGGED_IN:
                    msg = "Jesteś nie zalogowany!";
                    break;
                case ErrorCodes.WRONG_INVATATION_ID:
                    msg = "Zły idnetyfikator zaproszenia!";
                    break;
            }
            MessageBox.Show(msg, title, buttons);
            lastProcessedInvitation.status = 2;
            invitationProcessedCallback(lastProcessedInvitation);
            invitationProcessing.Set();
        }

        public static void declineInvitation(Invitation inv)
        {
            invitationProcessing.WaitOne();
            invitationProcessing.Reset();
            lastProcessedInvitation = inv;
            ServerProcessing.processSendMessage(MessageProccesing.CreateMessage(Options.DECLINE_FRIEND, new Id(inv.invitationId)));
        }

        public static void declineInvitationReply(String message)
        {
            String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            ErrorCodes error = (ErrorCodes)int.Parse(replySplit[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1]);
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            string title = "Dodanie znajomego";
            String msg = "";
            switch (error)
            {
                case ErrorCodes.NO_ERROR:
                    msg = "Zaproszenie pomyślnie odrzucone!";
                    break;
                case ErrorCodes.ALREADY_FRIENDS:
                    msg = "Jesteście już znajomymi!";
                    break;
                case ErrorCodes.INVITATION_ALREADY_ACCEPTED:
                    msg = "Zaproszenie zostało już zaakceptowane!";
                    break;
                case ErrorCodes.SELF_INVITE_ERROR:
                    msg = "Nie możesz zaprosić sam siebie!";
                    break;
                case ErrorCodes.NOT_LOGGED_IN:
                    msg = "Jesteś nie zalogowany!";
                    break;
                case ErrorCodes.WRONG_INVATATION_ID:
                    msg = "Zły identyfikator zaproszenia!";
                    break;
            }
            MessageBox.Show(msg, title, buttons);
            invitationProcessedCallback(lastProcessedInvitation);
            invitationProcessing.Set();
        }

        public static void checkIsUserExist(String username)
        {
            ServerProcessing.processSendMessage(MessageProccesing.CreateMessage(Options.CHECK_USER_NAME, new Username(username)));
        }

        public static void checkIsUserExistReply(String message)
        {
            String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            ErrorCodes error = (ErrorCodes)int.Parse(replySplit[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1]);

            if (checkUsernameCallback == null)
            {
                return;
            }

            switch (error)
            {
                case ErrorCodes.NO_ERROR:
                    checkUsernameCallback(false);
                    break;
                case ErrorCodes.USER_ALREADY_EXISTS:
                    checkUsernameCallback(true);
                    break;
            }
        }

        public static void inviteToConversation(Username u)
        {
            ServerProcessing.processSendMessage(MessageProccesing.CreateMessage(Options.INVITE_TO_CONVERSATION, u));
        }

        public static void inviteToConversationReply(String message)
        {
            String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            ErrorCodes error = (ErrorCodes)int.Parse(replySplit[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1]);

            if (inviteToConversationReplyOk == null || inviteToConversationReplyFromUser == null)
            {
                return;
            }

            switch (error)
            {
                case ErrorCodes.NO_ERROR:
                    inviteToConversationReplyOk();
                    break;
                case ErrorCodes.USER_OFFLINE:
                    inviteToConversationReplyFromUser(false);
                    break;
                case ErrorCodes.NOT_LOGGED_IN:
                    MessageBox.Show("Jesteś nie zalogowany!");
                    break;
            }
        }

        public static void inviteToConversationReplyFromUserFunc(String message, Boolean ack)
        {
            String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            String userString = replySplit[1].Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries)[1];
            Username us = MessageProccesing.DeserializeObject(Options.ACCEPTED_CALL, userString) as Username;

            if (inviteToConversationReplyOk == null || inviteToConversationReplyFromUser == null)
            {
                return;
            }

            inviteToConversationReplyFromUser(ack);
        }

        public static void incomingCall(String message)
        {
            String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            String userString = replySplit[1].Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries)[1];
            Call c = MessageProccesing.DeserializeObject(Options.INCOMMING_CALL, userString) as Call;

            //MessageBox.Show("INCOMING CALL\n\r"+message);
            IncomingCallForm icf = new IncomingCallForm(c);
            icf.ShowDialog();

            //if (inviteToConversationReplyOk == null || inviteToConversationReplyFromUser == null)
            //{
            //    return;
            //}

            //inviteToConversationReplyFromUser(ack);
        }

        public static void acceptCall(Call call)
        {
            ServerProcessing.processSendMessage(MessageProccesing.CreateMessage(Options.JOIN_CONVERSATION, call.callId));
        }

        public static void declineCall(Call call)
        {
            ServerProcessing.processSendMessage(MessageProccesing.CreateMessage(Options.LEAVE_CONVERSATION, call.callId));
        }

        public static void joinConversationAccepted(String message)
        {
            MessageBox.Show("ACK CONV \n" + message);
        } 
    }
}
