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
        IPEndPoint localEndPoint;

        private SocketAsyncEventArgs connectArgs;
        private SocketAsyncEventArgs sendArgs;
        private SocketAsyncEventArgs receiveArgs;

        public void Start(IPEndPoint LocalEndPoint)
        {
            // Endpoint 설정
            localEndPoint = LocalEndPoint;

            // 서버 연결 시작
            StartConnect();
        }

        public void StartConnect()
        {
            try
            {
                // 서버 Connect 를 위한 SocketAsyncEventArgs 객체 초기화
                connectArgs = null;
                connectArgs = new SocketAsyncEventArgs();
                connectArgs.Completed += new EventHandler<SocketAsyncEventArgs>(ConnectEventArg_Completed);
                connectArgs.RemoteEndPoint = localEndPoint;

                // 클라이언트 소켓 초기화
                clientSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // 서버 연결 시도. 바로 연결 시 false 반환, 지연 시 true 반환
                Reconnect_Server();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(1000);

                StartConnect();
            }
        }

        // 서버 연결 완료 시 실행되는 이벤트 함수
        void ConnectEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            // 서버 연결 오류 시
            if (e.SocketError != SocketError.Success)
            {
                Reconnect_Server();

                return;
            }

            Init_Send_Receive();

            Console.WriteLine("Server Connect Complete!");
        }

        private void Init_Send_Receive()
        {
            // 데이터 송신을 위한 SocketAsyncEventArgs 객체 생성
            sendArgs = new SocketAsyncEventArgs();
            sendArgs.Completed += IO_Completed;
            SendMessage("Connect Complete!");

            // 데이터 수신을 위한 SocketAsyncEventArgs 객체 생성
            receiveArgs = new SocketAsyncEventArgs();
            receiveArgs.SetBuffer(new byte[1024], 0, 1024);
            receiveArgs.Completed += IO_Completed;

            // 비동기 데이터 수신
            if (!clientSocket.ReceiveAsync(receiveArgs))
            {
                // 서버 연결 오류 시
                if (receiveArgs.SocketError != SocketError.Success || receiveArgs.BytesTransferred == 0)
                {
                    Reconnect_Server();

                    return;
                }

                ProcessReceive(receiveArgs);
            }
        }

        // 데이터 송수신 완료 시 실행될 이벤트 함수
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

        // 메시지 입력 시 서버로 메시지 전송
        public void SendMessage(string message)
        {
            // 입력받은 메시지를 byte 배열로 변환
            byte[] messageBuffer = Encoding.UTF8.GetBytes(message);

            sendArgs.SetBuffer(messageBuffer, 0, messageBuffer.Length);

            if(clientSocket.Connected == false)
            {
                Reconnect_Server();

                return;
            }

            // 비동기 데이터 전송 작업
            if (!clientSocket.SendAsync(sendArgs))
            {
                // 서버 연결 오류 시
                if (sendArgs.SocketError != SocketError.Success || sendArgs.BytesTransferred == 0)
                {
                    Reconnect_Server();

                    return;
                }

                ProcessSend(sendArgs);
            }
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                Console.WriteLine("메시지 전송 중 오류 발생.");
                return;
            }

            Console.WriteLine($"서버에 전송한 메시지: {Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred)}");
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
            else
            {
                Reconnect_Server();
            }
        }

        object reconnectLock = new object();

        private void Reconnect_Server()
        {
            try
            {
                Console.WriteLine("서버와 연결중 . . .");

                Thread.Sleep(1000);

                bool willRaiseEvent = clientSocket.ConnectAsync(connectArgs);
                if (!willRaiseEvent)
                {
                    Init_Send_Receive();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("서버 연결 중 오류 발생 : " + e.Message);
            }
        }

        public void CloseClientSocket()
        {
            try
            {
                clientSocket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception) { }
            clientSocket.Close();
        }
    }
}
