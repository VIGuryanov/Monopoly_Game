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

namespace MonopolyMAUI.Server
{
    internal static class ClientProcess
    {
        internal static XClient Client;
        internal static void RunClient()
        {
            //Console.Title = "XClient";
            //Console.ForegroundColor = ConsoleColor.White;

            Client = new XClient();
            DialogueMethods.client = Client;

            Client.OnPacketRecieve += OnPacketRecieve;
            Client.Connect("127.0.0.1", 4910);

            var rand = new Random();

            //Thread.Sleep(1000);

            //Console.WriteLine("Sending handshake packet..");

            Task.Run(() =>
            {
                while (true)
                {
                    Client.QueuePacketSend(XPacketConverter.Serialize(XPacketType.ClientSimpleRequest, new ClientSimpleRequest { ReqCode = (sbyte)ClientRequestCode.EndTurn }).ToPacket());
                    Thread.Sleep(5000);
                }
            });

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

            switch ((ServerRequestCode)req.ServerReqCode)
            {
                case ServerRequestCode.ApproveGame:
                    DialogueFields.ApproveGameRequested = true;
                    Client.QueuePacketSend(XPacketConverter.Serialize(
                        XPacketType.ClientSimpleResponce,
                        new ClientSimpleResponce()
                        {
                            ServerReqCode = (sbyte)ServerRequestCode.ApproveGame,
                            Responce = true
                        }).ToPacket());
                    break;
                case ServerRequestCode.CubesDouble:
                    Client.QueuePacketSend(XPacketConverter.Serialize(XPacketType.ClientSimpleResponce,
                        new ClientSimpleResponce()
                        {
                            ServerReqCode = (sbyte)ServerRequestCode.CubesDouble,
                            Responce = true
                        }).ToPacket());
                    break;
                case ServerRequestCode.LeaveJail:
                    Client.QueuePacketSend(XPacketConverter.Serialize(XPacketType.ClientSimpleResponce,
                        new ClientSimpleResponce()
                        {
                            ServerReqCode = (sbyte)ServerRequestCode.LeaveJail,
                            Responce = false
                        }).ToPacket());
                    break;
            }
        }

        private static void ProcessCubesThrow(XPacket packet)
        {
            var res = XPacketConverter.Deserialize<CubesThrowResult>(packet);

            PlayersList.Players[res.PlayerID].Move(res.FirstCube, res.SecondCube);
        }

        private static void ProcessUpdateStatus(XPacket packet)
        {
            var stat = XPacketConverter.Deserialize<UpdateClientStatus>(packet);

            while (Client.UserPlayerEntity == null)
                Thread.Sleep(50);

            PlayersList.Players[stat.PlayerID].PlayerEntity.Bankrupt = stat.Bankruptcy;
            PlayersList.Players[stat.PlayerID].PlayerEntity.InJail = stat.InJail;
            PlayersList.Players[stat.PlayerID].PlayerEntity.JailTurns = stat.JailTurns;
        }

        private static void ProcessUpdateBalance(XPacket packet)
        {
            var bal = XPacketConverter.Deserialize<UpdateClientBalance>(packet);

            while (Client.UserPlayerEntity == null)
                Thread.Sleep(50);

            PlayersList.Players[bal.PlayerID].UpdateMoney(bal.NewMoney);
            PlayersList.Players[bal.PlayerID].PlayerEntity.PrisonKeysCount = bal.NewPrisonKeysCount;
        }
    }
}
