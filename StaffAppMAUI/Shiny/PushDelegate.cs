using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DevExpress.Data.Linq.Helpers;
//using MvvmHelpers;
using Shiny.Push;
using StaffApp;
using StaffApp.StaffDataService;
using StaffApp.StaffDataService.Databases;
using StaffApp.Views;
using static SQLite.SQLite3;


public class PushDelegate : IPushDelegate
{
    List<string> InProcessNotifications = new List<string>();

    string LastTitle = "";
    string LastMessage = "";
    static DateTime LastNotification = DateTime.MinValue;
    static bool Navigated = false;

    string PreviousTitle = null;
    string PreviousMessage = null;

    bool flag = false;

    private static readonly object _lock = new object();

    public Task OnEntry(PushNotification notification)
    {
        try
        {
            if (!Navigated)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (Shell.Current != null)
                    {
                        Navigated = true;

                        await Shell.Current.GoToAsync("//LoginPage");
                    }
                });
            }
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }

        return Task.CompletedTask;

    }

    public async Task OnReceived(PushNotification notification)
    {

        //return;

        lock (_lock)
        {
            if (flag)
            {
                return;
            }
            flag = true;
        }

        try
        {
            if (notification == null)
            {
                throw new Exception("Notification is null in PushDelegate::OnReceived");
            }

            await InsertNotification(notification);
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);

        }
        finally
        {
            lock (_lock)
            {
                flag = false;
            }
        }
    }

    private async Task<bool> InsertNotification(PushNotification notification)
    {
        string Title = notification.Notification == null || notification.Notification?.Title == null ? "" : notification.Notification?.Title;
        string Message = notification.Notification == null || notification.Notification?.Message == null ? "" : notification.Notification?.Message;


        if ((string.IsNullOrEmpty(Title)) & (notification.Data != null) & (notification.Data.ContainsKey("Title")))
        {
            Title = notification.Data["Title"];
        }

        if ((string.IsNullOrEmpty(Message)) & (notification.Data != null) & (notification.Data.ContainsKey("Message")))
        {
            Message = notification.Data["Message"];
        }

        LastTitle = Title;
        LastMessage = Message;
        LastNotification = DateTime.Now;

        int staffID = 0;
        int eventID = 0;
        int staffBookingID = 0;
        string comments = "";
        string action = "";
        string actionPayload = "";
        string gUID = "";

        try
        {
            if (notification.Data != null)
            {
                if (notification.Data.ContainsKey("GUID"))
                {
                    gUID = notification.Data["GUID"].ToUpper() ?? "";
                }

                if (notification.Data.ContainsKey("StaffID"))
                {
                    if (!int.TryParse(notification.Data["StaffID"], out staffID))
                    {
                        staffID = -1;
                    }
                }

                if (notification.Data.ContainsKey("EventID"))
                {
                    if (!int.TryParse(notification.Data["EventID"], out eventID))
                    {
                        eventID = -1;
                    }
                }

                if (notification.Data.ContainsKey("StaffBookingID"))
                {
                    if (!int.TryParse(notification.Data["StaffBookingID"], out staffBookingID))
                    {
                        staffBookingID = -1;
                    }
                }

                if (notification.Data.ContainsKey("Comments"))
                {
                    comments = notification.Data["Comments"] ?? "";
                }

                if (notification.Data.ContainsKey("Action"))
                {
                    action = notification.Data["Action"] ?? "";
                }

                if (notification.Data.ContainsKey("ActionPayload"))
                {
                    actionPayload = notification.Data["ActionPayload"] ?? "";
                }
            }
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }

        try
        {

            var y = await NotificationsDatabase.SaveItemAsync(new StaffApp.StaffDataService.Notification
            {
                Title = Title ?? "<empty>",
                Message = Message ?? "<empty>",
                Arrived = DateTime.Now,
                Opened = DateTime.MinValue,
                //Username = (DataService.GetCurrentUser == null) || (DataService.GetCurrentUser.UserName == null) ? "" : DataService.GetCurrentUser.UserName,
                Username = "abc",
                //StaffID = (DataService.GetCurrentUser == null) || (DataService.GetCurrentUser.StaffItem <= 0) ? -1 : DataService.GetCurrentUser.StaffItem,
                StaffID = 1,
                EventID = eventID,
                StaffBookingID = staffBookingID,
                Comments = comments ?? "",
                Action = action ?? "",
                ActionPayload = actionPayload ?? "",
                GUID = gUID.ToUpper() ?? "",
            });

            if (y > 0)
            {
                Navigated = false;
            }

            return true;
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);

            return false;
        }
    }

    public async Task OnTokenRefreshed(string token)
    {
        Preferences.Set("PushNotificationsToken", token);

        //await DataService.SetPushToken(token);

    }

    public async Task OnNewToken(string token)
    {
        Preferences.Set("PushNotificationsToken", token);

       //await DataService.SetPushToken(token);
    }

    public async Task OnUnRegistered(string token)
    {
        Preferences.Set("PushNotificationsToken", token);

        //await DataService.SetPushToken(token);
    }
}