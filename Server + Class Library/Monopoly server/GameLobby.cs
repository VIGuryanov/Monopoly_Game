using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TCPServer;
using XProtocol.Serializator;
using XProtocol;
using XProtocol.Packets.Client_Server;
using XProtocol.Packets.Server;

namespace Monopoly_server
{
    internal static class GameLobby
    {
        private static Queue<ConnectedClient> waitingClients = new();
        private static DateTime? GameSearchBegin = null;
        private static Task TeamBuilder = new Task(() => SearchGame());

        internal static void RegisterUser(ConnectedClient client)
        {
            client.GlobalStatus = ClientGlobalStatus.InLobby;//multiple requests no tested

            lock (waitingClients)
            {
                waitingClients.Enqueue(client);
                if (GameSearchBegin == null)
                    GameSearchBegin = DateTime.Now;
            }

            if (TeamBuilder.Status != TaskStatus.Running)
            {
                TeamBuilder = new Task(() => SearchGame());
                TeamBuilder.Start();
            }
        }

        internal static void SearchGame()
        {
            while (true)
                lock (waitingClients)
                    if (waitingClients.Count >= 4)
                    {
                        var playerToGame = new[] { waitingClients.Dequeue(), waitingClients.Dequeue(), waitingClients.Dequeue(), waitingClients.Dequeue() };

                        new Task(() =>
                        {
                            GameRoom gameRoom = new GameRoom(playerToGame.ToArray());
                        }).RunSynchronously();

                        if (waitingClients.Count == 0)
                        {
                            GameSearchBegin = null;
                            break;
                        }
                    }
                    else if (waitingClients.Count > 1 && DateTime.Now - GameSearchBegin > TimeSpan.FromMinutes(1))
                    {
                        var playerToGame = new List<ConnectedClient>();
                        var count = waitingClients.Count % 4;
                        for (int i = 0; i < count; i++)
                            playerToGame.Add(waitingClients.Dequeue());

                        new Task(() =>
                        {
                            GameRoom gameRoom = new GameRoom(playerToGame.ToArray());
                        }).RunSynchronously();

                        if (waitingClients.Count == 0)
                        {
                            GameSearchBegin = null;
                            break;
                        }
                    }
                    else
                        Thread.Sleep(10000);
        }
    }

    internal class GameRoom
    {
        ConnectedClient[] players;
        internal List<ConnectedClient> PlayersApprovedGame = new();
        internal List<ConnectedClient> PlayersRejectedGame = new();
        bool approveTimeout;

        internal GameRoom(ConnectedClient[] plrs)
        {
            players = plrs;
            BeginGame();
        }

        internal void BeginGame()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].Room = this;
                players[i].GlobalStatus = ClientGlobalStatus.InGame;
                for (int j = 0; j < players.Length; j++)
                    players[i].QueuePacketSend(XPacketConverter.Serialize(XPacketType.Nickname, NickNamePacket.EncodeString(players[j].Nick)).ToPacket());

                players[i].QueuePacketSend(XPacketConverter.Serialize(XPacketType.ServerSimpleRequest, new ServerSimpleRequest { ServerReqCode = (sbyte)ServerRequestCode.ApproveGame }).ToPacket());
            }

            if (!CheckApprove())
            {
                foreach (var player in players)
                {
                    player.QueuePacketSend(XPacketConverter.Serialize(
                            XPacketType.ServerNotification,
                           new ServerNotification()
                           {
                               NotificationCode =(sbyte)(approveTimeout ? ServerNotificationCode.GameApproveTimeout : ServerNotificationCode.OneRejectedGame)
                           })
                            .ToPacket());

                    if (PlayersRejectedGame.Contains(player) || !PlayersApprovedGame.Contains(player) && approveTimeout)
                        player.GlobalStatus = ClientGlobalStatus.Free;
                    else
                        GameLobby.RegisterUser(player);
                }
                Console.WriteLine("Bad game");
                return;
            }

            /*while(true)
            {
                for(int i=0;i<players.Length;i++)
                {
                    if (!players[i].PlyerEntity.Bankrupt)
                }
            }*/
            Console.WriteLine("Good game!");
        }

        internal bool CheckApprove()
        {
            var approveRequested = DateTime.Now;
            while (true)
            {
                if (PlayersRejectedGame.Count != 0)
                    return false;

                if (PlayersApprovedGame.Count == players.Length)
                    return true;

                if (DateTime.Now - approveRequested >= TimeSpan.FromSeconds(30))
                {
                    approveTimeout = true;
                    return false;
                }

                Thread.Sleep(2000);
            }
        }
    }
}
