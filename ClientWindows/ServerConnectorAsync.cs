using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ClientWindows
{
    class ReceiveObject
    {
        public const int MAX_BUFFER_SIZE = 2048;
        public byte[] buffer = new byte[MAX_BUFFER_SIZE];
        public StringBuilder sb = new StringBuilder();
    }
    class ServerConnectorAsync
    {
        private static int port = 13579;
        private static String address = Shared.IP.serverIp;

        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        private static System.Timers.Timer connectionTimer = new System.Timers.Timer();

        private static Socket sock;


        public static void StartConnection()
        {
            connectionTimer.Interval = 5000;
            connectionTimer.Elapsed += new ElapsedEventHandler(connectionTimerOnTimerElapsed);
            connectionTimer.AutoReset = false;


            try
            {
                IPAddress ip = IPAddress.Parse(address);
                IPEndPoint remoteAddr = new IPEndPoint(ip, port);
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.BeginConnect(remoteAddr, new AsyncCallback(ConnectCallback), sock);
                connectionTimer.Start();
            } catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return; //TODO Add message for client when problem
            }
        }
        public static void StopConnection()
        {
            try
            {
                sock.Shutdown(SocketShutdown.Both);
                sock.Close();
            } catch(SocketException)
            {
                return;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        private static void connectionTimerOnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (sock.Connected)
            {
                connectionTimer.Stop();
            } else
            {
                connectionTimer.Stop();
                Reconnect();
            }
        }
        public static void Reconnect()
        {
            sock.Close();
            try
            {
                IPAddress ip = IPAddress.Parse(address);
                IPEndPoint remoteAddr = new IPEndPoint(ip, port);
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.BeginConnect(remoteAddr, new AsyncCallback(ConnectCallback), sock);
                connectionTimer.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return; //TODO Add message for client when problem
            }
        }
        public static void SendMessage(String message)
        {
            try
            {
                byte[] byteData = Encoding.ASCII.GetBytes(message);
                sock.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), sock);
            } catch(System.Net.Sockets.SocketException)
            {
                if(!sock.Connected)
                {
                    //TODO do something when connection is lost
                }
            } 
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return;
            }
            //sendDone.WaitOne();
        }

        private static void SendCallback(IAsyncResult res)
        {
            try
            {
                Socket sockInt = (Socket)res.AsyncState;
                int len = sockInt.EndSend(res);
                sendDone.Set();
            } catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return;
                //TODO implement fault
            }
        }

        public static void ReceiveWhile()
        {
            connectDone.WaitOne();
            do
            {
                Receive();
                receiveDone.WaitOne();
            } while (sock != null);
        }

        public static void Receive()
        {
            receiveDone.Reset();
            try
            {
                ReceiveObject ro = new ReceiveObject();
                sock.BeginReceive(ro.buffer, 0, ReceiveObject.MAX_BUFFER_SIZE, 0, new AsyncCallback(ReceiveCallback), ro);

            } catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return;
                //TODO Implement faults
            }
        }

        public static void ReceiveCallback(IAsyncResult res)
        {
            try
            {
                ReceiveObject ro = (ReceiveObject)res.AsyncState;
                int len = sock.EndReceive(res);
                ro.sb.Append(Encoding.ASCII.GetString(ro.buffer, 0, len));
                if (ro.sb.Length > 0)
                {
                    ServerProcessing.processReceivedMessage(ro.sb.ToString());
                }
                receiveDone.Set();
            }
            catch (Exception e) //TODO: System.Net.Sockets.SocketException - when server crash or close
            {
                MessageBox.Show(e.ToString());
                return;
                //TODO Implement faults
            }
        }

        private static void ConnectCallback(IAsyncResult res)
        {
            try
            {
                Socket sockInt = (Socket)res.AsyncState;
                sockInt.EndConnect(res); //TODO: ObjectDisposedException - when no connection
                if(sockInt.Connected)
                {
                    connectionTimer.Stop();
                }
                connectDone.Set();
            } catch(ObjectDisposedException)
            {

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return;
                //TODO Implement faults
            }
        }

        public static Boolean getConnectionState()
        {
            if (sock == null)
                return false;
            return sock.Connected;
        }
    }
}
