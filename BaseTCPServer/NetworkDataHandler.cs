using System;
using System.Collections.Generic;
using System.Text;
using BaseTCPServerBindings;

namespace BaseTCPServer
{
    public class NetworkDataHandler
    {
        private delegate void PacketData(int index, byte[] data);
        private static Dictionary<int, PacketData> Packets;

        public static void InitializeNetworkPackages()
        {
            Console.WriteLine("Initializing network packages...");
            Packets = new Dictionary<int, PacketData>
            {
                { (int)ClientPackets.CThankYou, HandleThankYou }, { (int)ClientPackets.CStringMessage, HandleStringMessage}
            };
        }

        public static void HandleNetworkInformation(int index, byte[] data)
        {
            int packetNum;
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            packetNum = buffer.ReadInteger();
            buffer.Dispose();
            if (Packets.TryGetValue(packetNum, out PacketData Packet))
            {
                Packet.Invoke(index, data);
            }
        }

        private static void HandleThankYou(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();

            System.Diagnostics.Debug.WriteLine(string.Format("From {0}: {1}", index, msg));
        }

        private static void HandleConnectionOK(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();

            Console.WriteLine(msg);
        }

        private static void HandleStringMessage(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();
            Console.WriteLine(msg);
        }
    }
}
