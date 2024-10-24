﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerProgram
{
    internal class Server
    {
        private int connectionCount;
        Socket listenSocket;

        BufferManager bufferManager;
        const int opsToPreAlloc = 2;
        
        SocketAsyncEventArgsPool readWritePool;
        Semaphore maxNumberAcceptedClients;

        public Server(int numConnections, int receiveBufferSize)
        {
            connectionCount = numConnections;

            bufferManager = new BufferManager(receiveBufferSize * numConnections * opsToPreAlloc, receiveBufferSize);

            readWritePool = new SocketAsyncEventArgsPool(numConnections);
            maxNumberAcceptedClients = new Semaphore(numConnections, numConnections);
        }

        public void Init()
        {
            bufferManager.InitBuffer();

            SocketAsyncEventArgs readWriteEventArg;

            for (int i = 0; i < connectionCount; i++)
            {
                readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);

                bufferManager.SetBuffer(readWriteEventArg);
                
                readWritePool.Push(readWriteEventArg);
            }
        }

        public void Start(IPEndPoint localEndPoint)
        {
            listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            
            listenSocket.Bind(localEndPoint);
            listenSocket.Listen(100);

            SocketAsyncEventArgs acceptEventArg = new SocketAsyncEventArgs();
            acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
            StartAccept(acceptEventArg);
        }

        public void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            bool willRaiseEvent = false;
            while (!willRaiseEvent)
            {
                maxNumberAcceptedClients.WaitOne();

                acceptEventArg.AcceptSocket = null;
                willRaiseEvent = listenSocket.AcceptAsync(acceptEventArg);
                if (!willRaiseEvent)
                {
                    ProcessAccept(acceptEventArg);
                }
            }
        }

        void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);

            StartAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            //SocketAsyncEventArgs readEventArgs = readWritePool.Pop();
            //readEventArgs.UserToken = e.AcceptSocket;

            //bool willRaiseEvent = e.AcceptSocket.ReceiveAsync(readEventArgs);
            //if (!willRaiseEvent)
            //{
            //    ProcessReceive(readEventArgs);
            //}

            Console.WriteLine("Client is Connected!");
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

            listenSocket.Close();
        }
    }
}
