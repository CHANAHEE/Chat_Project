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
            SocketAsyncEventArgs readWriteEventArg;

            readWriteEventArg = new SocketAsyncEventArgs();
            readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(ReceiveCompleted);

            if (!clientSocket.ReceiveAsync(readWriteEventArg))
            {
                ProcessReceive(readWriteEventArg);
            }

            Console.WriteLine("Connect Start!");
        }

        private void ReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success || e.BytesTransferred == 0)
            {
                Console.WriteLine("서버와의 연결이 끊어졌습니다.");
                return;
            }

            ProcessReceive(e);

            // 계속해서 데이터 수신
            if (!clientSocket.ReceiveAsync(e))
            {
                ProcessReceive(e);
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                string message = Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred);
                Console.WriteLine(message);
            }
            else
            {
                CloseClientSocket(e);
            }
        }
        public void SendMessage(string message)
        {
            byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
            var sendEventArgs = new SocketAsyncEventArgs();
            sendEventArgs.SetBuffer(messageBuffer, 0, messageBuffer.Length);
            sendEventArgs.Completed += SendCompleted;

            if (!clientSocket.SendAsync(sendEventArgs))
            {
                SendCompleted(this, sendEventArgs);
            }
        }

        private void SendCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                Console.WriteLine("메시지 전송 중 오류 발생.");
                return;
            }

            Console.WriteLine($"서버에 전송한 메시지: {Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred)}");
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
