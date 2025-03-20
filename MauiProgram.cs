using AppMultasDigesett.Vistas;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
//using Plugin.Maui.Audio;

namespace AppMultasDigesett;

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
			}).AddAudio();
		builder.Services.AddSingleton(AudioManager.Current);
		builder.Services.AddTransient<CrearMulta>();
#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
