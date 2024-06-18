using AlcoholTracker.Lib;
using Microsoft.Extensions.Logging;

namespace AlcoholTracker
{
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
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<ISettingsRepository, SettingsRepository>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

#if IOS
            var h = new Platforms.iOS.IosHealthRepository();
            h.Initialize();
            builder.Services.AddSingleton<IHealthRepository>(h);    //TODO: ugly
#else
            builder.Services.AddSingleton<IHealthRepository, DummyHealthRepository>();
#endif

            return builder.Build();
        }
    }
}
