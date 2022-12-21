using MyORM;
using Monopoly_class_library.Lands;

namespace Monopoly_server.GameLogic
{
    public static class EntityManager
    {
        public static readonly UserLandCard[] userLandCards;
        public static readonly LandSet[] landSets;

        static EntityManager()
        {
            var orm = new MyORM.MyORM(@"(localdb)\MSSQLLocalDB", "MonopolyDB", true);
            landSets = orm.Select<LandSetsDb>().Select(l => l.ToLandSet()).ToArray();
            userLandCards = orm.Select<LandCardDB>().Select(l => l.ToUserLandCard(landSets)).ToArray();
        }
    }

    [DB_Table]
    public class LandCardDB
    {
        [DB_Field]
        public string Name { get; }

        [DB_Field]
        public string RussianName { get; }

        [DB_Field]
        public string Set { get; }

        [DB_Field]
        public int Price { get; }

        [DB_Field]
        public int? Rent0 { get; }

        [DB_Field]
        public int? Rent1 { get; }

        [DB_Field]
        public int? Rent2 { get; }

        [DB_Field]
        public int? Rent3 { get; }

        [DB_Field]
        public int? Rent4 { get; }

        [DB_Field]
        public int? Rent5 { get; }

        private LandCardDB(string name, string rName, string set, int price, int? r0, int? r1, int? r2, int? r3, int? r4, int? r5)
        {
            Name = name;
            RussianName = rName;
            Set = set;
            Price = price;
            Rent0 = r0;
            Rent1 = r1;
            Rent2 = r2;
            Rent3 = r3;
            Rent4 = r4;
            Rent5 = r5;
        }

        public UserLandCard ToUserLandCard(LandSet[] landSets)
        {
            var lSet = landSets.Where(s => s.Name == Set).First();

            UserLandCard userLand = lSet.LandType switch
            {
                LandType.Simple => new UserSimpleLandCard(Name, RussianName, lSet, Price,
                                                (int)Rent0!, (int)Rent1!, (int)Rent2!, (int)Rent3!, (int)Rent4!, (int)Rent5!),
                LandType.Seaport => new UserSeaportLandCard(Name, RussianName, lSet, Price,
                                                        (int)Rent0!, (int)Rent1!, (int)Rent2!, (int)Rent3!),
                LandType.Communication => new UserCommunicationsLandCard(Name, RussianName, lSet, Price),
                _ => throw new ArgumentException("Unsupported LandType enum"),
            };
            
            lSet.AddLandToSet(userLand);
            return userLand;
        }
    }

    [DB_Table]
    public class LandSetsDb
    {
        [DB_Field]
        public string Set { get; }

        [DB_Field]
        public int LandType { get; }

        [DB_Field]
        public int? HousePrice { get; }

        private LandSetsDb(string set, int lType, int? hPrice)
        {
            Set = set;
            LandType = lType;
            HousePrice = hPrice;
        }

        public LandSet ToLandSet()
        {
            var lType = (LandType)LandType;
            return new LandSet(Set, lType, HousePrice);
        }
    }
}
