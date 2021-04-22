using Shared;
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

        /// <summary>
        /// <para> Key: <c>ConversationId,ReceiverClientId</c> </para> 
        /// <para> Value: <c>List of messages</c> </para>
        /// </summary>
        private Dictionary<int,Dictionary<IPAddress, Queue<byte[]>>> voiceToSend;
        private int port;
        /// <summary>
        /// <para> Key: <c>clientIp</c> </para>
        /// <para> Value: <c>handler</c> </para>
        /// </summary>
        private Dictionary<IPAddress, EventWaitHandle> userNewVoiceHandler;

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
            var udpTokenSource = new CancellationTokenSource();
            var udpToken = udpTokenSource.Token;

            TcpClient client = obj as TcpClient;
            IPAddress clientIp = IPAddress.Parse((((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()));
            NetworkStream stream = client.GetStream();
            int clientId = menager.AddActiveUser();
            #pragma warning disable CS4014 
            Task.Run(() => ServerMessages(clientId, stream,token),token);
            #pragma warning restore CS4014 
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

                    try
                    {
                        //Prepare response
                        sendMessage = menager.ProccesClient(messageData.ToString(), clientId);
                    }
                    // Tells server to do something
                    catch(CustomException e)
                    {
                        // Start new UDPs and return no error
                        string[] fields = e.Message.Split("$$", StringSplitOptions.RemoveEmptyEntries);
                        Options option = (Options)int.Parse(fields[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1]);
                        InvitationId conversationId = MessageProccesing.DeserializeObject(e.Message) as InvitationId;

                        if (option == Options.JOIN_CONVERSATION)
                        {
                            Task.Run(() => UdpRead(clientIp, port,conversationId.invitationId));
                            Task.Run(() => UdpWrite(clientIp,port, conversationId.invitationId, token), token);
                            sendMessage = MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR);
                        }
                        else if(option == Options.LEAVE_CONVERSATION)
                        {
                            udpTokenSource.Cancel();
                            sendMessage = MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR);
                        }
        
                    }                 
                    message = Encoding.ASCII.GetBytes(sendMessage);
                    //Send response
                    stream.Write(message);
                }
                catch (Exception e)
                {
                    udpTokenSource.Cancel();
                    tokenSource.Cancel();
                    menager.Disconnect(clientId);
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
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }


        // One Task for one Client
        public void UdpWrite(IPAddress clientIp,int port, int conversationId, CancellationToken ct)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ep = new IPEndPoint(clientIp, port);
            while (true)
            {
                userNewVoiceHandler[clientIp].WaitOne();
                lock (userNewVoiceHandler[clientIp])
                {
                    if (ct.IsCancellationRequested)
                    {
                        return;
                    }
                    s.SendTo(voiceToSend[conversationId][clientIp].Dequeue(), ep);
                    userNewVoiceHandler[clientIp].Reset();
                }
            }
        }

        // One Task for one Client, make try catch
        public void UdpRead(IPAddress clientIp,int port,int conversationId)
        {
            voiceToSend[conversationId][clientIp] = new Queue<byte[]>();
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(clientIp, port);
            UdpClient receivingUdpClient = new UdpClient(11000);
            while (true)
            {
                byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);
                foreach (var key in voiceToSend[conversationId].Keys)
                {
                    // Key is clientIp
                    if(key != clientIp)
                    {
                        lock (voiceToSend[conversationId][key]) voiceToSend[conversationId][key].Enqueue(receiveBytes);
                        lock (userNewVoiceHandler[key]) userNewVoiceHandler[key].Set();
                    }
                }
            }

        }
        public ServerConnection()
        {
            menager = new ClientProcessing();
            port = 11000;
            RunServer();
        }
    }
}
