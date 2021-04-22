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
        private static Options lastOptions;

        //TODO semaphore for sync request (cannot send new request if old not finished)
        public static void ProcessServerMessageCallback(IAsyncResult ar)
        {
            // Get the listener that handles the client request.
            TcpListener listener = (TcpListener)ar.AsyncState;

            // End the operation and display the received data on
            // the console.
            TcpClient client = listener.EndAcceptTcpClient(ar);

            // Process the connection here. (Add the client to a
            // server table, read data, etc.)
            Console.WriteLine("Client connected completed");

            // Signal the calling thread to continue.
            tcpClientConnected.Set();
        }

        public static void processSendMessage(String message)
        {
            lastOptions = (Options)int.Parse((message.Split(new String[] { "$$" }, StringSplitOptions.RemoveEmptyEntries)[0])
                .Split(new String[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1]);
            ServerConnectorAsync.SendMessage(message);
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
                }
            } else
            {
                //TODO Implement faults
            }
        }
    }
}
