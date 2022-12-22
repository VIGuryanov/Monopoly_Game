using System;
using System.Linq;
using System.Text;
using XProtocol.Serializator;

namespace XProtocol.Packets.Client_Server
{
    public class NickNamePacket
    {
        [XField(1)]
        public long part1;

        [XField(2)]
        public long part2;

        [XField(3)]
        public long part3;

        public NickNamePacket() { }

        private NickNamePacket(long p1, long p2, long p3)
        {
            part1 = p1;
            part2 = p2;
            part3 = p3;
        }

        public static NickNamePacket EncodeString(string message)
        {
            if (message.Length > 18)
                throw new ArgumentException();

            var packet = new NickNamePacket(1, 1, 1);

            for (int i = 0; i < message.Length; i++)
            {
                if (i < 6)
                {
                    packet.part1 *= 1000;
                    packet.part1 += message[i];
                }
                else if (i < 13)
                {
                    packet.part2 *= 1000;
                    packet.part2 += message[i];
                }
                else
                {
                    packet.part3 *= 1000;
                    packet.part3 += message[i];
                }
            }

            return packet;
        }

        public string DecodeToString()
        {
            var sBuilder = new StringBuilder();
            var clone = new NickNamePacket(part1, part2, part3);
            while (true)
            {
                if (clone.part3 / 10 != 0)
                {
                    sBuilder.Append((char)(clone.part3 % 1000));
                    clone.part3 /= 1000;
                    continue;
                }
                if (clone.part2 / 10 != 0)
                {
                    sBuilder.Append((char)(clone.part2 % 1000));
                    clone.part2 /= 1000;
                    continue;
                }
                if (clone.part1 / 10 != 0)
                {
                    sBuilder.Append((char)(clone.part1 % 1000));
                    clone.part1 /= 1000;
                    continue;
                }
                break;
            }
            return new string(sBuilder.ToString().Reverse().ToArray());
        }
    }

    /*public class NickNamePacket
    {
        [XField(1)]
        public NickNames Nicknames;
    }

    public struct NickNames
    {
        public string[] Nicks;
    }*/
}