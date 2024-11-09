﻿using System;
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
                
        Semaphore maxNumberAcceptedClients;

        MainForm mainForm;

        Dictionary<Socket, SocketAsyncEventArgs> sendArgsCollection = new Dictionary<Socket, SocketAsyncEventArgs>();

        public Server(int numConnections, MainForm mainForm)
        {
            connectionCount = numConnections;

            maxNumberAcceptedClients = new Semaphore(numConnections, numConnections);

            this.mainForm = mainForm;
        }

        public void Start(IPEndPoint localEndPoint)
        {
            // 소켓 생성
            serverSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            
            // 소켓 바인딩 (소켓이 수신할 IP 주소 및 포트 설정)
            serverSocket.Bind(localEndPoint);

            // 소켓 리스닝 (소켓 수신대기 및 갯수 설정)
            serverSocket.Listen(100);

            StartAccept();
        }

        public void StartAccept()
        {
            try
            {
                bool willRaiseEvent = false;
                while (!willRaiseEvent)
                {
                    // 클라이언트 최대 수 제한
                    maxNumberAcceptedClients.WaitOne();

                    // 비동기 수락 작업 : 즉시 반환이면 false, 아니면 true
                    SocketAsyncEventArgs AcceptEventArg = new SocketAsyncEventArgs();
                    AcceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);

                    willRaiseEvent = serverSocket.AcceptAsync(AcceptEventArg);
                    if (!willRaiseEvent)
                    {
                        ProcessAccept(AcceptEventArg);
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

            StartAccept();
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            Socket ClientSocket = e.AcceptSocket;

            if(ClientSocket == null)
            {
                return;
            }

            // 서버 UI 에 클라이언트 연결 성공 메시지 추가
            mainForm.Update_Message(DateTime.Now.ToString(), "Client Connected! - " + ClientSocket.RemoteEndPoint);

            // readEventArgs 객체의 버퍼를 설정. 1024 바이트만큼 미리 할당. 이후 ReceiveAsync 을 통해 수신된 데이터를 저장할 버퍼.
            SocketAsyncEventArgs ReceiveEventArg = new SocketAsyncEventArgs();
            ReceiveEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            ReceiveEventArg.SetBuffer(new byte[1024], 0, 1024);
            ReceiveEventArg.UserToken = ClientSocket;

            SocketAsyncEventArgs SendEventArg = new SocketAsyncEventArgs();
            SendEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            SendEventArg.UserToken = ClientSocket;
            sendArgsCollection.Add(ClientSocket, SendEventArg);

            // 비동기 수신 시작
            bool willRaiseEvent = ClientSocket.ReceiveAsync(ReceiveEventArg);
            if (!willRaiseEvent)
            {
                ProcessReceive(ReceiveEventArg);
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

                // 비동기 데이터 송신 작업. 수신 받은 클라이언트 이외의 모든 클라이언트에게 데이터 전송
                foreach(KeyValuePair<Socket,SocketAsyncEventArgs> Args in sendArgsCollection)
                {
                    if(Args.Value.UserToken == e.UserToken)
                    {
                        continue;
                    }

                    // 데이터 송신을 위한 버퍼 작업
                    byte[] SendMessage = new byte[1024];                    
                    Buffer.BlockCopy(e.Buffer, 0, SendMessage, 0, e.BytesTransferred);
                    Args.Value.SetBuffer(SendMessage, 0, e.BytesTransferred);

                    bool willRaiseEvent = ((Socket)Args.Value.UserToken).SendAsync(Args.Value);
                    if (!willRaiseEvent)
                    {
                        ProcessSend(Args.Value);
                    }
                }

                if (e.SocketError == SocketError.Success)
                {
                    // 데이터 재수신을 위한 버퍼 작업
                    e.SetBuffer(e.Buffer, 0, 1024);

                    // 비동기 데이터 수신 작업
                    bool willRaiseEvent = ((Socket)e.UserToken).ReceiveAsync(e);

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
                mainForm.Update_Message(DateTime.Now.ToString(), "Send Completed!");
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

            maxNumberAcceptedClients.Release();
        }

        public void CloseAllClientSocket()
        {
            //foreach(KeyValuePair<Socket, SocketAsyncEventArgs> e in Args)
            //{
            //    Socket socket = (Socket)e.Value.UserToken;

            //    if (socket == null)
            //    {
            //        continue;
            //    }

            //    try
            //    {
            //        socket.Shutdown(SocketShutdown.Send);
            //    }
            //    catch (Exception) { }
            //    socket.Close();

            //    maxNumberAcceptedClients.Release();
            //}

            //serverSocket.Close();
        }
    }
}
