using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientWindows
{
    class CallProcessing
    {
        private static int connectionPortRecv = 11000;
        private static int connectionPortSend = 11001;
        private static String connectionIp = Shared.IP.serverIp;

        private static Boolean connectionExist = false;

        private static ByteCallback receiveMsgCallback = null;

        private static FileStream logFile;

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

            connectionExist = true;

            openLog();

            ReceiveMessages();
        }

        public static void Stop()
        {
            connectionExist = false;
            udpState.uClientRecv.Close();
            //udpState.uClientSend.Close();
            udpState.socketSend.Close();
            udpState.ePointRecv = null;
            udpState.ePointSend = null;
            udpState.socketSend = null;
            udpState.uClientRecv = null;
            if(logFile!=null)
                logFile.Close();
        }

        private static void openLog()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            logFile = File.Open(path+"\\logFile.txt", FileMode.Append, FileAccess.Write, FileShare.None);
            if (logFile == null)
                return;
            byte[] splitter = Encoding.ASCII.GetBytes("---------------------\n");
            logFile.Write(splitter, 0, splitter.Length);
        }

        private static void logToFile(byte[] b)
        {
            //if (false)
            //    return;
            if (logFile == null)
                return;
            if (b == null)
                b = Encoding.ASCII.GetBytes("NULL");
            logFile.Write(b, 0, b.Length);
            byte[] endLine = Encoding.ASCII.GetBytes("\n");
            logFile.Write(endLine, 0, endLine.Length);
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
                logToFile(receiveBytes);

                if (connectionExist == false || receiveBytes == null || receiveMsgCallback == null)
                {
                    ReceiveMessages();
                    return;
                }

                if (connectionExist&&receiveBytes!=null&&receiveMsgCallback!=null)
                    receiveMsgCallback(receiveBytes); //TODO: NullReferenceException, InvalidOperationException: BufferFull

                if (u == null || e == null || u.Client == null)
                    return;

                ReceiveMessages();
            }catch(ObjectDisposedException)
            {
                return; //TODO: System.ObjectDisposedException (when connection close)
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
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
