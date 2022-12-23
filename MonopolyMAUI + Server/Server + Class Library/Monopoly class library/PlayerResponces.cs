using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_class_library
{
    public interface IPlayerResponce
    {        
    }

    public class BuyHouseResponse
    {
        public readonly bool Response;
        public BuyHouseResponse(bool resp)
        {
            Response= resp;
        }
    }
}
