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

        public void Start(IPEndPoint LocalEndPoint)
        {
            // 클라이언트 소켓 초기화
            clientSocket = new Socket(LocalEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

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
                SocketAsyncEventArgs connectEventArg = new SocketAsyncEventArgs();
                connectEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(ConnectEventArg_Completed);
                connectEventArg.RemoteEndPoint = localEndPoint;

                // 서버 연결 시도. 바로 연결 시 false 반환, 지연 시 true 반환
                bool willRaiseEvent = clientSocket.ConnectAsync(connectEventArg);

                if (!willRaiseEvent)
                {
                    Console.WriteLine("Server Connect Complete!");
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }

        // 서버 연결 완료 시 실행되는 이벤트 함수
        void ConnectEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            // 서버 연결 오류 시
            if (e.SocketError != SocketError.Success || e.BytesTransferred == 0)
            {
                Console.WriteLine("서버 연결 시도중 . . .");
                StartConnect();

                return;
            }

            Console.WriteLine("Server Connect Complete!");
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

            // 데이터 송수신을 위한 SocketAsyncEventArgs 객체 생성
            SocketAsyncEventArgs IOEventArgs = new SocketAsyncEventArgs();
            IOEventArgs.SetBuffer(messageBuffer, 0, messageBuffer.Length);
            IOEventArgs.Completed += IO_Completed;
            
            // 비동기 데이터 전송 작업
            if (!clientSocket.SendAsync(IOEventArgs))
            {
                // 서버 연결 오류 시
                if (IOEventArgs.SocketError != SocketError.Success || IOEventArgs.BytesTransferred == 0)
                {
                    Console.WriteLine("서버 연결 시도중 . . .");
                    StartConnect();

                    return;
                }

                ProcessSend(IOEventArgs);
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

            // 데이터 수신을 위한 버퍼 설정
            e.SetBuffer(new byte[1024], 0, 1024);

            // 비동기 데이터 수신
            if (!clientSocket.ReceiveAsync(e))
            {
                // 서버 연결 오류 시
                if (e.SocketError != SocketError.Success || e.BytesTransferred == 0)
                {
                    Console.WriteLine("서버 연결 시도중 . . .");
                    StartConnect();

                    return;
                }

                ProcessReceive(e);
            }
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
                Console.WriteLine("서버와 연결중 . . .");
                StartConnect();
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
