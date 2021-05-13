using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shared;

namespace ClientWindows
{
    class ServerProcessing
    {
        public static ManualResetEvent tcpClientConnected = new ManualResetEvent(false);
        public static ManualResetEvent syncProcessNotCompleted = new ManualResetEvent(true);
        private static Options lastOptions;

        public static void processSendMessage(String message)
        {
            syncProcessNotCompleted.WaitOne();
            syncProcessNotCompleted.Reset();
            lastOptions = (Options)int.Parse((message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries)[0])
                .Split(new String[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1]);
            ServerConnectorAsync.SendMessage(message);
            if (lastOptions == Options.JOIN_CONVERSATION || lastOptions == Options.LEAVE_CONVERSATION) syncProcessNotCompleted.Set();
            return;
        }

        public static void processReceivedMessage(String message)
        {
            if(message[0]=='O') //ASYNC (Incoming Call, Invitation etc)
            {
                String[] replySplit = message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                Options opt = (Options)int.Parse(replySplit[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1]);
                switch(opt)
                {
                    case Options.FRIEND_INVITATIONS:
                        LoggedInService.incomingInvitation(message);
                        break;
                    case Options.ACTIVE_FRIENDS:
                        LoggedInService.newActiveFriends(message);
                        break;
                    case Options.INACTIVE_FRIENDS:
                        LoggedInService.newInactiveFriends(message);
                        break;
                    case Options.ACCEPTED_CALL:
                        LoggedInService.inviteToConversationReplyFromUserFunc(message, true);
                        break;
                    case Options.DECLINED_CALL:
                        LoggedInService.inviteToConversationReplyFromUserFunc(message, false);
                        break;
                    case Options.INCOMMING_CALL:
                        LoggedInService.incomingCall(message);
                        break;
                }
            }
            else if(message[0]=='E')
            {
                switch(lastOptions)
                {
                    case Options.LOGIN:
                        LoginService.loginReply(message);
                        break;
                    case Options.CREATE_USER:
                        LoginService.registerReply(message);
                        break;
                    case Options.LOGOUT:
                        LoggedInService.logoutReply(message);
                        break;
                    case Options.GET_FRIENDS:
                        LoggedInService.getFriendsReply(message);
                        break;
                    case Options.ADD_FRIEND:
                        LoggedInService.addFriendReply(message);
                        break;
                    case Options.ACCEPT_FRIEND:
                        LoggedInService.acceptInvitationReply(message);
                        break;
                    case Options.DECLINE_FRIEND:
                        LoggedInService.declineInvitationReply(message);
                        break;
                    case Options.CHECK_USER_NAME:
                        if(Program.isLoggedIn)
                        {
                            LoggedInService.checkIsUserExistReply(message);
                        } else
                        {
                            LoginService.checkIsUserExistReply(message);
                        }
                        break;
                    case Options.INVITE_TO_CONVERSATION:
                        LoggedInService.inviteToConversationReply(message);
                        break;
                    case Options.LEAVE_CONVERSATION:
                        break;
                    case Options.JOIN_CONVERSATION:
                        break;
                }
                syncProcessNotCompleted.Set();
            } else
            {
                //TODO Implement faults
            }
        }
    }
}
