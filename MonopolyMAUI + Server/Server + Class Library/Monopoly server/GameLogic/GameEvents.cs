using Monopoly_class_library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TCPServer;
using XProtocol.Packets.Server;
using XProtocol.Serializator;
using XProtocol;

namespace Monopoly_server.GameLogic
{
    internal class GameEvents
    {
        private static readonly Random rnd = new();
        internal static void MovePlayer(ConnectedClient player)
        {
            for (int i = 0; i < 2; i++)
            {
                var cubeRes1 = rnd.Next(1, 6);
                var cubeRes2 = rnd.Next(1, 6);
                player.PlayerEntity.MovePlayer(cubeRes1 + cubeRes2, GameInfo.Field.FieldCards.Length);
                player.QueuePacketSend(XPacketConverter.Serialize(XPacketType.CubesThrowResult, new CubesThrowResult { FirstCube = (byte)cubeRes1, SecondCube = (byte)cubeRes2 }).ToPacket());
                player.UpdateStatus();
                if (cubeRes1 != cubeRes2)
                    return;
                else
                {
                    player.QueuePacketSend(XPacketConverter.Serialize(XPacketType.ServerSimpleRequest, new ServerSimpleRequest { ServerReqCode = (sbyte)ServerRequestCode.CubesDouble }).ToPacket());
                    while (player.Dialogue.CubesDoubleRethrowApprove == null)
                        Thread.Sleep(500);
                    if ((bool)player.Dialogue.CubesDoubleRethrowApprove)
                        continue;
                    player.Dialogue.CubesDoubleRethrowApprove = null;
                    return;
                }
            }
            player.PlayerEntity.GoJail(GameInfo.JailLoc);
            player.UpdateStatus();
        }
    }
}
