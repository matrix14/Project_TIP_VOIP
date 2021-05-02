using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientWindows
{
    class CallProcessing
    {
        private static int connectionPort = 11000;
        private static String connectionIp = "10.1.1.1"; //TODO: server IP address

        private static ByteCallback receiveMsgCallback;

        private struct UdpState
        {
            public UdpClient uClient;
            public IPEndPoint ePointRec;
            public IPEndPoint ePointSend;
        }

        private static UdpState udpState;

        public static ByteCallback ReceiveMsgCallback { get => receiveMsgCallback; set => receiveMsgCallback = value; }

        public static void Start()
        {
            IPAddress ip;
            IPAddress.TryParse(connectionIp, out ip);

            IPEndPoint e = new IPEndPoint(IPAddress.Any, connectionPort);
            IPEndPoint eSend = new IPEndPoint(ip, connectionPort);
            UdpClient u = new UdpClient(e);

            udpState.ePointRec = e;
            udpState.ePointSend = eSend;
            udpState.uClient = u;

            ReceiveMessages();
        }

        public static void Stop()
        {
            udpState.uClient.Close();
        }

        
        public static void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient u = ((UdpState)(ar.AsyncState)).uClient;
            IPEndPoint e = ((UdpState)(ar.AsyncState)).ePointRec;

            byte[] receiveBytes = u.EndReceive(ar, ref e);
            ReceiveMsgCallback(receiveBytes);
            ReceiveMessages();
            //TODO: callback to inCallForm with sound or to sound processing method
            //string receiveString = Encoding.ASCII.GetString(receiveBytes);
        }

        public static void ReceiveMessages()
        {
            (udpState.uClient).BeginReceive(new AsyncCallback(ReceiveCallback), udpState);
        }

        public static void SendMessages(byte[] msg)
        {
            (udpState.uClient).SendAsync(msg, msg.Length, udpState.ePointSend);
        }
    }
}
