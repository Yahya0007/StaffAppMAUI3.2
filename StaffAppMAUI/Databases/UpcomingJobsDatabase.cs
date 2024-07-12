using CommunityToolkit.Mvvm.Messaging;
using SQLite;
using StaffApp;
using StaffApp.Common.Helpers;
using StaffApp.StaffDataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffApp.StaffDataService.Databases
{
    public class UpcomingJobsHistory
    {
        [PrimaryKey]
        public int EventStaffRequiredID { get; set; }
        public DateTime? Viewed { get; set; } = null;
        public DateTime? WorkInterest { get; set; } = null;
        public DateTime? Favourite { get; set; } = null;
        public DateTime? ModifiedOn { get; set; } = DateTime.Now;
    }

    public static class UpcomingJobsDatabaseConstants
    {
        public const string UpcomingJobsDatabaseFilename = "UpcomingJobsHistoryDB.db3";

        public const SQLiteOpenFlags UpcomingJobsDatabaseFlags =
            // open the database in read/write mode
            SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLiteOpenFlags.SharedCache;

        public static string UpcomingJobsDatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, UpcomingJobsDatabaseFilename);
    }

    public static class UpcomingJobsDatabase
    {
        public static SQLiteAsyncConnection _database;

        async static Task Init()
        {
            try
            {
                if (_database is not null)
                    return;

                _database = new SQLiteAsyncConnection(UpcomingJobsDatabaseConstants.UpcomingJobsDatabasePath, UpcomingJobsDatabaseConstants.UpcomingJobsDatabaseFlags);

                var result = await _database.CreateTableAsync<UpcomingJobsHistory>();

                // var result1 = await _database.DropTableAsync<UpcomingJobsHistory>();

                // Check if the table is empty
                var count = await _database.Table<UpcomingJobsHistory>().CountAsync();
            }
            catch (Exception ex)
            {
                DataService.HandleException(ex, DataService.GetPasswordUsername(), "UpcomingJobsDatabase::Init");
            }
        }

        public static async Task<UpcomingJobsHistory> GetItemAsync(int ID)
        {
            try
            {
                await Init();

                return await _database.Table<UpcomingJobsHistory>().Where(x => x.EventStaffRequiredID == ID).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                DataService.HandleException(ex, DataService.GetPasswordUsername(), "UpcomingJobsDatabase::GetItemsAsync");
            }

            return null;
        }

        public static async Task<List<UpcomingJobsHistory>> GetItemsAsync()
        {
            try
            {
                await Init();

                return await _database.Table<UpcomingJobsHistory>().OrderByDescending(x => x.EventStaffRequiredID).ToListAsync();
            }
            catch (Exception ex)
            {
                DataService.HandleException(ex, DataService.GetPasswordUsername(), "UpcomingJobsDatabase::GetItemsAsync");
            }

            return null;
        }

        public static async Task<int> SaveItemAsync(UpcomingJobsHistory item)
        {
            int x = 0;

            try
            {
                await Init();

                x = await _database.InsertOrReplaceAsync(item);

                //var y = await UpcomingJobsDatabase.GetAllItemsAsync();

                if (x > 0)
                {
                    //WeakReferenceMessenger.Default.Send(new NotificationChangeMessage("NotificationChange"));
                }
            }
            catch (Exception ex)
            {
                DataService.HandleException(ex, DataService.GetPasswordUsername(), "UpcomingJobsDatabase::SaveItemAsync");
            }

            return x;
        }

        public static async Task<int> DeleteItemAsync(UpcomingJobsHistory item)
        {
            await Init();

            return await _database.DeleteAsync(item);
        }

        public static async Task<int> DeleteItemsOlderThanAsync(int days)
        {
            await Init();

            DateTime cutoffDate = DateTime.Now.AddDays(-days);

            // Query the database to find items older than the cutoff date
            var itemsToDelete = await _database.Table<UpcomingJobsHistory>()
                .Where(x => x.ModifiedOn < cutoffDate)
                .ToListAsync();

            int rowsAffected = 0;

            // Delete each item
            foreach (var item in itemsToDelete)
            {
                rowsAffected += await _database.DeleteAsync(item);
            }

            return rowsAffected;
        }

        public static async Task<bool> ItemExistsAsync(UpcomingJobsHistory item)
        {
            try
            {
                await Init();

                //var z = await _database.Table<UpcomingJobsHistory>().ToListAsync();

                var y = await _database.Table<UpcomingJobsHistory>().Where(x => x.EventStaffRequiredID == item.EventStaffRequiredID).CountAsync();

                if (y > 0)
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
                DataService.HandleException(ex, DataService.GetPasswordUsername(), "UpcomingJobsDatabase::ItemExistsAsync: " + item.EventStaffRequiredID.ToString());

                return false;
            }
        }

        public static async Task<bool> IsWorkInterestSent(int ID)
        {
            try
            {
                await Init();

                //var z = await _database.Table<UpcomingJobsHistory>().ToListAsync();

                var y = await _database.Table<UpcomingJobsHistory>().Where(x => (x.EventStaffRequiredID == ID) && (x.WorkInterest != null) && (x.WorkInterest != DateTime.MinValue)).CountAsync();

                if (y > 0)
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
                DataService.HandleException(ex, DataService.GetPasswordUsername(), "UpcomingJobsDatabase::ItemExistsAsync: " );

                return false;
            }
        }

        public static async Task<int> GetCount()
        {
            try
            {
                await Init();

                //int countOfNotificationsToUpdate = await _database.Table<UpcomingJobsHistory>().Where(x => (x.StaffID == DataService.GetCurrentUser.StaffItem.ID) && (x.Opened == DateTime.MinValue)).CountAsync();

                int countOfNotificationsToUpdate = await _database.Table<UpcomingJobsHistory>().CountAsync();

                //var y = await _database.Table<UpcomingJobsHistory>().OrderByDescending(x => x.Arrived).ToListAsync();

                return countOfNotificationsToUpdate;
            }
            catch (Exception ex)
            {
                DataService.HandleException(ex, DataService.GetPasswordUsername(), "UpcomingJobsDatabase::GetCount");

                return 0;
            }

        }
    }
}
