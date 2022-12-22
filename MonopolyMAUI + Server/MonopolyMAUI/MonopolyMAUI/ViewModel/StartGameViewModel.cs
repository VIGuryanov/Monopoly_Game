using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MonopolyMAUI.Models;
using MonopolyMAUI.Server;
using MonopolyMAUI.Services;
using MonopolyMAUI.View;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MonopolyMAUI.ViewModel;

public partial class StartGameViewModel : BaseViewModel
{
    [ObservableProperty]

    public ObservableCollection<User> players = new();

    public PlayerService playerService;

    public StartGameViewModel() { }

    public StartGameViewModel(PlayerService playerService)
    {
        this.playerService = playerService;
    }

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
        try
        {
            IsBusy = true;
            //Players.Add(new User() { Nickname = UserName });
            await Task.Run(() => DialogueMethods.SendMyNickname(UserName));
            var playersNicks = await Task.Run(DialogueMethods.GetTeamNickNames);

            var rnd = new Random();
            foreach (var nick in playersNicks)
                Players.Add(new User() { Nickname = nick, Color = Color.FromRgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)) });

            PlayersList.Players = Players.Select(x=>new User_Player {UserEntity = x, PlayerEntity = new Monopoly_class_library.Player() }).ToArray();
            foreach(var userPlayer in PlayersList.Players)
                if(userPlayer.UserEntity.Nickname == UserName)
                    ClientProcess.Client.UserPlayerEntity = userPlayer;
            //var players = await playerService.GetPlayersFromJsonAsync();/*Наверное,вот тут нужно ответ с сервера*/
            //Сделать анимацию в StartGame.xaml.cs
            /*if (Players.Count > 0)
                Players.Clear();
            foreach (var player in players)
                Players.Add(player);*/
            await Shell.Current.GoToAsync(nameof(GamePage), new Dictionary<string, object>
            {
                {"Players", Players}
            });
        }
        catch (Exception ex)
        {

            Debug.WriteLine($"Unable to get players: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /*[RelayCommand]
    async Task GetPlayersAsync()
    {
        try
        {
            var players = await playerService.GetPlayersFromJsonAsync();
            if (players?.Count != 0)
                Players.Clear();

            foreach (var player in players)
                Players.Add(player);
        }
        catch (Exception ex)
        {
#if DEBUG
            Debug.WriteLine(ex);
#endif
            //Этого делать не следует!, просто в качестве примера
            await Shell.Current.DisplayAlert("Error!", $"Unable to get players:{ex.Message}", "OK");
        }
        finally
        {

        }
    }*/
}
