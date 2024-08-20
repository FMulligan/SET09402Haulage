﻿using Microsoft.Extensions.Logging;
using System.Reflection;
using HaulageApp.Data;
using HaulageApp.ViewModels;
using HaulageApp.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Maui;

namespace HaulageApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("HaulageApp.appsettings.json");

        var config = new ConfigurationBuilder()
            .AddJsonStream(stream!)
            .Build();

        builder.Configuration.AddConfiguration(config);
        
        var connectionString = builder.Configuration.GetConnectionString("LocalConnection");
        if (connectionString == null)
            throw new ApplicationException("LocalConnection is not set");
        
        builder.Services.AddDbContext<HaulageDbContext>(options => options.UseSqlServer(connectionString));
        
        builder.Services.AddSingleton<AllNotesViewModel>();
        builder.Services.AddTransient<NoteViewModel>();
        
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();
        
        builder.Services.AddSingleton<AllNotesPage>();
        builder.Services.AddTransient<NotePage>();
        
        builder.Services.AddSingleton<AllVehiclesPage>();
        builder.Services.AddTransient<VehiclePage>();
        builder.Services.AddSingleton<AllVehiclesViewModel>();
        builder.Services.AddTransient<VehicleViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}