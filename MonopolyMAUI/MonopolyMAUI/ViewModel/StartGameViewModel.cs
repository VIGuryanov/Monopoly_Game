using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MonopolyMAUI.Models;
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
    User user;

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
            return;
        }
        if (UserName.Length >= 18)
        {
            NicknameError = "To big nickname!";
            UserName = string.Empty;
            return;
        }
        //TODO: возможно придётся делать проверку на одинаковые никнеймы(вопрос где именно? тут или на сервере?
        try
        {
            IsBusy = true;
            User = new User() { Nickname = UserName };
            Players.Add(user);
            await Task.Delay(1000);//Убрать
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
            //IsBusy = false;
        }
    }
}
