
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MonopolyMAUI.Models;
using MonopolyMAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MonopolyMAUI.ViewModel;

//[QueryProperty("Players", "Players")]
public partial class GameViewModel : ObservableObject
{
    PlayerService playerService;
    public ObservableCollection<User> Players { get; set; } = new();

    public GameViewModel(PlayerService service)
    {
        playerService = service;
    }

    [RelayCommand]
    async Task GetPlayersAsync()
    {
        try
        {
            var players = await playerService.GetPlayersFromJsonAsync();
            if(players?.Count != 0)
                Players.Clear();

            foreach(var player in players)
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
    }
}
