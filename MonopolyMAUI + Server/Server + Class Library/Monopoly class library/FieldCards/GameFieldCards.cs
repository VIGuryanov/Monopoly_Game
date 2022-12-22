using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monopoly_class_library.Lands;

namespace Monopoly_class_library.FieldCards
{
    public class GameFieldLandCard : IGameFieldCard
    {
        public UserLandCard boundLandEntity { get; }

        public GameFieldLandCard(UserLandCard userLand)
        {
            boundLandEntity = userLand;
        }
    }

    public class GameFieldEmptyCard : IGameFieldCard
    {
    }

    public class GameFieldChanceCard : IGameFieldCard
    {
    }

    public class GameFieldTreasuryCard : IGameFieldCard
    {
    }

    public class GameFieldGoPrisonCard : IGameFieldCard
    {
    }

    public class GameFieldPrisonCard : IGameFieldCard
    {
    }

    public class GameFieldTaxCard : IGameFieldCard
    {
        public int TaxAmount { get; }

        public GameFieldTaxCard(int taxAmount)
        {
            TaxAmount = taxAmount;
        }
    }
}
