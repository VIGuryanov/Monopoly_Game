using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyMAUI.Models
{
    public partial class User : ObservableObject
    {
        [ObservableProperty]
        public string nickname;

        [ObservableProperty]
        public int id;

        [ObservableProperty]
        public Color color;

        [ObservableProperty]
        public int amount;
    }
}
