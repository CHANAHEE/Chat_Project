using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientProgram
{
    internal class Client
    {
        Socket clientSocket;

        BufferManager bufferManager;
        const int opsToPreAlloc = 2;

        public Client(int numConnections, int receiveBufferSize)
        {
            bufferManager = new BufferManager(receiveBufferSize * numConnections * opsToPreAlloc, receiveBufferSize);
        }

        public void Init()
        {
            bufferManager.InitBuffer();

            SocketAsyncEventArgs readWriteEventArg;

            readWriteEventArg = new SocketAsyncEventArgs();
            readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);

            bufferManager.SetBuffer(readWriteEventArg);
        }

        public void Start(IPEndPoint localEndPoint)
        {
            clientSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            SocketAsyncEventArgs connectEventArg = new SocketAsyncEventArgs();
            connectEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(ConnectEventArg_Completed);
            connectEventArg.RemoteEndPoint = localEndPoint;

            StartConnect(connectEventArg);
        }

        public void StartConnect(SocketAsyncEventArgs connectEventArg)
        {
            try
            {
                bool willRaiseEvent = clientSocket.ConnectAsync(connectEventArg);

                if (!willRaiseEvent)
                {
                    ProcessConnect(connectEventArg);
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }

        void ConnectEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessConnect(e);

            StartConnect(e);
        }

        private void ProcessConnect(SocketAsyncEventArgs e)
        {
            //SocketAsyncEventArgs readEventArgs = readWritePool.Pop();
            //readEventArgs.UserToken = e.AcceptSocket;

            //bool willRaiseEvent = e.AcceptSocket.ReceiveAsync(readEventArgs);
            //if (!willRaiseEvent)
            //{
            //    ProcessReceive(readEventArgs);
            //}

            Console.WriteLine("Connect Start!");
        }

        void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                e.SetBuffer(e.Offset, e.BytesTransferred);
                Socket socket = (Socket)e.UserToken;
                bool willRaiseEvent = socket.SendAsync(e);

                if (!willRaiseEvent)
                {
                    ProcessSend(e);
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                Socket socket = (Socket)e.UserToken;
                bool willRaiseEvent = socket.ReceiveAsync(e);

                if (!willRaiseEvent)
                {
                    ProcessReceive(e);
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            Socket socket = (Socket)e.UserToken;

            try
            {
                socket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception) { }
            socket.Close();
        }
    }
}
