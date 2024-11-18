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

        MainForm mainForm;

        bool IsCreate_SendArgs = false;

        public void Start(IPEndPoint LocalEndPoint, MainForm NewMainForm)
        {
            // Endpoint 설정
            localEndPoint = LocalEndPoint;

            // 메시지 표시를 위한 Mainform 객체
            mainForm = NewMainForm;

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

                // 서버 연결 시도. 바로 연결 시 false 반환, 지연 시 true 반환
                Connect_Server();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(1000);

                Connect_Server();
            }
        }

        // 서버 연결 완료 시 실행되는 이벤트 함수
        void ConnectEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            // 서버 연결 오류 시
            if (e.SocketError != SocketError.Success)
            {
                Connect_Server();

                return;
            }

            Init_Send_Receive();

            Console.WriteLine("Server Connect Complete!");
        }

        // 서버 연결 시도 함수
        private void Connect_Server()
        {
            try
            {
                Console.WriteLine("Connect Server . . .");

                Thread.Sleep(1000);

                if (clientSocket != null)
                {
                    clientSocket.Close();
                }

                // 클라이언트 소켓 초기화
                clientSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                bool willRaiseEvent = clientSocket.ConnectAsync(connectArgs);
                if (!willRaiseEvent)
                {
                    Init_Send_Receive();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occurred in Server Connecting : " + e.Message);
            }
        }

        private void Init_Send_Receive()
        {
            // 데이터 송신을 위한 SocketAsyncEventArgs 객체 생성
            sendArgs = new SocketAsyncEventArgs();
            sendArgs.Completed += IO_Completed;
            IsCreate_SendArgs = true;

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
                    Connect_Server();

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
            if(clientSocket.Connected == false)
            {
                Console.WriteLine("Unable Connect to Server : " + message);

                return;
            }

            // 입력받은 메시지를 byte 배열로 변환
            byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
            sendArgs.SetBuffer(messageBuffer, 0, messageBuffer.Length);

            // 비동기 데이터 전송 작업
            if (!clientSocket.SendAsync(sendArgs))
            {
                // 서버 연결 오류 시
                if (sendArgs.SocketError != SocketError.Success || sendArgs.BytesTransferred == 0)
                {
                    Connect_Server();

                    return;
                }

                ProcessSend(sendArgs);
            }
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                Console.WriteLine("Error occurred in Sending Message to Server");
                return;
            }

            Console.WriteLine($"Send Message : {Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred)}");
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if(IsCreate_SendArgs ==  false)
            {
                return;
            }

            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                string message = Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred);
                mainForm.Update_ReceiveMessage(message);

                // 계속해서 데이터 수신
                if (!clientSocket.ReceiveAsync(e))
                {
                    ProcessReceive(e);
                }
            }
            else
            {
                Connect_Server();
            }
        }

        // 소켓 연결 해제
        public void CloseClientSocket()
        {
            try
            {
                clientSocket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception e) 
            { 
                Console.WriteLine("Socket Error : " + e.Message);
            }

            clientSocket.Close();
        }
    }
}
