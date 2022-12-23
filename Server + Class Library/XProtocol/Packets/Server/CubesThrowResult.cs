using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProtocol.Serializator;

namespace XProtocol.Packets.Server
{
    public class CubesThrowResult
    {
        [XField(0)]
        public sbyte PlayerID;
        [XField(1)]
        public byte FirstCube;
        [XField(2)]
        public byte SecondCube;
    }
}
