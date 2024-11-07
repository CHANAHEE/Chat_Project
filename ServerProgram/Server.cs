using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerProgram
{
    internal class Server
    {
        private int connectionCount;
        Socket serverSocket;
                
        SocketAsyncEventArgsPool readWritePool;
        Semaphore maxNumberAcceptedClients;

        MainForm mainForm;

        public Server(int numConnections, MainForm mainForm)
        {
            connectionCount = numConnections;

            readWritePool = new SocketAsyncEventArgsPool(numConnections);
            maxNumberAcceptedClients = new Semaphore(numConnections, numConnections);

            this.mainForm = mainForm;
        }

        public void Init()
        {
            SocketAsyncEventArgs readEventArg;

            for (int i = 0; i < connectionCount; i++)
            {
                readEventArg = new SocketAsyncEventArgs();
                readEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                                
                readWritePool.Push(readEventArg);
            }
        }



        public void Start(IPEndPoint localEndPoint)
        {
            // 소켓 생성
            serverSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            
            // 소켓 바인딩 (소켓이 수신할 IP 주소 및 포트 설정)
            serverSocket.Bind(localEndPoint);

            // 소켓 리스닝 (소켓 수신대기 및 갯수 설정)
            serverSocket.Listen(100);

            // 연결 비동기 작업에 대한 정보 설정
            SocketAsyncEventArgs acceptEventArg = new SocketAsyncEventArgs();
            acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);

            StartAccept(acceptEventArg);
        }

        public void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            try
            {
                bool willRaiseEvent = false;
                while (!willRaiseEvent)
                {
                    // 클라이언트 최대 수 제한
                    maxNumberAcceptedClients.WaitOne();

                    // 이전 수락 작업 초기화
                    acceptEventArg.AcceptSocket = null;

                    // 비동기 수락 작업 : 즉시 반환이면 false, 아니면 true
                    willRaiseEvent = serverSocket.AcceptAsync(acceptEventArg);
                    if (!willRaiseEvent)
                    {
                        ProcessAccept(acceptEventArg);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);

            StartAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            // 서버 UI 에 클라이언트 연결 성공 메시지 추가
            mainForm.Update_Message(DateTime.Now.ToString(), "Client Connected!");

            // Pool 에서 SocketAsyncEventArgs 객체 가져오기
            SocketAsyncEventArgs readEventArgs = readWritePool.Pop();

            // 클라이언트가 연결된 소켓을 할당하여 추후 작업 시 해당 소켓 참조 
            readEventArgs.UserToken = e.AcceptSocket;

            // readEventArgs 객체의 버퍼를 설정. 1024 바이트만큼 미리 할당. 이후 ReceiveAsync 을 통해 수신된 데이터를 저장할 버퍼.
            readEventArgs.SetBuffer(new byte[1024], 0, 1024);

            // 비동기 수신 시작
            bool willRaiseEvent = e.AcceptSocket.ReceiveAsync(readEventArgs);
            if (!willRaiseEvent)
            {
                ProcessReceive(e);
            }
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
            // 버퍼에 들어있는 데이터의 크기가 있는지와 Socket 의 연결여부를 체크
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                // 버퍼에 들어있는 아스키코드 메시지를 UTF8 로 인코딩하여 string 으로 변환
                string message = Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred);

                // 서버 UI 에 수신받은 클라이언트 메시지 추가
                mainForm.Update_Message(DateTime.Now.ToString(), message);

                // 데이터 송신을 위한 버퍼 작업
                e.SetBuffer(e.Buffer, 0, e.BytesTransferred);
                Socket socket = (Socket)e.UserToken;

                // 비동기 데이터 송신 작업
                bool willRaiseEvent = socket.SendAsync(e);
                if (!willRaiseEvent)
                {
                    ProcessSend(e);
                }
            }
            else
            {
                CloseClientSocket(e);
                mainForm.Update_Message(DateTime.Now.ToString(), "Client DisConnected!");
            }
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                Socket socket = (Socket)e.UserToken;
                e.SetBuffer(e.Buffer, 0, 1024);

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

            if(socket is null)
            {
                Console.WriteLine("Socket is Null");
                return;
            }

            try
            {
                socket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception) { }
            socket.Close();

            readWritePool.Push(e);

            maxNumberAcceptedClients.Release();
        }

        public void CloseAllClientSocket()
        {
            for(int i = 0; i < readWritePool.Count; i++)
            {
                SocketAsyncEventArgs e = readWritePool.Pop();

                Socket socket = (Socket)e.UserToken;

                if(socket == null)
                {
                    continue;
                }

                try
                {
                    socket.Shutdown(SocketShutdown.Send);
                }
                catch (Exception) { }
                socket.Close();

                maxNumberAcceptedClients.Release();                
            }

            serverSocket.Close();
        }
    }
}
