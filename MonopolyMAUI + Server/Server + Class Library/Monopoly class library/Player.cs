using Monopoly_class_library.Lands;
using System.Data.SqlTypes;

namespace Monopoly_class_library
{
    public class Player
    {
        public int Money { get; set; } = 1500;
        public List<UserLandCard> Lands { get; } = new();
        public int PrisonKeysCount { get; set; } = 0;

        public void MovePlayer(int moveLength, int gameFieldLength)
        {
            if (moveLength + FieldLocation >= gameFieldLength - 1)
                Money += 200;
            FieldLocation = (FieldLocation + moveLength) % gameFieldLength;
        }

        public int FieldLocation { get; set; }

        public bool Bankrupt { get; set;} = false;
        public (int, List<UserLandCard>, int) BankruptSnapshot { get; private set; }
        public Player? OweTo { get; private set; }

        public bool InJail { get; set; } = false;
        public int JailTurns { get; set; } = 0;
        public void GoJail(int jailLoc)
        {
            FieldLocation = jailLoc;
            InJail = true;
            JailTurns = 3;
        }

        public void LeaveJail()
        {
            if (JailTurns != 0)
                if (PrisonKeysCount == 0)
                    if (Money < 50)
                        return;
                    else
                        Money -= 50;
                else
                    PrisonKeysCount -= 1;
            InJail = false;
            JailTurns = 0;
        }

        public void GiveMoney(int value)
        {
            Money += value;
        }

        public ActionResult TakeAwayMoney(int value)
        {
            if (Money - value < 0)
                return new ActionResult(ErrorMessages.MoneyNotEnough);
            Money -= value;
            return new ActionResult();
        }

        public ActionResult TakeAwayMoneyWithBankruptcy(int value, Player? player)
        {
            Money -= value;

            if (Money < 0)
            {
                BankruptSnapshot = (Money, Lands, PrisonKeysCount);
                OweTo = player;
                return new ActionResult(ErrorMessages.Bankruptcy);
            }
            return new ActionResult();
        }

        public void TransferPropertyBankrupt()
        {
            if (OweTo == null)
            {
                Money = 0;
                PrisonKeysCount = 0;
            }
            else
            {
                OweTo.GiveMoney(BankruptSnapshot.Item1);
                OweTo.PrisonKeysCount += BankruptSnapshot.Item3;
            }
            foreach (var land in BankruptSnapshot.Item2)
                land.Transfer(null);
        }
    }
}