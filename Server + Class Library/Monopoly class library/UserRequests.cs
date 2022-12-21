using Monopoly_class_library.FieldCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_class_library
{
    public static class UserRequests
    {
        public static ActionResult BuyHouse(SquareGameField field, Player player)
        {
            if(field.FieldCards[player.FieldLocation] is GameFieldLandCard landField && landField.boundLandEntity.Owner == null)
                return landField.boundLandEntity.Buy(player);
            return new ActionResult("Invalid operation for current field");
        }
    }
}
