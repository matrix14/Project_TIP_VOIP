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

            } else if(message[0]=='E')
            {
                switch(lastOptions)
                {
                    case Options.LOGIN:
                        LoginService.loginReply(message);
                        break;
                    case Options.CREATE_USER:
                        LoginService.registerReply(message);
                        break;
                }
            } else
            {
                //TODO Implement faults
            }
        }
    }
}
