using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonopolyMAUI.Models;
using Monopoly_class_library;
using MonopolyMAUI.ViewModel;
using MonopolyMAUI.View;
using Microsoft.Maui.Controls;
using MonopolyMAUI.Graphics;

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
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var stack = GamePage.Instance.FindByName<VerticalStackLayout>($"Players_{PlayerEntity.FieldLocation}");

                foreach (var views in stack)
                {
                    if (views is GraphicsView graphics && graphics.StyleId == $"Player_{UserEntity.Nickname}")
                    {
                        stack.Remove(views);
                        break;
                    }
                }

                var moneyMem = PlayerEntity.Money;
                PlayerEntity.MovePlayer(cube1 + cube2, 40);
                PlayerEntity.Money = moneyMem;
                //TODO вызов анимации движения
                stack = GamePage.Instance.FindByName<VerticalStackLayout>($"Players_{PlayerEntity.FieldLocation}");
                stack.Add(new GraphicsView()
                {
                    StyleId = $"Player_{UserEntity.Nickname}",
                    Drawable = new PlayerPoint(UserEntity.Color),
                    HeightRequest = 12,
                });
            });
        }
    }
}
