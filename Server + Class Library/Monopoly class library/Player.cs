using Monopoly_class_library.Lands;

namespace Monopoly_class_library
{
    public class Player
    {
        public int Money { get; private set; } = 1500;
        public List<UserLandCard> Lands { get; } = new();
        public int PrisonKeysCount { get; } = 0;
        public int FieldLocation { get; } = 0;
        public bool Bankrupt { get; } = false;

        public void GiveMoney(int value)
        {
            Money += value;
        }

        public ActionResult TakeAwayMoney(int value)
        {
            if (Money - value < 0)
                return new ActionResult("Not enough money");
            Money -= value;
            return new ActionResult();
        }

        public ActionResult TakeAwayMoneyWithBankruptcy(int value)
        {
            if (Money - value < 0)
            {
                throw new NotImplementedException("No bankruptcy");
                return new ActionResult("Not enough money");
            }
            Money -= value;
            return new ActionResult();
        }

        public void TransferAllProperty(Player? to)
        {
            throw new NotImplementedException();
        }
    }
}