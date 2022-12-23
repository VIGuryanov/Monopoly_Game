using Monopoly_class_library;
using Monopoly_class_library.Chance;
using Monopoly_class_library.Lands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPServer;
using XProtocol.Packets.Server;
using XProtocol.Serializator;
using XProtocol;

namespace Monopoly_server.GameLogic
{
    internal static class HandleIChance
    {
        internal static void Handle(IChance card, ConnectedClient player)
        {
            Handle((dynamic)card, player);
            player.UpdateBalance();
            player.UpdateStatus();
        }

        internal static void Handle(GetMoney card, ConnectedClient player)
        {
            player.PlayerEntity.GiveMoney(card.Amount);
        }

        internal static void Handle(PayMoney card, ConnectedClient player)
        {
            player.PlayerEntity.TakeAwayMoneyWithBankruptcy(card.Amount, null);
        }

        internal static void Handle(GetPrisonKey card, ConnectedClient player)
        {
            player.PlayerEntity.PrisonKeysCount++;
        }

        internal static void Handle(GoPrison card, ConnectedClient player)
        {
            player.QueuePacketSend(XPacketConverter.Serialize(XPacketType.CubesThrowResult, new CubesThrowResult { FirstCube = (byte)(GameInfo.JailLoc - player.PlayerEntity.FieldLocation), SecondCube = 0 }).ToPacket());
            player.PlayerEntity.GoJail(GameInfo.JailLoc);
        }

        internal static void Handle(GoField card, ConnectedClient player)
        {
            if (player.PlayerEntity.FieldLocation > card.targetIndex)
            {
                player.PlayerEntity.MovePlayer(GameInfo.Field.FieldCards.Length - player.PlayerEntity.FieldLocation + card.targetIndex, GameInfo.Field.FieldCards.Length);
                player.QueuePacketSend(XPacketConverter.Serialize(XPacketType.CubesThrowResult, new CubesThrowResult { FirstCube = (byte)(GameInfo.Field.FieldCards.Length - player.PlayerEntity.FieldLocation + card.targetIndex), SecondCube = 0 }).ToPacket());
            }
            else
            { 
                player.PlayerEntity.MovePlayer(card.targetIndex - player.PlayerEntity.FieldLocation, GameInfo.Field.FieldCards.Length);
                player.QueuePacketSend(XPacketConverter.Serialize(XPacketType.CubesThrowResult, new CubesThrowResult { FirstCube = (byte)(card.targetIndex - player.PlayerEntity.FieldLocation), SecondCube = 0 }).ToPacket());
            }
        }

        internal static void Handle(GoBack card, ConnectedClient player)
        {
            player.PlayerEntity.MovePlayer(-3, GameInfo.Field.FieldCards.Length);
        }

        internal static void Handle(PayHouses card, ConnectedClient player)
        {
            var pay = 0;
            foreach (var land in player.PlayerEntity.Lands)
            {
                if (land is UserSimpleLandCard sLand)
                {
                    if (sLand.HousesCount == 5)
                        pay += card.perHotel;
                    else
                        pay += sLand.HousesCount * card.perHouse;
                }
            }
            player.PlayerEntity.TakeAwayMoneyWithBankruptcy(pay, null);
        }
    }
}
