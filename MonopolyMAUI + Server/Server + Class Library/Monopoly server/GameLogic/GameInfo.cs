using Monopoly_class_library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyORM;
using Monopoly_class_library.FieldCards;
using TCPServer;
using Monopoly_class_library.Chance;

namespace Monopoly_server.GameLogic
{
    public static class GameInfo
    {
        public static readonly SquareGameField Field;
        public static readonly int JailLoc;

        static IChance[] Chance;
        static int ChanceIndex = 0;
        static IChance[] Treasure;
        static int TreasureIndex = 0;

        static GameInfo()
        {
            var quickNameAccess = EntityManager.userLandCards.ToDictionary(l => l.Name);
            JailLoc = 10;
            Field = new SquareGameField(new IGameFieldCard[]
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

            Chance = new IChance[]
            {
                new GoField(){targetIndex = 39 },
                new GoField(){ targetIndex=0},
                new GetPrisonKey(),
                new PayHouses{ perHouse = 40, perHotel = 115},
                new GoField(){ targetIndex = 24},
                new PayMoney(){ Amount = 15},
                new GetMoney(){ Amount=150},
                new GoPrison(),
                new PayHouses{ perHouse=25, perHotel = 100},
                new GoField(){ targetIndex = 15},
                new GetMoney(){ Amount=50},
                new PayMoney(){ Amount=20},
                new PayMoney(){ Amount=150},
                new GetMoney(){ Amount=100},
                new GoField(){ targetIndex = 6},
                new GoBack(){ FieldCount = 3}
            };

            Treasure = new IChance[]
            {
                new GetMoney(){ Amount=200},
                new GetMoney{ Amount=25},
                new GetPrisonKey(),
                new GetMoney{ Amount=25},
                new GetMoney{ Amount=100},
                new GetMoney{ Amount=25},
                new GoPrison(),
                new PayMoney(){ Amount=50},
                new GetMoney{ Amount=10},
                new GetMoney{ Amount=50},
                new PayMoney{ Amount=100},
                new GoField{targetIndex = 1 },
                new GetMoney{ Amount=100},
                new PayMoney(){ Amount=10},
                new PayMoney(){ Amount=50}
            };

            Shuffle(Chance);
            Shuffle(Treasure);
        }

        public static IChance GetChance()
        {
            if (Chance.Length == ChanceIndex)
            {
                ChanceIndex = 0;
                Shuffle(Chance);
            }
            return Chance[ChanceIndex];
        }

        public static IChance GetTreasure()
        {
            if (Treasure.Length == TreasureIndex)
            {
                TreasureIndex = 0;
                Shuffle(Treasure);
            }
            return Treasure[TreasureIndex];
        }

        public static void Shuffle(IChance[] arr)
        {
            // создаем экземпляр класса Random для генерирования случайных чисел
            Random rand = new Random();

            for (int i = arr.Length - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);

                var tmp = arr[j];
                arr[j] = arr[i];
                arr[i] = tmp;
            }
        }
    }
}
