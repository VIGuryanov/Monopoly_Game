using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_class_library.Chance
{
    public interface IChance
    {

    }

    public class GetMoney : IChance
    {
        public int Amount;
    }

    public class PayMoney:IChance
    { 
        public int Amount;
    }

    public class GetPrisonKey:IChance
    {

    }

    public class GoPrison:IChance
    {

    }

    public class GoField:IChance
    {
        public int targetIndex;
    }

    public class GoBack:IChance
    {
        public int FieldCount;
    }

    public class PayHouses:IChance
    { 
        public int perHouse;
        public int perHotel;
    }
}
