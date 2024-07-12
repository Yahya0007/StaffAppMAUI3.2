using CommunityToolkit.Mvvm.Messaging;
using SQLite;
using StaffApp.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffApp.StaffDataService.Databases
{
    public static class Constants
    {
        public const string DatabaseFilename = "Notifications1.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }

    public static class NotificationsDatabase
    {
        public static SQLiteAsyncConnection _database;

        async static Task Init()
        {
            try
            {
                if (_database is not null)
                    return;

                _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

                // var result1 = await _database.DropTableAsync<Notification>();
                var result = await _database.CreateTableAsync<Notification>();

                // Check if the table is empty
                var count = await _database.Table<Notification>().CountAsync();
            }
            catch (Exception ex)
            {
                DataService.HandleException(ex, DataService.GetPasswordUsername(), "NotificationsDatabase::Init");
            }

            //if (count == 0)
            //{
            //    // Add a new row since the table is empty
            //    var newNotification = new Notification
            //    {
            //        Title = "Sample Title",
            //        Message = "Sample Message",
            //        Arrived = DateTime.Now,
            //        Opened = DateTime.MinValue,
            //        Username = "SampleUser",
            //        StaffID = DataService.GetCurrentUser.StaffItem.ID,
            //        EventID=0,
            //        StaffBookingID=0,
            //        Action="",
            //        ActionPayload="",
            //        Comments="",
            //        GUID=""
            //        //ItemColour = Colors.Azure
            //    };

            //    await _database.InsertAsync(newNotification);
            //}
        }

        //public NotificationsDatabase()
        //{
        //    //_database = InitialiseDB(dbPath);
        //}

        //public NotificationsDatabase(string dbPath)
        //{
        //    //_database = InitialiseDB(dbPath);
        //}

        //private SQLiteAsyncConnection InitialiseDB(string dbPath)
        //{
        //    var _database = new SQLiteAsyncConnection(dbPath);

        //    var x = _database.CreateTableAsync<Notification>();

        //    AddRowIfTableEmpty();

        //    return _database;
        //}

        //private async void AddRowIfTableEmpty()
        //{
        //    // Check if the table is empty
        //    var count = await _database.Table<Notification>().CountAsync();

        //    if (count == 0)
        //    {
        //        // Add a new row since the table is empty
        //        var newNotification = new Notification
        //        {
        //            Title = "Sample Title",
        //            Message = "Sample Message",
        //            Arrived = DateTime.Now,
        //            Opened = DateTime.Now,
        //            Username = "SampleUser",
        //            StaffID = 1,
        //            ItemColour = Colors.Azure
        //        };

        //        await _database.InsertAsync(newNotification);
        //    }
        //}

        //public Task<List<Notification>> GetNotificationsAsync()
        //{
        //    return _database.Table<Notification>().ToListAsync();

        //    //List<Notification> notifications = _database.Table<Notification>().ToListAsync();

        //    //// Use LINQ to order the list in descending order based on the Arrived property
        //    //List<Notification> sortedNotifications = notifications.OrderByDescending(notification => notification.Arrived).ToList();

        //    //return sortedNotifications;
        //}

        //public Task<int> SaveNotificationAsync(Notification notification)
        //{
        //    return _database.InsertAsync(notification);
        //}

        public static async Task<List<Notification>> GetAllItemsAsync()
        {
            try
            {
                await Init();
                return await _database.Table<Notification>().OrderByDescending(x => x.Arrived).ToListAsync();
            }
            catch (Exception ex)
            {
                DataService.HandleException(ex, DataService.GetPasswordUsername(), "NotificationsDatabase::GetAllItemsAsync");
            }

            return null;
        }

        public static async Task<List<Notification>> GetItemsAsync()
        {
            try
            {
                await Init();
                //return await _database.Table<Notification>().Where(x => x.StaffID == DataService.GetCurrentUser.StaffItem.ID).OrderByDescending(x => x.Arrived).ToListAsync();

                var currentStaffID = (DataService.GetCurrentUser == null) || (DataService.GetCurrentUser.StaffItem <= 0) ? -1 : DataService.GetCurrentUser.StaffItem;

                return await _database.Table<Notification>().Where(x => x.StaffID == currentStaffID).OrderByDescending(x => x.Arrived).ToListAsync();
            }
            catch (Exception ex)
            {
                DataService.HandleException(ex, DataService.GetPasswordUsername(), "NotificationsDatabase::GetItemsAsync");
            }

            return null;
        }

        public static async Task<List<Notification>> GetItemsNotOpenedAsync()
        {
            try
            {
                await Init();
                //return await _database.Table<Notification>().Where(x => (x.StaffID == DataService.GetCurrentUser.StaffItem.ID) && (x.Opened != DateTime.MinValue)).OrderByDescending(x => x.Arrived).ToListAsync();

                var currentStaffID = (DataService.GetCurrentUser == null) || (DataService.GetCurrentUser.StaffItem <= 0) ? -1 : DataService.GetCurrentUser.StaffItem;

                return await _database.Table<Notification>().Where(x => (x.StaffID == currentStaffID) && (x.Opened != DateTime.MinValue)).OrderByDescending(x => x.Arrived).ToListAsync();
            }
            catch (Exception ex)
            {
                DataService.HandleException(ex, DataService.GetPasswordUsername(), "NotificationsDatabase::GetItemsNotOpenedAsync");
            }
            // SQL queries are also possible
            //return await Database.QueryAsync<Notification>("SELECT * FROM [Notification] WHERE [Done] = 0");

            return null;
        }

        public static async Task<Notification> GetItemAsync(string guid)
        {
            try
            {
                await Init();

                return await _database.Table<Notification>().Where(i => i.GUID == guid).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                DataService.HandleException(ex, DataService.GetPasswordUsername(), "NotificationsDatabase::GetItemAsync");
            }

            return null;
        }

        public static async Task<int> SaveItemAsync(Notification notification)
        {
            int x = 0;

            try
            {
                await Init();

                await _database.RunInTransactionAsync(connection =>
                {
                    string guid = notification.GUID.ToUpper();

                    var existingNotification = connection.Table<Notification>()
                        .Where(n => n.GUID == guid)
                        .FirstOrDefault();

                    if (existingNotification == null)
                    {
                        x = connection.InsertOrReplace(notification);
                        //x = connection.Insert(notification);
                    }
                    //else
                    //{
                    //    x = tran.Update(notification);
                    //}
                });

                //var z = await NotificationsDatabase.GetAllItemsAsync();

                //var existingNotification = await _database.Table<Notification>()
                //   .Where(n => n.GUID.ToUpper() == item.GUID.ToUpper())
                //   .FirstOrDefaultAsync();

                //if (existingNotification == null)
                //{
                //    x = await _database.InsertAsync(item);
                //}
                //else
                //{
                //    x = await _database.UpdateAsync(item);
                //}

                //if (item.Id != 0)
                //{
                //    x = await _database.UpdateAsync(item);
                //}
                //else
                //{
                //    x = await _database.InsertAsync(item);
                //}

                //var y = await NotificationsDatabase.GetAllItemsAsync();

                if (Microsoft.Maui.Devices.DeviceInfo.Platform == Microsoft.Maui.Devices.DevicePlatform.Android)
                {
                    if (x > 0)
                    {
                        WeakReferenceMessenger.Default.Send(new NotificationChangeMessage("NotificationChange"));
                    }
                }
            }
            catch (Exception ex)
            {
                DataService.HandleException(ex, DataService.GetPasswordUsername(), "NotificationsDatabase::SaveItemAsync");
            }

            return x;
        }

        public static async Task<int> DeleteItemAsync(Notification item)
        {
            await Init();
            return await _database.DeleteAsync(item);
        }

        public static async Task ClearOpenedHighlight()
        {
            try
            {
                await Init();

                //var notificationsToUpdate = await _database.Table<Notification>().Where(x => (x.StaffID == DataService.GetCurrentUser.StaffItem.ID) && (x.Opened == DateTime.MinValue)).ToListAsync();

                var currentStaffID = (DataService.GetCurrentUser == null) || (DataService.GetCurrentUser.StaffItem <= 0) ? -1 : DataService.GetCurrentUser.StaffItem;

                var notificationsToUpdate = await _database.Table<Notification>().Where(x => (x.StaffID == currentStaffID) && (x.Opened == DateTime.MinValue)).ToListAsync();

                // Update the 'Opened' property to DateTime.Now for each notification
                foreach (var notification in notificationsToUpdate)
                {
                    notification.Opened = DateTime.Now;
                    await _database.UpdateAsync(notification);
                }
            }
            catch (Exception ex)
            {
                DataService.HandleException(ex, DataService.GetPasswordUsername(), "NotificationsDatabase::ClearOpenedHighlight");
            }

            return;
        }

        //public static async Task<Notification> GetItembyGUIDAsync(string gUID)
        //{
        //    await Init();

        //    Notification x;

        //    try
        //    {
        //        var y = await _database.Table<Notification>().Where(x => x.GUID == gUID).ToListAsync();

        //        if (y.Count > 0)
        //        {
        //            x = y[0];
        //        }
        //        else
        //        {
        //            x = null;
        //        }
        //    }
        //    catch
        //    {
        //        x = null;
        //    }

        //    return x;
        //}

        public static async Task<bool> ItemExistsAsync(Shiny.Push.PushNotification notification)
        {
            string gUID = "";

            if ((notification.Data != null) && (notification.Data.ContainsKey("GUID")))
            {
                gUID = notification.Data["GUID"] ?? "";
                // Use actionPayload as needed
            }

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

            if ((string.IsNullOrEmpty(gUID)) & (string.IsNullOrEmpty(Title)) & (string.IsNullOrEmpty(Message)))
            {
                return true;
            }

            try
            {
                await Init();

                //var z = await _database.Table<Notification>().ToListAsync();

                //var y = await _database.Table<Notification>().Where(x => x.Title == Title & x.Message == Message & ((DateTime.Now - x.Arrived).TotalMinutes < 1)).ToListAsync();

                List<Notification> y = null;

                // Calculate the threshold time (one minute ago)
                //DateTime thresholdTime = DateTime.Now.AddMinutes(-1);

                // DO NOT USE: Gives error as substract operation not supported
                //y = await _database.Table<Notification>().Where(x => x.Title == Title & x.Message == Message & ((DateTime.Now - x.Arrived).TotalMinutes < 1)).ToListAsync();

                //y = await _database.Table<Notification>()
                //   .Where(x => x.Title == Title
                //               && x.Message == Message
                //               && x.Arrived >= thresholdTime)
                //   .ToListAsync();

                y = await _database.Table<Notification>()
                   .Where(x => x.GUID == gUID)
                   .ToListAsync();

                if (y.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                DataService.HandleException(ex, DataService.GetPasswordUsername(), "NotificationsDatabase::ItemExistsAsync: " + Title + " : " + Message);

                return false;
            }
        }

        public static async Task<int> GetNotificationsCount()
        {
            try
            {
                await Init();

                //int countOfNotificationsToUpdate = await _database.Table<Notification>().Where(x => (x.StaffID == DataService.GetCurrentUser.StaffItem.ID) && (x.Opened == DateTime.MinValue)).CountAsync();

                //int countOfNotificationsToUpdate = await _database.Table<Notification>().Where(x => (x.StaffID == DataService.GetCurrentUser.StaffItem) && (x.Opened == DateTime.MinValue)).CountAsync();

                // Retrieve the current staff ID
                //var currentStaffID = DataService.GetCurrentUser.StaffItem;

                var currentStaffID = (DataService.GetCurrentUser == null) || (DataService.GetCurrentUser.StaffItem <= 0) ? -1 : DataService.GetCurrentUser.StaffItem;

                // Ensure DateTime.MinValue is used in the query correctly
                //int countOfNotificationsToUpdate = await _database.Table<Notification>()
                //                                                  .Where(x => (x.StaffID == currentStaffID) && (x.Opened == DateTime.MinValue))
                //                                                  .CountAsync();

                // Get all notifications for the current staff
                var notifications = await _database.Table<Notification>()
                                                   .Where(x => x.StaffID == currentStaffID)
                                                   .ToListAsync();

                // Filter notifications where Opened is DateTime.MinValue
                int countOfNotificationsToUpdate = notifications.Count(x => x.Opened == DateTime.MinValue);

                return countOfNotificationsToUpdate;

                //var y = await _database.Table<Notification>().OrderByDescending(x => x.Arrived).ToListAsync();

                //return countOfNotificationsToUpdate;
            }
            catch (Exception ex)
            {
                DataService.HandleException(ex, DataService.GetPasswordUsername(), "NotificationsDatabase::GetNotificationsCount");

                return -1;
            }

        }
    }
}
