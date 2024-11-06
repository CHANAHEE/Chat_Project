using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientProgram
{
    internal class Client
    {
        Socket clientSocket;

        public void Start(IPEndPoint localEndPoint)
        {
            // 클라이언트 소켓 초기화
            clientSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // 서버 Connect 를 위한 SocketAsyncEventArgs 객체 초기화
            SocketAsyncEventArgs connectEventArg = new SocketAsyncEventArgs();
            connectEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(ConnectEventArg_Completed);
            connectEventArg.RemoteEndPoint = localEndPoint;

            // 서버 연결 시작
            StartConnect(connectEventArg);
        }

        public void StartConnect(SocketAsyncEventArgs connectEventArg)
        {
            try
            {
                // 서버 연결 시도. 바로 연결 시 false 반환, 지연 시 true 반환
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
            // 서버 연결 오류 시
            if (e.SocketError != SocketError.Success || e.BytesTransferred == 0)
            {
                Console.WriteLine("서버 연결 시도중 . . .");
                StartConnect(e);

                return;
            }

            // 서버 연결 성공 시
            ProcessConnect(e);
        }

        private void ProcessConnect(SocketAsyncEventArgs e)
        {
            // 데이터 송수신을 위한 SocketAsyncEventArgs 객체 초기화
            SocketAsyncEventArgs readWriteEventArg;

            readWriteEventArg = new SocketAsyncEventArgs();
            readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(ReceiveCompleted);

            // 비동기 데이터 수신
            if (!clientSocket.ReceiveAsync(readWriteEventArg))
            {
                // 서버 연결 오류 시
                if (e.SocketError != SocketError.Success || e.BytesTransferred == 0)
                {
                    Console.WriteLine("서버 연결 시도중 . . .");
                    StartConnect(e);

                    return;
                }

                ProcessReceive(readWriteEventArg);
            }

            Console.WriteLine("Server Connect Complete!");
        }

        private void ReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success || e.BytesTransferred == 0)
            {
                Console.WriteLine("서버 연결 시도중 . . .");

                StartConnect(e);

                return;
            }

            ProcessReceive(e);
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                string message = Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred);
                Console.WriteLine(message);

                // 계속해서 데이터 수신
                if (!clientSocket.ReceiveAsync(e))
                {
                    ProcessReceive(e);
                }
            }
            else if(e.SocketError == SocketError.NotConnected)
            {
                Console.WriteLine("서버와 연결중 . . .");
                StartConnect(e);

                return;
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        public void SendMessage(string message)
        {
            byte[] messageBuffer = Encoding.UTF8.GetBytes(message);

            SocketAsyncEventArgs sendEventArgs = new SocketAsyncEventArgs();
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
