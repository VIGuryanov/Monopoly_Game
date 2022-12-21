using Microsoft.Extensions.Logging;
using MonopolyMAUI.Services;
using MonopolyMAUI.View;
using MonopolyMAUI.ViewModel;

namespace MonopolyMAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");                
            });

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainViewModel>();

        builder.Services.AddSingleton<RulesPage>();
        builder.Services.AddSingleton<RulesViewModel>();

        builder.Services.AddTransient<StartGame>();
        builder.Services.AddTransient<StartGameViewModel>();

        builder.Services.AddTransient<GamePage>();
        builder.Services.AddTransient<GameViewModel>();
        builder.Services.AddTransient<PlayerService>();


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
