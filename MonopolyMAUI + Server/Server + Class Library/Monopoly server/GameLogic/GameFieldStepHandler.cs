using Monopoly_class_library;
using Monopoly_class_library.FieldCards;
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
    internal static class GameFieldStepHandler
    {
        internal static void HandleStep(ConnectedClient player)
        {
            HandleStep((dynamic)GameInfo.Field.FieldCards[player.PlayerEntity.FieldLocation], player);
        }

        static void HandleStep(GameFieldEmptyCard fieldCard, ConnectedClient player)
        {

        }

        static void HandleStep(GameFieldChanceCard fieldCard, ConnectedClient player)
        {

        }

        static void HandleStep(GameFieldTreasuryCard fieldCard, ConnectedClient player)
        {

        }

        static void HandleStep(GameFieldGoPrisonCard fieldCard, ConnectedClient player)
        {
            player.PlayerEntity.GoJail(GameInfo.JailLoc);
            player.QueuePacketSend(XPacketConverter.Serialize(XPacketType.CubesThrowResult, new CubesThrowResult { FirstCube = (byte)(GameInfo.JailLoc - player.PlayerEntity.FieldLocation), SecondCube = 0 }).ToPacket());
            player.UpdateStatus();
        }

        static void HandleStep(GameFieldPrisonCard fieldCard, ConnectedClient player)
        {

        }

        static void HandleStep(GameFieldLandCard fieldCard, ConnectedClient player)
        {
            if(fieldCard.boundLandEntity.Owner != null)
            {
                ActionResult res = ((dynamic)fieldCard.boundLandEntity).GetRent(player);
                if(!res.IsSuccess)
                {
                    player.UpdateBalance();
                    player.UpdateStatus();         
                }
            }
            /*if(fieldCard.boundLandEntity.Owner == null)
                server.SendMessageAsync(Protocolization("buy house?"), );*/
        }

        static void HandleStep(GameFieldTaxCard fieldCard, ConnectedClient player)
        {
            player.PlayerEntity.TakeAwayMoneyWithBankruptcy(fieldCard.TaxAmount, null);
            player.UpdateBalance();
            player.UpdateStatus();  
        }
    }
}
