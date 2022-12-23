using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonopolyMAUI.Models;
using Monopoly_class_library;

namespace MonopolyMAUI.Server
{
    internal class User_Player
    {
        internal User UserEntity;
        internal Player PlayerEntity;

        internal void UpdateMoney(int value)
        {
            UserEntity.Amount = value;
            PlayerEntity.Money = value;
        }

        internal void Move(int cube1, int cube2)
        {
            PlayerEntity.FieldLocation += cube1 + cube2;
            //TODO вызов анимации движения
        }
    }
}
