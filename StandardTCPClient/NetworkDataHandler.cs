using System;
using System.Collections.Generic;
using System.Text;
using StandardTCPServerBindings;

namespace StandardTCPClient
{
    public class NetworkDataHandler
    {
        private delegate void PacketData(byte[] data);
        private static Dictionary<int, PacketData> Packets;

        public static void InitializeNetworkPackages()
        {
            Console.WriteLine("Initializing network packages...");
            Packets = new Dictionary<int, PacketData>
            {
                { (int)ServerPackets.SConnectionOk, HandleConnectionOK }, { (int)ServerPackets.SStringMessage, HandleStringMessage}
            };
        }

        public static void HandleNetworkInformation(byte[] data)
        {
            int packetNum;
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            packetNum = buffer.ReadInteger();
            buffer.Dispose();
            if (Packets.TryGetValue(packetNum, out PacketData Packet))
            {
                Packet.Invoke(data);
            }
        }

        private static void HandleConnectionOK(byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();
            Console.WriteLine(msg);
            TCPClient.ThankYouServer();
            if (TCPClient.OnStringMessageReceived != null)
            {
                TCPClient.OnStringMessageReceived.Invoke(msg);
            }
        }

        private static void HandleStringMessage(byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();
            Console.WriteLine(msg);
            if (TCPClient.OnStringMessageReceived != null)
            {
                TCPClient.OnStringMessageReceived.Invoke(msg);
            }
        }
    }
}
