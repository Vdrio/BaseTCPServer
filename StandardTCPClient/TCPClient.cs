using System;
using System.Net;
using System.Net.Sockets;
using StandardTCPServerBindings;
using System.Collections.Generic;

namespace StandardTCPClient
{
    public class TCPClient
    {

        public delegate void StringMessageReceived(string data);
        public delegate void ActionStatusUpdate(string data);
        public static StringMessageReceived OnStringMessageReceived;
        private static Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private byte[] AsyncBuffer = new byte[1024];

        public static void ConnectToServer()
        {
            Console.WriteLine("Connecting to server...");
            ClientSocket.BeginConnect("192.168.1.101", 5555, new AsyncCallback(ConnectCallback), ClientSocket);
        }

        public static void ConnectToServer(string ip, int port)
        {
            Console.WriteLine("Connecting to server...");
            ClientSocket.BeginConnect(ip, port, new AsyncCallback(ConnectCallback), ClientSocket);
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            ClientSocket.EndConnect(ar);
            while (true)
            {
                OnReceive();
            }
        }

        private static void OnReceive()
        {
            byte[] sizeInfo = new byte[4];
            byte[] receivedBuffer = new byte[1024];
            int totalRead = 0, currentRead = 0;

            try
            {
                currentRead = totalRead = ClientSocket.Receive(sizeInfo);
                if (totalRead <= 0)
                {
                    Console.WriteLine("Not connected to server.");
                }
                else
                {
                    Console.WriteLine("Reading data...");
                    while (totalRead < sizeInfo.Length && currentRead > 0)
                    {
                        currentRead = ClientSocket.Receive(sizeInfo, totalRead, sizeInfo.Length - totalRead, SocketFlags.None);
                        totalRead += currentRead;
                    }

                    int messageSize = 0;
                    messageSize |= sizeInfo[0];
                    messageSize |= (sizeInfo[1] << 8);
                    messageSize |= (sizeInfo[2] << 16);
                    messageSize |= (sizeInfo[3] << 24);

                    byte[] data = new byte[messageSize];
                    totalRead = 0;
                    currentRead = totalRead = ClientSocket.Receive(data, totalRead, data.Length - totalRead, SocketFlags.None);
                    while (totalRead < messageSize && currentRead > 0)
                    {
                        currentRead = ClientSocket.Receive(data, totalRead, data.Length - totalRead, SocketFlags.None);
                        totalRead += currentRead;
                    }

                    NetworkDataHandler.HandleNetworkInformation(data);


                }
            }
            catch
            {
                Console.WriteLine("Not connected to server.");
            }
        }

        public static void SendData(byte[] data)
        {
            ClientSocket.Send(data);
        }

        public static void ThankYouServer()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ClientPackets.CThankYou);
            buffer.WriteString("Thank you for letting me connect");
            SendData(buffer.ToArray());
            buffer.Dispose();
        }

    }
}
