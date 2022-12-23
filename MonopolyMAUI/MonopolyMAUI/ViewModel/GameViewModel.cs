using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MonopolyMAUI.View;

namespace MonopolyMAUI.ViewModel;

[QueryProperty("Players", "Players")]
public partial class GameViewModel : StartGameViewModel
{
    /*[RelayCommand]
    async Task TapCommand(string name)
    {

        Shell.Current.
    }*/

    /*Navigation.PushAsync для того, чтобы положить страницу в стек - для выхода в менюшку
      и PopAsync для возвращения
     */

    [RelayCommand]
    async Task TappedRightRecognizer(string idParam)
    {
        var id = int.Parse(idParam);
        /*проверка на принадлежность участка пользователю и наличие всех участков данной группы в собственности*/
        
        if (false/*нужное условие*/)
            return;
        
        var result = await Shell.Current.DisplayAlert("Действие с домом", null, "Построить", "Снести");
        var buildings = Shell.Current.CurrentPage.FindByName<HorizontalStackLayout>($"Buildings_{id}");
        if (result)
        {
            /* логика с сервером на добавление дома и увеличение ренты*/
            if (buildings != null)
            {
                if(buildings.Count == 5)
                {
                    await Shell.Current.DisplayAlert("Ошибка", "Вы не можете построить больше домов!", "OK");
                }
                if (buildings.Count == 4)
                    buildings.Add(new Image
                    {
                        Source="hotel.png", 
                        HeightRequest= 12,
                        WidthRequest= 12
                    });
                if(buildings.Count < 4)
                    buildings.Add(new Image
                    {
                        Source = "house.png",
                        HeightRequest = 12,
                        WidthRequest = 12
                    });
            }
            return;
        }
        if (buildings.Count == 0)
        {
            await Shell.Current.DisplayAlert("Ошибка", "У вас нет домов!", "OK");
        }
        if(buildings.Count > 0)
            buildings.RemoveAt(buildings.Count-1);
        return;
    }
}
