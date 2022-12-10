using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace MauiApp3;

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

        /*Window window = new Window();
        window.Height = Microsoft.Maui.Devices.DeviceDisplay.MainDisplayInfo.Height;
        window.Width = Microsoft.Maui.Devices.DeviceDisplay.MainDisplayInfo.Width;*/

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
