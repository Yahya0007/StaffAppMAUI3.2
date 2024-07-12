using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DevExpress.Data.Linq.Helpers;
//using MvvmHelpers;
using Shiny.Push;
using StaffApp;
//using StaffApp.StaffDataService.Databases;
using StaffApp.Views;
using static SQLite.SQLite3;


public class PushDelegate : IPushDelegate
{
    public Task OnEntry(PushNotification notification)
    {
        return Task.CompletedTask;
    }

    public Task OnReceived(PushNotification notification)
    {
        return Task.CompletedTask;
    }

    public Task OnTokenRefreshed(string token)
    {
        Preferences.Set("PushNotificationsToken", token);

        return Task.CompletedTask;
    }

    public Task OnNewToken(string token)
    {
        Preferences.Set("PushNotificationsToken", token);

        return Task.CompletedTask;
    }

    public Task OnUnRegistered(string token)
    {
        Preferences.Set("PushNotificationsToken", token);

        return Task.CompletedTask;
    }
}