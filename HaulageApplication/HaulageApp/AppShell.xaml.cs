﻿using HaulageApp.Views;

namespace HaulageApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("home", typeof(AllNotesPage));
        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute(nameof(NotePage), typeof(NotePage));
        Routing.RegisterRoute("settings", typeof(SettingsPage));
    }
}