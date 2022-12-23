using System.Linq;

namespace Monopoly_class_library.Lands
{
    public class UserLandCard
    {
        public Player? Owner { get; private set; } = null;
        public string Name { get; }
        public string RussianName { get; }
        public LandSet Set { get; }
        public int Price { get; }
        public int[] LandRent { get; }
        public bool Mortgaged { get; } = false;

        public UserLandCard(string name, string rusName, LandSet set, int price, params int[] rents)
        {
            Name = name;
            RussianName = rusName;
            Set = set;
            Price = price;
            LandRent = rents;
        }

        public ActionResult Buy(Player buyer)
        {
            if (buyer.Money < Price)
                return new ActionResult(ErrorMessages.MoneyNotEnough);

            if (!buyer.TakeAwayMoney(Price).IsSuccess)
                throw new Exception("Despite the filter the money not enough");

            Transfer(buyer);
            return new ActionResult();
        }

        public void Transfer(Player? to)
        {
            Owner = to;
            if(Owner != null)
                Owner.Lands.Add(this);
        }

        public ActionResult MortgageAndGetMoney()
        {
            if (Owner == null)
                return new ActionResult(ErrorMessages.LandOwnershipError);

            if (Mortgaged)
                return new ActionResult(ErrorMessages.AlreadyMortgaged);

            if (this is UserSimpleLandCard simpleLand && simpleLand.HousesCount != 0)
                return new ActionResult(ErrorMessages.HouseCountError);

            Owner.GiveMoney(Price / 2);
            return new ActionResult();
        }

        public ActionResult UnMortgageAndPayMoney()
        {
            if (Owner == null)
                return new ActionResult(ErrorMessages.LandOwnershipError);

            if (!Mortgaged)
                return new ActionResult(ErrorMessages.NoMortgaged);

            return Owner.TakeAwayMoney((int)(1.1 * Price / 2));
        }

        protected ActionResult GetRent(Player from, Func<int> rentLogic)
        {
            if (Owner == null)
                return new ActionResult(ErrorMessages.LandOwnershipError);

            var rentAmount = rentLogic();

            var tryTakeMoney = from.TakeAwayMoneyWithBankruptcy(rentAmount, Owner);
            if (!tryTakeMoney.IsSuccess)
                return tryTakeMoney;

            Owner.GiveMoney(rentAmount);
            return new ActionResult();
        }
    }

    public class UserSimpleLandCard : UserLandCard
    {
        public UserSimpleLandCard(string name, string rusName, LandSet set, int price, params int[] rents)
            : base(name, rusName, set, price, rents)
        {

        }

        public byte HousesCount { get; private set; }

        public ActionResult GetRent(Player from) =>
            GetRent(from, () =>
            {
                var rentAmount = LandRent[HousesCount];
                if (HousesCount == 0 && Set.SetLands.All(l => l.Owner == Owner && !l.Mortgaged))
                    rentAmount *= 2;
                return rentAmount;
            });

        public ActionResult BuyHouse()
        {
            if(Owner == null)
                throw new InvalidOperationException("No owner of the land");

            if (!Set.SetLands.All(l => l.Owner == Owner && !l.Mortgaged))
                return new ActionResult(ErrorMessages.SetOwnershipError);

            if (Set.SetLands.Any(l => (l as UserSimpleLandCard)!.HousesCount < HousesCount))
                return new ActionResult(ErrorMessages.HouseCountError);

            if (HousesCount == 5)
                return new ActionResult(ErrorMessages.HouseCountError);

            Owner.TakeAwayMoney((int)Set.HousePrice!);
            HousesCount++;

            return new ActionResult();
        }

        public ActionResult SellHouse()
        {
            if(Owner == null)
                throw new InvalidOperationException("No owner of the land");

            if (HousesCount == 0)
                return new ActionResult(ErrorMessages.HouseCountError);

            if (Set.SetLands.Any(l => (l as UserSimpleLandCard)!.HousesCount == HousesCount + 1))
                return new ActionResult(ErrorMessages.HouseCountError);

            Owner.GiveMoney((int)(Set.HousePrice! / 2));
            HousesCount--;

            return new ActionResult();
        }
    }

    public class UserSeaportLandCard : UserLandCard
    {
        public UserSeaportLandCard(string name, string rusName, LandSet set, int price, params int[] rents)
            : base(name, rusName, set, price, rents)
        {

        }

        public ActionResult GetRent(Player from) =>
            GetRent(from, () => LandRent[Set.SetLands.Where(l => l.Owner == Owner && !Mortgaged).Count()]);
    }

    public class UserCommunicationsLandCard : UserLandCard
    {
        public UserCommunicationsLandCard(string name, string rusName, LandSet set, int price)
            : base(name, rusName, set, price)
        {

        }

        public ActionResult GetRent(Player from, int cubePoints) =>
            GetRent(from, () =>
                Set.SetLands.Where(l => l.Owner == Owner && !Mortgaged).Count() switch
                {
                    1 => cubePoints * 4,
                    2 => cubePoints * 10,
                    _ => throw new ArgumentException($"Unsupported value {cubePoints} in communication land rent handler"),
                });
    }
}
