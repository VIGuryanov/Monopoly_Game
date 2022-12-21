using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MonopolyMAUI.Models;
using MonopolyMAUI.View;
using System.Collections.ObjectModel;

namespace MonopolyMAUI.ViewModel;

public partial class StartGameViewModel : BaseViewModel
{
    public ObservableCollection<User> Players { get; set; } = new();

    [ObservableProperty]
    string userName;

    [ObservableProperty]
    string nicknameError;

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task Submit()
    {
        if (string.IsNullOrWhiteSpace(UserName))
        {
            NicknameError = "Empty nickname! Please enter your nickname";
            UserName = string.Empty;
            return;//Тут надо написать про плохой никнейм иначе ожидание подключения других пользователей
        }
        if (UserName.Length >= 18)
        {
            NicknameError = "To big nickname!";
            UserName = string.Empty;
            return;//Тут надо написать про плохой никнейм иначе ожидание подключения других пользователей
        }
        //TODO: возможно придётся делать проверку на одинаковые никнеймы(вопрос где именно? тут или на сервере?
        //или сделать коллекцию, и уже там все ники будут,
        //но если всегда разные клиенты запускаются, то будет ли там инфа по всем пользователям?(Поэтому надо скорее всего с серва отправлять)

        //TODO: тут нужно сделать обращение к серверу и подождать других пользователей(подключения всех пользователь)
        //И здесь нужно продумать добавление игроков в список на странице игры(В видосике про MVVM или навигации говорилось про это)
        //(и потом просто сделать уже на странице игры кнопку "ГОТОВ")
        //Players = await ...
        Players.Add(new User() { Nickname = UserName});
        //TODO: ПЕРЕДЕЛАТЬ ПОД MVVM(Binding Players) И СЕРВЕР
        await Shell.Current.GoToAsync(nameof(GamePage),true, new Dictionary<string, object> {
            {"Players", Players}
        });
        //UserName = string.Empty;
    }
}
