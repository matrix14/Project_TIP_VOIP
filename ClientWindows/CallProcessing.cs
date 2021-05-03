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
        private static int connectionPortRecv = 11000;
        private static int connectionPortSend = 11001;

        private static ByteCallback receiveMsgCallback;

        private struct UdpState
        {
            public UdpClient uClientRecv;
            public UdpClient uClientSend;
            public IPEndPoint ePointRecv;
            public IPEndPoint ePointSend;
        }

        private static UdpState udpState;

        public static ByteCallback ReceiveMsgCallback { get => receiveMsgCallback; set => receiveMsgCallback = value; }

        public static void Start()
        {
            IPAddress ip;
            IPAddress.TryParse(Shared.IP.serverIp, out ip);

            IPEndPoint eRecv = new IPEndPoint(IPAddress.Any, connectionPortRecv);
            IPEndPoint eSend = new IPEndPoint(ip, connectionPortSend);
            UdpClient uRecv = new UdpClient(eRecv);
            UdpClient uSend = new UdpClient(eSend);

            udpState.ePointRecv = eRecv;
            udpState.ePointSend = eSend;
            udpState.uClientRecv = uRecv;
            udpState.uClientSend = uSend;

            ReceiveMessages();
        }

        public static void Stop()
        {
            udpState.uClientRecv.Close();
            udpState.uClientSend.Close();
        }

        
        public static void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient u = ((UdpState)(ar.AsyncState)).uClientRecv;
            IPEndPoint e = ((UdpState)(ar.AsyncState)).ePointRecv;

            byte[] receiveBytes = u.EndReceive(ar, ref e);
            ReceiveMsgCallback(receiveBytes);
            ReceiveMessages();
            //TODO: callback to inCallForm with sound or to sound processing method
            //string receiveString = Encoding.ASCII.GetString(receiveBytes);
        }

        public static void ReceiveMessages()
        {
            (udpState.uClientRecv).BeginReceive(new AsyncCallback(ReceiveCallback), udpState);
        }

        public static void SendMessages(byte[] msg)
        {
            (udpState.uClientSend).SendAsync(msg, msg.Length, udpState.ePointSend);
        }
    }
}
