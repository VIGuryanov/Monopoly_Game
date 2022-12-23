using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MonopolyMAUI.View;

namespace MonopolyMAUI.ViewModel;

public partial class MainViewModel : ObservableObject
{
    [RelayCommand]
    async Task TapRules()
    {
        await Shell.Current.GoToAsync(nameof(RulesPage));
    }

    [RelayCommand]
    async Task TapGame()
    {
        await Shell.Current.GoToAsync(nameof(StartGame));
    }
}

