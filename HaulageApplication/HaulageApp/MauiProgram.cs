using Microsoft.Extensions.Logging;
using System.Reflection;
using HaulageApp.Data;
using HaulageApp.Services;
using HaulageApp.ViewModels;
using HaulageApp.Views;
using Microsoft.Extensions.Configuration;

namespace HaulageApp;

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
        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("HaulageApp.appsettings.json");

        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

        builder.Configuration.AddConfiguration(config);
        builder.Services.AddSingleton<HaulageDbContext>();
        builder.Services.AddSingleton<INoteService, NoteService>();

        builder.Services.AddSingleton<AllNotesViewModel>();
        builder.Services.AddTransient<NoteViewModel>();

        builder.Services.AddSingleton<AllNotesPage>();
        builder.Services.AddTransient<NotePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}