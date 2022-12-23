using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_class_library.Lands
{
    public class LandSet
    {
        public string Name { get; }
        public LandType LandType { get; }
        public int? HousePrice { get; }
        public List<UserLandCard> SetLands { get; } = new();

        public LandSet(string name, LandType lType, int? hPrice)
        {
            Name = name;
            LandType = lType;
            HousePrice = hPrice;
        }

        public void AddLandToSet(UserLandCard land)
        {
            if(land.Set.Name != Name)
                throw new InvalidOperationException("Land already assigned to another set!");
            SetLands.Add(land);
        }
    }
}
