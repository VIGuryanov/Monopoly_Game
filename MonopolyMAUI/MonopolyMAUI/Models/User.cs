using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyMAUI.Models
{
    public class User
    {
        public string Nickname { get; set; }
        public int Id { get; set; }
        public Color Color { get; set; }
        public int Amount { get; set; }
    }
}
