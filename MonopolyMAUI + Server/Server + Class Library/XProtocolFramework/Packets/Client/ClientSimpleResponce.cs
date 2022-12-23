using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProtocol.Serializator;

namespace XProtocol.Packets.Client
{
    public class ClientSimpleResponce
    {
        [XField(0)]
        public sbyte ServerReqCode;
        [XField(1)]
        public bool Responce;
    }
}
