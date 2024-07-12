using DevExpress.Xpo;
using StaffApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Reflection;
using StaffApp.StaffDataService;
using DevExpress.Maui.Editors;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using Microsoft.Maui.Primitives;
using DevExpress.Maui.CollectionView;
using CommunityToolkit.Mvvm.Messaging;
using static StaffApp.App;
using DevExpress.Maui.Core;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;
namespace StaffApp.ViewModels
{
    class UpcomingJobsViewModel : BaseViewModel
    {
        string filter;
        public ObservableCollection<FilterItem> PredefinedFilters
        {
            get;
            set;
        }

        public BindingList<FilterItem> SelectedFilters
        {
            get;
            set;
        }

        public string Filter
        {
            get
            {
                return filter;
            }
            set
            {
                filter = value;
                OnPropertyChanged("Filter");
            }
        }

        public Command<object> MapCommand { get; set; }

        public Command<object> WorkRequestCommand { get; set; }

        public Command<object> BookYourselfOnJobCommand { get; set; }

        public Command LogOutCommand { get; }

        public Command<UpcomingJobs> ItemTapped { get; }

        public ObservableCollection<UpcomingJobs> UpcomingJobsList { get; set; }

        public ICommand AddToFavoritesCommand { get; }

        public int NextPageNoToLoad = 0;

        bool isRefreshing = false;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    OnPropertyChanged("IsRefreshing");
                }
            }
        }

        ICommand loadItemsCommand;
        public ICommand LoadItemsCommand
        {
            get { return loadItemsCommand; }
            set
            {
                if (loadItemsCommand != value)
                {
                    loadItemsCommand = value;
                    OnPropertyChanged("LoadItemsCommand");
                }
            }
        }

        ICommand loadMoreItemsCommand = null;
        public ICommand LoadMoreCommand
        {
            get { return loadMoreItemsCommand; }
            set
            {
                if (loadMoreItemsCommand != value)
                {
                    loadMoreItemsCommand = value;
                    OnPropertyChanged("LoadMoreItemsCommand");
                }
            }
        }

        bool isenabled = true;
        public bool IsEnabled
        {
            get => isenabled;
            set => SetProperty(ref isenabled, value);
        }

        public UpcomingJobsViewModel()
        {
            IsEnabled = true;
            IsBusy = false;
            NextPageNoToLoad = 0;

            Title = "Upcoming Jobs";

            LoadItemsCommand = new Command(OnLoadItemsCommand);

            LoadMoreCommand = new Command(OnLoadMoreItemsCommand);

            WorkRequestCommand = new Command<object>(WorkRequest);

            UpcomingJobsList = new ObservableCollection<UpcomingJobs>();

            AddToFavoritesCommand = new Command<UpcomingJobs>(AddToFavorites);

            SelectedFilters = new BindingList<FilterItem>();
            PredefinedFilters = new ObservableCollection<FilterItem>() {
            new FilterItem(){ DisplayText= "M", Filter = "GetDayOfWeek([EventDate]) = 1" },
            new FilterItem(){ DisplayText= "T", Filter = "GetDayOfWeek([EventDate]) = 2" },
            new FilterItem(){ DisplayText= "W", Filter = "GetDayOfWeek([EventDate]) = 3" },
            new FilterItem(){ DisplayText= "T", Filter = "GetDayOfWeek([EventDate]) = 4" },
            new FilterItem(){ DisplayText= "F", Filter = "GetDayOfWeek([EventDate]) = 5" },
            new FilterItem(){ DisplayText= "S", Filter = "GetDayOfWeek([EventDate]) = 6" },
            new FilterItem(){ DisplayText= "S", Filter = "GetDayOfWeek([EventDate]) = 7" },
            };

            SelectedFilters.ListChanged += SelectedFiltersChanged;

            OnLoadItemsCommand();
        }

        private void SelectedFiltersChanged(object sender, ListChangedEventArgs e)
        {
            if (SelectedFilters.Count > 0)
                Filter = String.Join(" OR ", SelectedFilters.Select(f => f.Filter));
            else
                Filter = string.Empty;
        }

        public void AddToFavorites(UpcomingJobs upcomingjobs)
        {
            if (IsBusy)
            {
                return;
            }

            upcomingjobs.IsFavorite = !upcomingjobs.IsFavorite;

        }
        void OnMapCommand(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            var x = (obj as UpcomingJobs).VenuePostcode;

            var placemark = new Placemark
            {
                PostalCode = x
            };
            var options = new MapLaunchOptions { Name = (obj as UpcomingJobs).Venue ?? "", NavigationMode = NavigationMode.None };
            Map.OpenAsync(placemark, options);

        }

        void WorkRequest(object obj)
        {
        }

        private void BookYourselfOnJob(object obj)
        {
        }

        private async void OnLoadMoreItemsCommand()
        {
            if (IsBusy)
            {
                return;
            }

            await ExecuteLoadItemsCommand();

        }

        private async void OnLoadItemsCommand()
        {
            if (IsBusy)
            {
                return;
            }

            NextPageNoToLoad = 0;
            UpcomingJobsList.Clear();

            await ExecuteLoadItemsCommand();

        }

        public Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
            {
                return Task.CompletedTask;
            }

            IsBusy = true;
            IsRefreshing = true;
            IsEnabled = false;


            try
            {
                Random rand = new Random();

                string[] jobTypes = { "Manager", "Supervisor", "Assistant", "Clerk", "Operator", "Technician", "Engineer", "Manager", "Supervisor", "Clerk" };

                for (int i = 0; i < 30; i++)
                {
                    var y = new UpcomingJobs
                    {
                        Client = RandomString(rand, 10),
                        AppInfo = RandomString(rand, 15),
                        EventID = rand.Next(1, 1000),
                        EventDate = RandomDate(rand),
                        Weekday = rand.Next(1, 8),
                        EventStaffRequiredID = rand.Next(1, 1000),
                        MapLink = RandomString(rand, 20),
                        MeetingPoint = RandomString(rand, 15),
                        NearestStation = RandomString(rand, 15),
                        StaffRequired = rand.Next(1, 100),
                        StaffBooked = rand.Next(1, 100),
                        StaffBalance = rand.Next(1, 100),
                        EventStatus = RandomString(rand, 10),
                        EventType = RandomString(rand, 10),
                        StaffNotes = RandomString(rand, 20),
                        StartTime = RandomDate(rand),
                        EndTime = RandomDate(rand),
                        JobsUniforms = RandomString(rand, 10),
                        Venue = RandomString(rand, 20),
                        VenueFirst = RandomString(rand, 20),
                        VenuePostcode = RandomString(rand, 10),
                        VenueDetails = RandomString(rand, 30),
                        JobType = RandomString(rand, 10),
                        //JobType = jobTypes[rand.Next(jobTypes.Length)],
                        AppRole = jobTypes[rand.Next(jobTypes.Length)],
                        StaffJobBlob = RandomString(rand, 20),
                        ItemColour = Color.FromRgb(rand.Next(256), rand.Next(256), rand.Next(256)),
                        ItemBackgroundColour = Color.FromRgb(rand.Next(256), rand.Next(256), rand.Next(256)),
                        isAllowSelfJobBooking = rand.Next(2) == 1,
                        isAllowRequestforWork = rand.Next(2) == 1,
                        IsFavorite = rand.Next(2) == 1,
                        IsJobDisabled = rand.Next(2) == 1
                    };

                    UpcomingJobsList.Add(y);
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
            finally
            {
                IsBusy = false;
                IsEnabled = true;

                IsRefreshing = false;

            }

            return Task.CompletedTask;
        }

        private static string RandomString(Random rand, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] buffer = new char[length];
            for (int i = 0; i < length; i++)
            {
                buffer[i] = chars[rand.Next(chars.Length)];
            }
            return new string(buffer);
        }

        private static DateTime RandomDate(Random rand)
        {
            DateTime start = new DateTime(2000, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(rand.Next(range)).AddHours(rand.Next(24)).AddMinutes(rand.Next(60)).AddSeconds(rand.Next(60));
        }

    }

    public class FilterItem
    {
        public string DisplayText { get; set; }
        public string Filter { get; set; }
    }
}