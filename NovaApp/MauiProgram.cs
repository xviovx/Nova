using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using Xe.AcrylicView;
using Microcharts.Maui;

namespace NovaApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureMopups()
			.UseAcrylicView()
			.UseMicrocharts()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Poppins-Bold.ttf", "PoppinsBold");
				fonts.AddFont("Poppins-Semibold.ttf", "PoppinsSemibold");
				fonts.AddFont("Poppins-Regular.ttf", "PoppinsRegular");
				fonts.AddFont("Poppins-Medium.ttf", "PoppinsMedium");
                fonts.AddFont("Poppins-Light.ttf", "PoppinsLight");
				fonts.AddFont("Poppins-Black.ttf", "PoppinsBlack");
				fonts.AddFont("Poppins-Thin.ttf", "PoppinsThin");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
