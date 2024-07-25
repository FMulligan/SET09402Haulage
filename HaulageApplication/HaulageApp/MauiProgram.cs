using Microsoft.Extensions.Logging;
using System.Reflection;
using HaulageApp.Data;
using HaulageApp.ViewModels;
using HaulageApp.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

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

        // Load configuration from embedded resource appsettings.json
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "HaulageApp.appsettings.json";
        Stream stream = null;
        
            stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new ApplicationException($"Could not find embedded resource '{resourceName}'");
            }

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            builder.Configuration.AddConfiguration(config);

            // Get the connection string from the configuration
            var connectionString = builder.Configuration.GetConnectionString("LocalConnection");
            if (connectionString == null) 
            {
                throw new ApplicationException("LocalConnection is not set");
            }
            // Register HaulageDbContext with the DI container
        builder.Services.AddDbContext<HaulageDbContext>(options => options.UseSqlServer(connectionString));

            // Register services and view models
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