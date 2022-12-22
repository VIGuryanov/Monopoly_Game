using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProtocol.Serializator;

namespace XProtocol.Packets.Client
{
    public class ClientSimpleRequest
    {
        [XField(0)]
        public sbyte ReqCode;
    }
}
