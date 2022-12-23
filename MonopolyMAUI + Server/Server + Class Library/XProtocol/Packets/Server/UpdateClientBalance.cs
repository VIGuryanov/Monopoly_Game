using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProtocol.Serializator;

namespace XProtocol.Packets.Server
{
    public class UpdateClientBalance
    {
        [XField(0)]
        public int NewMoney;
        [XField(1)]
        public int NewPrisonKeysCount;
    }
}
