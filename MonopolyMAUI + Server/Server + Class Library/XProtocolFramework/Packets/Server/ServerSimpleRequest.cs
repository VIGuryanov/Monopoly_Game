using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProtocol.Serializator;

namespace XProtocol.Packets.Server
{
    public class ServerSimpleRequest
    {
        [XField(0)]
        public sbyte ServerReqCode;
    }
}
