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
        private static String connectionIp = Shared.IP.serverIp;

        private static ByteCallback receiveMsgCallback;

        private struct UdpState
        {
            public UdpClient uClientRecv;
            //public UdpClient uClientSend;
            public Socket socketSend;
            public IPEndPoint ePointRecv;
            public IPEndPoint ePointSend;
        }

        private static UdpState udpState;

        public static ByteCallback ReceiveMsgCallback { get => receiveMsgCallback; set => receiveMsgCallback = value; }

        public static void Start()
        {
            IPAddress ip;
            IPAddress.TryParse(connectionIp, out ip);

            IPEndPoint eRecv = new IPEndPoint(ip, connectionPortRecv);
            //IPEndPoint eSend = new IPEndPoint(ip, connectionPortSend);
            UdpClient uRecv = new UdpClient(connectionPortRecv);
            //UdpClient uSend = new UdpClient(eSend);

            Socket sSend = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPEndPoint eSend = new IPEndPoint(ip, connectionPortSend);

            udpState.ePointRecv = eRecv;
            udpState.ePointSend = eSend;
            udpState.uClientRecv = uRecv;
            udpState.socketSend = sSend;
            //udpState.uClientSend = uSend;

            ReceiveMessages();
        }

        public static void Stop()
        {
            udpState.uClientRecv.Close();
            //udpState.uClientSend.Close();
            udpState.socketSend.Close();
            udpState.ePointRecv = null;
            udpState.ePointSend = null;
            udpState.socketSend = null;
            udpState.uClientRecv = null;
        }

        
        public static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                UdpClient u = ((UdpState)(ar.AsyncState)).uClientRecv;
                IPEndPoint e = ((UdpState)(ar.AsyncState)).ePointRecv;

                if (u == null || e == null || u.Client == null)
                    return;

                byte[] receiveBytes = u.EndReceive(ar, ref e);
                ReceiveMsgCallback(receiveBytes);

                if (u == null || e == null || u.Client == null)
                    return;

                ReceiveMessages();
                //TODO: callback to inCallForm with sound or to sound processing method
                //string receiveString = Encoding.ASCII.GetString(receiveBytes);
            } catch (Exception e)
            {
                return;
            }
        }

        public static void ReceiveMessages()
        {
            (udpState.uClientRecv).BeginReceive(new AsyncCallback(ReceiveCallback), udpState);
        }

        public static void SendMessages(byte[] msg)
        {
            if (udpState.socketSend == null || udpState.ePointSend == null)
                return;
            (udpState.socketSend).SendTo(msg, udpState.ePointSend);
        }
    }
}
