using Monopoly_class_library;
using Monopoly_server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using XProtocol;
using XProtocol.Packets.Client;
using XProtocol.Packets.Client_Server;
using XProtocol.Packets.Server;
using XProtocol.Serializator;

namespace TCPServer
{
    internal class ConnectedClient
    {
        public Guid Id { get;} = new Guid();
        public Socket Client { get; }
        public Player PlyerEntity { get; } = new Player();
        public string Nick { get; private set; }
        internal GameRoom Room {get;set; }
        public ClientGlobalStatus GlobalStatus { get; set; } = ClientGlobalStatus.Free;
        public ClientGameStatus GameStatus { get;set;} = ClientGameStatus.None;

        private readonly Queue<byte[]> _packetSendingQueue = new Queue<byte[]>();

        public ConnectedClient(Socket client)
        {
            Client = client;

            Task.Run((Action)ProcessIncomingPackets);
            Task.Run((Action)SendPackets);
        }

        private void ProcessIncomingPackets()
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
                    if(GlobalStatus == ClientGlobalStatus.Free)
                        ProcessNickname(packet);
                    break;
                case XPacketType.ClientSimpleResponce:
                    ProcessSimpleResponce(packet);
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

            switch((ServerRequestCode)resp.ServerReqCode)
            {
                case ServerRequestCode.ApproveGame:
                    if(!Room.PlayersApprovedGame.Concat(Room.PlayersRejectedGame).Contains(this))
                        if(resp.Responce)
                            lock(Room.PlayersApprovedGame)
                                Room.PlayersApprovedGame.Add(this);
                        else
                            lock(Room.PlayersRejectedGame)
                                Room.PlayersRejectedGame.Add(this);
                    break;
            }

        }

        private void ProcessNickname(XPacket packet)
        {
            Nick = XPacketConverter.Deserialize<NickNamePacket>(packet).DecodeToString();
            GameLobby.RegisterUser(this);
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
    }
}
