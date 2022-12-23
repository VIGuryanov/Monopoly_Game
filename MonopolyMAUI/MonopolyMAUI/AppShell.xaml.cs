using MonopolyMAUI.View;

namespace MonopolyMAUI;

public partial class AppShell : Shell
{
	public AppShell()
	{
        InitializeComponent();

        Routing.RegisterRoute(nameof(RulesPage), typeof(RulesPage));
        Routing.RegisterRoute(nameof(StartGame), typeof(StartGame));
        Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));
    }
}
