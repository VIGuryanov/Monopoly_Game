using Monopoly_class_library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyORM;
using Monopoly_class_library.FieldCards;
using TCPServer;

namespace Monopoly_server.GameLogic
{
    public static class GameInfo
    {
        public static SquareGameField field;

        static GameInfo()
        {
            var quickNameAccess = EntityManager.userLandCards.ToDictionary(l => l.Name);
            field = new SquareGameField(new IGameFieldCard[] 
            {
                new GameFieldEmptyCard(),
                new GameFieldLandCard(quickNameAccess["Old Road"]),
                new GameFieldTreasuryCard(),
                new GameFieldLandCard(quickNameAccess["Main Highway"]),
                new GameFieldTaxCard(200),
                new GameFieldLandCard(quickNameAccess["West Seaport"]),
                new GameFieldLandCard(quickNameAccess["Aqua Park"]),
                new GameFieldChanceCard(),
                new GameFieldLandCard(quickNameAccess["City park"]),
                new GameFieldLandCard(quickNameAccess["Ski Resort"]),
                new GameFieldPrisonCard(),
                new GameFieldLandCard(quickNameAccess["Resident Area"]),
                new GameFieldLandCard(quickNameAccess["Electric Company"]),
                new GameFieldLandCard(quickNameAccess["Business Quarter"]),
                new GameFieldLandCard(quickNameAccess["Traiding Platform"]),
                new GameFieldLandCard(quickNameAccess["North Seaport"]),
                new GameFieldLandCard(quickNameAccess["Puskin Street"]),
                new GameFieldTreasuryCard(),
                new GameFieldLandCard(quickNameAccess["Prospect Mira"]),
                new GameFieldLandCard(quickNameAccess["Victory Avenue"]),
                new GameFieldEmptyCard(),
                new GameFieldLandCard(quickNameAccess["Bar"]),
                new GameFieldChanceCard(),
                new GameFieldLandCard(quickNameAccess["Night Club"]),
                new GameFieldLandCard(quickNameAccess["Restaurant"]),
                new GameFieldLandCard(quickNameAccess["East Seaport"]),
                new GameFieldLandCard(quickNameAccess["Computers"]),
                new GameFieldLandCard(quickNameAccess["Internet"]),
                new GameFieldLandCard(quickNameAccess["Water Supply Company"]),
                new GameFieldLandCard(quickNameAccess["Mobile Communication"]),
                new GameFieldGoPrisonCard(),
                new GameFieldLandCard(quickNameAccess["Sea Transportation"]),
                new GameFieldLandCard(quickNameAccess["Railway"]),
                new GameFieldTreasuryCard(),
                new GameFieldLandCard(quickNameAccess["Airline company"]),
                new GameFieldLandCard(quickNameAccess["South Seaport"]),
                new GameFieldChanceCard(),
                new GameFieldLandCard(quickNameAccess["Resort Zone"]),
                new GameFieldTaxCard(100),
                new GameFieldLandCard(quickNameAccess["Hotel Resort"])
            });
        }
    }
}
