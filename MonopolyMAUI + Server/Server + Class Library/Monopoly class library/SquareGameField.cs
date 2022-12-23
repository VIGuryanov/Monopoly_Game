using Monopoly_class_library.FieldCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_class_library
{
    public class SquareGameField
    {
        ///<summary>
        ///Order down-right angle -> clockwise
        ///</summary>>
        public IGameFieldCard[] FieldCards { get; }
        public int Width { get; }

        public SquareGameField(IGameFieldCard[] cards)
        {
            FieldCards = cards;
            Width = (FieldCards.Length / 4 + 1);
        }
    }
}
