using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProtocol;
using XProtocol.Serializator;
using XProtocol.Packets.Client;
using XProtocol.Packets.Client_Server;
using XProtocol.Packets.Server;
using Monopoly_class_library;

namespace MonopolyMAUI.Server
{
    internal static class ClientProcess
    {
        static XClient client;
        internal static void RunClient()
        {
            //Console.Title = "XClient";
            //Console.ForegroundColor = ConsoleColor.White;

            client = new XClient();
            DialogueMethods.client = client;

            client.OnPacketRecieve += OnPacketRecieve;
            client.Connect("127.0.0.1", 4910);

            var rand = new Random();

            //Thread.Sleep(1000);

            //Console.WriteLine("Sending handshake packet..");

            /*client.QueuePacketSend(
                XPacketConverter.Serialize(
                    XPacketType.Nickname,
                    NickNamePacket.EncodeString("clientsentюЪЁ"))
                    .ToPacket());*/

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
                case XPacketType.Unknown:
                    break;
                case XPacketType.Nickname:
                    ProcessNickNames(packet);
                    break;
                case XPacketType.ServerSimpleRequest:
                    ProcessSimpleRequest(packet);
                    break;
                case XPacketType.CubesThrowResult:
                    ProcessCubesThrow(packet);
                    break;
                case XPacketType.UpdateStatus:
                    ProcessUpdateStatus(packet);
                    break;
                case XPacketType.UpdateBalance:
                    ProcessUpdateBalance(packet);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void ProcessNickNames(XPacket packet)
        {
            var nick = XPacketConverter.Deserialize<NickNamePacket>(packet);

            DialogueFields.PlayersNicks.Add(nick.DecodeToString());
        }

        private static void ProcessSimpleRequest(XPacket packet)
        {
            var req = XPacketConverter.Deserialize<ServerSimpleRequest>(packet);

            switch((ServerRequestCode)req.ServerReqCode)
            {
                case ServerRequestCode.ApproveGame:
                    DialogueFields.ApproveGameRequested = true;
                    client.QueuePacketSend(XPacketConverter.Serialize(
                        XPacketType.ClientSimpleResponce,
                        new ClientSimpleResponce()
                        {
                            ServerReqCode = (sbyte)ServerRequestCode.ApproveGame,
                            Responce = true
                        }).ToPacket());
                    break;
            }
        }

        private static void ProcessCubesThrow(XPacket packet)
        {
            var res = XPacketConverter.Deserialize<CubesThrowResult>(packet);

        }

        private static void ProcessUpdateStatus(XPacket packet)
        {
            var stat = XPacketConverter.Deserialize<UpdateClientStatus>(packet);

            client.PlayerEntity.Bankrupt = stat.Bankruptcy;
            client.PlayerEntity.InJail = stat.InJail;
            client.PlayerEntity.JailTurns = stat.JailTurns;
            client.PlayerEntity.FieldLocation = stat.FieldPos;
        }

        private static void ProcessUpdateBalance(XPacket packet)
        {
            var bal = XPacketConverter.Deserialize<UpdateClientBalance>(packet);

            client.PlayerEntity.Money = bal.NewMoney;
            client.PlayerEntity.PrisonKeysCount = bal.NewPrisonKeysCount;
        }
    }
}
