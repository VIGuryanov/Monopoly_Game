using MonopolyMAUI.ViewModel;

namespace MonopolyMAUI.View;

public partial class StartGame : ContentPage
{
    StartGameViewModel vm;
    public StartGame(StartGameViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
    }


}