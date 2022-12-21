using System;
using System.Threading;
using XProtocol;
using XProtocol.Serializator;
using XProtocol.Packets.Client_Server;
using System.Net.Sockets;
using XProtocol.Packets.Client;
using XProtocol.Packets.Server;

namespace TCPClient
{
    internal class Program
    {
        private static int _handshakeMagic;
        private static XClient client;

        private static void Main()
        {
            Console.Title = "XClient";
            Console.ForegroundColor = ConsoleColor.White;

            client = new XClient();
            client.OnPacketRecieve += OnPacketRecieve;
            client.Connect("127.0.0.1", 4910);

            var rand = new Random();
            _handshakeMagic = rand.Next();

            Thread.Sleep(1000);

            Console.WriteLine("Sending handshake packet..");

            client.QueuePacketSend(
                XPacketConverter.Serialize(
                    XPacketType.Nickname,
                    NickNamePacket.EncodeString(Console.ReadLine()))
                    .ToPacket());

            while (true) { }
        }

        private static void OnPacketRecieve(byte[] packet)
        {
            var parsed = XPacket.Parse(packet);

            if (parsed != null)
            {
                ProcessIncomingPacket(parsed);
            }
        }

        private static void ProcessIncomingPacket(XPacket packet)
        {
            var type = XPacketTypeManager.GetTypeFromPacket(packet);

            switch (type)
            {
                case XPacketType.Handshake:
                    ProcessHandshake(packet);
                    break;
                case XPacketType.Unknown:
                    break;
                case XPacketType.Nickname:
                    ProcessNickname(packet);
                    break;
                case XPacketType.ServerSimpleRequest:
                    ProcessSimpleRequest(packet);
                    break;
                case XPacketType.ServerNotification:
                    ProcessServerNotification(packet);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void ProcessHandshake(XPacket packet)
        {
            var handshake = XPacketConverter.Deserialize<XPacketHandshake>(packet);

            if (_handshakeMagic - handshake.MagicHandshakeNumber == 15)
            {
                Console.WriteLine("Handshake successful!");
            }
        }

        private static void ProcessServerNotification(XPacket packet)
        {
            var notification = XPacketConverter.Deserialize<ServerNotification>(packet);

            switch (notification.NotificationCode)
            {
                case 0:Console.WriteLine("One player rejected game"); break;
                case 1: Console.WriteLine("Game approve timeout"); break;
                default: Console.WriteLine("Unknown code"); break;
            };
        }

        private static void ProcessSimpleRequest(XPacket packet)
        {
            var resp = XPacketConverter.Deserialize<ServerSimpleRequest>(packet);

            switch ((ServerRequestCode)resp.ServerReqCode)
            {
                case ServerRequestCode.ApproveGame:
                    Console.WriteLine("PLease approve game - write true/false");
                    var res = bool.TryParse(Console.ReadLine(), out bool userReq);
                    if (res == false)
                    {
                        Console.WriteLine("Bad input. One attempt left");
                        res = bool.TryParse(Console.ReadLine(), out userReq);
                    }
                    if (res == false)
                        userReq = false;
                    client.QueuePacketSend(XPacketConverter.Serialize(
                        XPacketType.ClientSimpleResponce,
                        new ClientSimpleResponce()
                        {
                            ServerReqCode = (sbyte)ServerRequestCode.ApproveGame,
                            Responce = userReq
                        }).ToPacket());
                    break;
            }
        }

        private static void ProcessNickname(XPacket packet)
        {
            Console.WriteLine("Resp: " + XPacketConverter.Deserialize<NickNamePacket>(packet).DecodeToString());
        }
    }
}
