using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProtocol.Serializator;

namespace XProtocol.Packets.Server
{
    public class UpdateClientStatus
    {
        [XField(0)]
        public sbyte PlayerID;
        [XField(1)]
        public bool InJail;
        [XField(2)]
        public int JailTurns;
        [XField(3)]
        public bool Bankruptcy;
    }
}
