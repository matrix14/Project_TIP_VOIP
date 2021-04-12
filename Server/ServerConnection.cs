using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class ServerConnection
    {
        public ClientProcessing menager { get; set; }

        public void RunServer()
        {
            // Create a TCP/IP (IPv4) socket and listen for incoming connections.
            TcpListener listener = new TcpListener(IPAddress.Any, 13579);
            listener.Start();
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Task.Run(() => { ClientConnectionAsync(client); });
            }
        }


        /// <summary>
        /// Used to request-response type of communication like: 
        /// Login, Registry, Start new conversation, End conversation, Get Friends
        /// </summary>
        /// <param name="obj"></param>
        public async void ClientConnectionAsync(Object obj)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            TcpClient client = obj as TcpClient;
            NetworkStream stream = client.GetStream();
            int clientId = menager.AddActiveUser();
            Task.Run(() => ServerMessages(clientId, stream,token),token);
            byte[] message;
            Decoder decoder = Encoding.ASCII.GetDecoder();
            while (true)
            {
                try
                {
                    string sendMessage = "";
                    byte[] buffer = new byte[2048];
                    StringBuilder messageData = new StringBuilder();
                    int bytes = -1;

                    do
                    {
                        bytes = await stream.ReadAsync(buffer, 0, buffer.Length);
                        char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                        decoder.GetChars(buffer, 0, bytes, chars, 0);
                        messageData.Append(chars);
                    } while (stream.DataAvailable);

                    //Prepare response
                    sendMessage = menager.ProccesClient(messageData.ToString(), clientId);

                    //Disconnection
                    if (sendMessage == "")
                    {
                        message = Encoding.ASCII.GetBytes("Error:0$$");
                        stream.Write(message);
                        tokenSource.Cancel();
                        Thread.Sleep(1000);
                        break;
                    }

                    message = Encoding.ASCII.GetBytes(sendMessage);
                    //Send response
                    stream.Write(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }

            }
        }


        public void ServerMessages(int clientID,NetworkStream stream, CancellationToken ct)
        {
            List<string> messagesToSend = new List<string>();
            try
            {
                while (true)
                {
                    if (ct.IsCancellationRequested)
                    {
                        return;
                    }
                    messagesToSend = menager.CheckServerMessages(clientID);

                    foreach (string message in messagesToSend)
                    {
                        byte[] send = Encoding.ASCII.GetBytes(message);
                        stream.Write(send);
                    }
                    messagesToSend.Clear();
                    Thread.Sleep(80);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        public ServerConnection()
        {
            menager = new ClientProcessing();
            RunServer();
        }
    }
}
