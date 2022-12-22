using System;
using System.Collections.Generic;

namespace XProtocol
{
    public static class XPacketTypeManager
    {
        private static readonly Dictionary<XPacketType, Tuple<byte, byte>> TypeDictionary =
            new Dictionary<XPacketType, Tuple<byte, byte>>();

        static XPacketTypeManager()
        {
            RegisterType(XPacketType.Handshake, 1, 0);
            RegisterType(XPacketType.Nickname, 2, 0);
            RegisterType(XPacketType.ServerSimpleRequest, 3, 0);
            RegisterType(XPacketType.ClientSimpleResponce, 4, 0);
            RegisterType(XPacketType.ServerNotification, 5, 0);
            RegisterType(XPacketType.UpdateBalance, 6, 0);
            RegisterType(XPacketType.UpdateStatus, 7, 0);
            RegisterType(XPacketType.ClientSimpleRequest, 8, 0);
            RegisterType(XPacketType.CubesThrowResult, 9, 0);
            RegisterType(XPacketType.TradeOffer, 10, 0);
        }

        public static void RegisterType(XPacketType type, byte btype, byte bsubtype)
        {
            if (TypeDictionary.ContainsKey(type))
            {
                throw new Exception($"Packet type {type:G} is already registered.");
            }

            TypeDictionary.Add(type, Tuple.Create(btype, bsubtype));
        }

        public static Tuple<byte, byte> GetType(XPacketType type)
        {
            if (!TypeDictionary.ContainsKey(type))
            {
                throw new Exception($"Packet type {type:G} is not registered.");
            }

            return TypeDictionary[type];
        }

        public static XPacketType GetTypeFromPacket(XPacket packet)
        {
            var type = packet.PacketType;
            var subtype = packet.PacketSubtype;

            foreach (var tuple in TypeDictionary)
            {
                var value = tuple.Value;

                if (value.Item1 == type && value.Item2 == subtype)
                {
                    return tuple.Key;
                }
            }

            return XPacketType.Unknown;
        }
    }
}
