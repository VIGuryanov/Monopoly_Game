using Monopoly_class_library;
using Monopoly_server;
using Monopoly_server.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using XProtocol;
using XProtocol.Packets.Client;
using XProtocol.Packets.Client_Server;
using XProtocol.Packets.Server;
using XProtocol.Serializator;
using Monopoly_class_library.FieldCards;
using System.Collections;

namespace TCPServer
{
    internal class ConnectedClient
    {
        public Guid Id { get; } = new Guid();
        public Socket Client { get; }
        public Player PlayerEntity { get; } = new Player();
        public string Nick { get; private set; }
        internal GameRoom Room { get; set; }
        internal sbyte RoomPlayerID { get; set; }

        internal Dialogue Dialogue { get; } = new Dialogue();
        public ClientGlobalStatus GlobalStatus { get; set; } = ClientGlobalStatus.Free;
        public ClientGameStatus GameStatus { get; set; } = ClientGameStatus.None;

        private readonly Queue<byte[]> _packetSendingQueue = new Queue<byte[]>();

        public ConnectedClient(Socket client)
        {
            Client = client;

            Task.Run((Action)ProcessIncomingPackets);
            Task.Run((Action)SendPackets);
        }

        private void ProcessIncomingPackets()
        {
            try
            {
                while (true) // Слушаем пакеты, пока клиент не отключится.
                {
                    var buff = new byte[256]; // Максимальный размер пакета - 256 байт.
                    Client.Receive(buff);

                    buff = buff.TakeWhile((b, i) =>
                    {
                        if (b != 0xFF) return true;
                        return buff[i + 1] != 0;
                    }).Concat(new byte[] { 0xFF, 0 }).ToArray();

                    var parsed = XPacket.Parse(buff);

                    if (parsed != null)
                    {
                        ProcessIncomingPacket(parsed);
                    }
                }
            }
            catch (Exception ex)
            {
                PlayerEntity.Bankrupt = true;
                UpdateStatus();
            }
        }

        private void ProcessIncomingPacket(XPacket packet)
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
                    if (GlobalStatus == ClientGlobalStatus.Free)
                        ProcessNickname(packet);
                    break;
                case XPacketType.ClientSimpleResponce:
                    ProcessSimpleResponce(packet);
                    break;
                case XPacketType.ClientSimpleRequest:
                    ProcessClientRequest(packet);
                    break;
                case XPacketType.TradeOffer:
                    ProcessTradeOffer(packet);
                    break;
                default:
                    Console.WriteLine($"Bad request from {Id} sent {type}");
                    break;
            }
        }

        private void ProcessHandshake(XPacket packet)
        {
            Console.WriteLine("Recieved handshake packet.");

            var handshake = XPacketConverter.Deserialize<XPacketHandshake>(packet);
            handshake.MagicHandshakeNumber -= 15;

            Console.WriteLine("Answering..");

            QueuePacketSend(XPacketConverter.Serialize(XPacketType.Handshake, handshake).ToPacket());
        }

        private void ProcessSimpleResponce(XPacket packet)
        {
            var resp = XPacketConverter.Deserialize<ClientSimpleResponce>(packet);

            switch ((ServerRequestCode)resp.ServerReqCode)
            {
                case ServerRequestCode.ApproveGame:
                    lock (Room.PlayersApprovedGame)
                        if (GameStatus == ClientGameStatus.None)//not accept packet if client in game
                            if (!Room.PlayersApprovedGame.Concat(Room.PlayersRejectedGame).Contains(this))
                                if (resp.Responce)
                                    lock (Room.PlayersApprovedGame)
                                        Room.PlayersApprovedGame.Add(this);
                                else
                                    lock (Room.PlayersRejectedGame)
                                        Room.PlayersRejectedGame.Add(this);
                    break;
                case ServerRequestCode.CubesDouble:
                    Dialogue.CubesDoubleRethrowApprove = resp.Responce;
                    break;
                case ServerRequestCode.LeaveJail:
                    Dialogue.ApprovedJailLeave = resp.Responce;
                    break;
            }

        }

        private void ProcessNickname(XPacket packet)
        {
            Nick = XPacketConverter.Deserialize<NickNamePacket>(packet).DecodeToString();
            //Nick = XPacketConverter.Deserialize<NickNamePacket>(packet).Nicknames.Nicks.FirstOrDefault();
            //if(Nick != null)
            GameLobby.RegisterUser(this);
        }

        void ProcessClientRequest(XPacket packet)
        {
            var req = XPacketConverter.Deserialize<ClientSimpleRequest>(packet);

            switch ((ClientRequestCode)req.ReqCode)
            {
                case ClientRequestCode.BuyLand:
                    if (GameStatus == ClientGameStatus.MakesTurn && GameInfo.Field.FieldCards[PlayerEntity.FieldLocation] is GameFieldLandCard land)
                    {
                        var res = land.boundLandEntity.Buy(PlayerEntity);

                    }
                    break;
                case ClientRequestCode.EndTurn:
                    if (GameStatus == ClientGameStatus.MakesTurn)
                        Dialogue.EndedTurn = true;
                    break;
                case ClientRequestCode.Pause:
                    Room.Paused = !Room.Paused; 
                    break;
            }
        }

        private void ProcessTradeOffer(XPacket packet)
        {
            /*var off = XPacketConverter.Deserialize<TradeOffer>(packet);
            
            if(Room.players.Length >= off.To && off.GiveMoney <= PlayerEntity.Money && off.TakeMoney >= Room.players[off.To].PlayerEntity.Money)
            {
                if(off.lands.LandIndexesGive.All(x=>x <= GameInfo.Field.FieldCards.Length) &&
                    off.lands.LandIndexesTake.All(x=>x <= GameInfo.Field.FieldCards.Length))
                {
                    var giveEntities = off.lands.LandIndexesGive.Select(x => GameInfo.Field.FieldCards[x]);
                    var takeEntities = off.lands.LandIndexesTake.Select(x => GameInfo.Field.FieldCards[x]);
                    if(giveEntities.All(x => x is GameFieldLandCard) && takeEntities.All(x => x is GameFieldLandCard))
                        Room.players[off.To].QueuePacketSend(XPacketConverter.Serialize<TradeOffer>(XPacketType.TradeOffer, off).ToPacket());
                }
            }*/
        }

        public void QueuePacketSend(byte[] packet)
        {
            if (packet.Length > 256)
            {
                throw new Exception("Max packet size is 256 bytes.");
            }

            _packetSendingQueue.Enqueue(packet);
        }

        private void SendPackets()
        {
            try
            {
                /*if(Room.Paused)
                {
                    QueuePacketSend(XPacketConverter.Serialize(new XPacket(XPacketType.ClientSimpleRequest, new ClientSimpleRequest { ReqCode = h}));
                }*/

                while (true)
                {
                    if (_packetSendingQueue.Count == 0)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    var packet = _packetSendingQueue.Dequeue();
                    Client.Send(packet);

                    Thread.Sleep(100);
                }
            }
            catch(Exception e)
            { }
        }

        internal void UpdateStatus()
        {
            foreach (var player in Room.players)
                player.QueuePacketSend(XPacketConverter.Serialize(XPacketType.UpdateStatus, new UpdateClientStatus
                {
                    PlayerID = RoomPlayerID,
                    InJail = PlayerEntity.InJail,
                    JailTurns = PlayerEntity.JailTurns,
                    Bankruptcy = PlayerEntity.Bankrupt
                }).ToPacket());
        }

        internal void UpdateBalance()
        {
            foreach (var player in Room.players)
                player.QueuePacketSend(XPacketConverter.Serialize(XPacketType.UpdateBalance, new UpdateClientBalance
                {
                    PlayerID = RoomPlayerID,
                    NewMoney = PlayerEntity.Money,
                    NewPrisonKeysCount = PlayerEntity.PrisonKeysCount,
                }).ToPacket());
        }

        internal void SendCubes(int cubeRes1, int cubeRes2)
        {
            foreach (var player in Room.players)
                player.QueuePacketSend(XPacketConverter.Serialize(XPacketType.CubesThrowResult, new CubesThrowResult { PlayerID = RoomPlayerID, FirstCube = (byte)cubeRes1, SecondCube = (byte)cubeRes2 }).ToPacket());
            Console.WriteLine(Nick +"  "+ cubeRes1 +"  "+ cubeRes2);
        }
    }
}
