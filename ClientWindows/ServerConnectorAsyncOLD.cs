using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientWindows
{
    static class ServerConnectorAsyncOLD
    {
        private const Int32 MAX_BUFFER_SIZE = 1024;

        private static String address = "localhost";
        private static Int32 port = 13579;
        private static TcpClient tcpclient = null;
        private static NetworkStream stream = null;

        /*
        public ServerConnectorAsync() { }

        public ServerConnectorAsync(String address, Int32 port)
        {
            this.address = address;
            this.port = port;
        }
        */
        public static void setPort(Int32 p)
        {
            if (!checkIfConnectionExists())
            {
                port = p;
            }
        }

        public static String getAddress()
        {
            return address;
        }

        public static void setAddress(String a)
        {
            if (!checkIfConnectionExists())
            {
                address = a;
            }
        }

        public static Int32 getPort()
        {
            return port;
        }

        public static TcpClient getTcpListener()
        {
            return tcpclient;
        }



        private static void Connect() //TODO: add server connection data configuration
        {
            try
            {
                tcpclient = new TcpClient(address, port);
                stream = tcpclient.GetStream();
                
            } catch(System.Net.Sockets.SocketException) {
                return;
            }
            
        }

        private static Boolean checkIfConnectionExists()
        {
            if(stream!=null)
            {
                return true;
            } else
            {
                return false;
            }
        }

        private static void Disconnect()
        {
            stream.Close();
            tcpclient.Close();
            stream = null;
        }

        private static void connectIfNeeded()
        {
            if(checkIfConnectionExists()==false)
            {
                Connect();
            }
        }

        public static String sendMessageAndGetReply(String message)
        {
            connectIfNeeded();
            if (!checkIfConnectionExists()) return "Error in creating connection!";
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            data = new Byte[MAX_BUFFER_SIZE];
            Int32 msgLength = stream.Read(data, 0, MAX_BUFFER_SIZE);
            return System.Text.Encoding.ASCII.GetString(data, 0, msgLength);
        }

        public static String sendMessage(String message)
        {
            connectIfNeeded();
            if (!checkIfConnectionExists()) return "Error in creating connection!";
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            return "Ok";
        }
    }
}
