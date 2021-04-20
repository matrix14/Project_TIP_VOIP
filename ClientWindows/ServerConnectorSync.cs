using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientWindows
{
    class ServerConnectorSync
    {
        private const Int32 MAX_BUFFER_SIZE = 1024;

        private String address = "localhost";
        private Int32 port = 13579;
        private TcpClient tcpclient = null;
        private NetworkStream stream = null;


        public ServerConnectorSync() { }

        public ServerConnectorSync(String address, Int32 port)
        {
            this.address = address;
            this.port = port;
        }

        private void Connect() //TODO: add server connection data configuration
        {
            try
            {
                this.tcpclient = new TcpClient(this.address, this.port);
                this.stream = tcpclient.GetStream();
            } catch(System.Net.Sockets.SocketException) {
                return;
            }
            
        }

        private Boolean checkIfConnectionExists()
        {
            if(this.stream!=null)
            {
                return true;
            } else
            {
                return false;
            }
        }

        private void Disconnect()
        {
            this.stream.Close();
            this.tcpclient.Close();
        }

        private void connectIfNeeded()
        {
            if(checkIfConnectionExists()==false)
            {
                this.Connect();
            }
        }

        public String sendMessageAndGetReply(String message)
        {
            connectIfNeeded();
            if (!checkIfConnectionExists()) return "Error in creating connection!";
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            this.stream.Write(data, 0, data.Length);
            data = new Byte[MAX_BUFFER_SIZE];
            Int32 msgLength = this.stream.Read(data, 0, MAX_BUFFER_SIZE);
            return System.Text.Encoding.ASCII.GetString(data, 0, msgLength);
        }

        public String sendMessage(String message)
        {
            connectIfNeeded();
            if (!checkIfConnectionExists()) return "Error in creating connection!";
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            this.stream.Write(data, 0, data.Length);
            return "Ok";
        }
    }
}
