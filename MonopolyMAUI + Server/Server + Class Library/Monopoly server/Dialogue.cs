using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_server
{
    internal class Dialogue
    {
        internal bool? CubesDoubleRethrowApprove { get; set; } = null;
        internal bool? ApprovedJailLeave { get; set; } = null;
        internal bool EndedTurn { get; set; } = false;
    }
}
