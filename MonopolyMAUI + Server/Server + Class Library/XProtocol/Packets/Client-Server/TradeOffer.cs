using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProtocol.Serializator;

namespace XProtocol.Packets.Client
{
    public class TradeOffer
    {
        [XField(0)]
        public sbyte To;
        [XField(1)]
        public int GiveMoney;
        [XField(2)]
        public int TakeMoney;
        [XField(3)]
        public LandContent lands;
    }

    public struct LandContent
    {
        public int[] LandIndexesTake;
        public int[] LandIndexesGive;
    }
}
