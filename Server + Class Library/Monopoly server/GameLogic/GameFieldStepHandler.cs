using Monopoly_class_library.FieldCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPServer;

namespace Monopoly_server.GameLogic
{
    internal static class GameFieldStepHandler
    {
        internal static async Task HandleStep(IGameFieldCard fieldCard, ConnectedClient player)
        {
            await HandleStep((dynamic)fieldCard, player);
        }

        internal static async Task HandleStep(GameFieldEmptyCard fieldCard, ConnectedClient player)
        {

        }

        internal static async Task HandleStep(GameFieldChanceCard fieldCard, ConnectedClient player)
        {

        }

        internal static async Task HandleStep(GameFieldTreasuryCard fieldCard, ConnectedClient player)
        {

        }

        internal static async Task HandleStep(GameFieldGoPrisonCard fieldCard, ConnectedClient player)
        {

        }

        internal static async Task HandleStep(GameFieldPrisonCard fieldCard, ConnectedClient player)
        {

        }

        internal static async Task HandleStep(GameFieldLandCard fieldCard, ConnectedClient player)
        {
            /*if(fieldCard.boundLandEntity.Owner == null)
                server.SendMessageAsync(Protocolization("buy house?"), );*/
        }

        internal static async Task HandleStep(GameFieldTaxCard fieldCard, ConnectedClient player)
        {

        }
    }
}
