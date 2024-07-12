using DevExpress.Maui.Core;
using DXMaterialThemesApp;
using StaffApp.StaffDataService;
using StaffApp.StaffDataService.Databases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffApp.Library
{
    public static class Utilities
    {
        private const string IsCustomSourceKey = "IsCustomSource";
        private const string IsLightThemeKey = "IsLightTheme";
        private const string IsDarkThemeKey = "IsDarkTheme";
        private const string ThemeColorKey = "ThemeColor";
        private const string IsSystemColorKey = "IsSystemColor";
        private const string ColorModelKey = "ColorModel";
        //private static readonly string DefaultPreviewColorHex = "#552E5D";

        private const int MaxClicksPerDay = 10;
        private const string ClickCountKey = "ClickCount";
        private const string LastResetDateKey = "LastResetDate";

        public static void CheckAndUpdateWorkRequestsCount()
        {
            var today = DateTime.Today;
            var lastResetDate = Preferences.Get(LastResetDateKey, DateTime.MinValue);

            if (lastResetDate != today)
            {
                Preferences.Set(ClickCountKey, 0);
                Preferences.Set(LastResetDateKey, today);
            }

        }

        public static async void SendWorkRequest(ObservableCollection<UpcomingJobs> UpcomingJobsList, bool isSelfJobBooking)
        {
            await UpcomingJobsDatabase.DeleteItemsOlderThanAsync(30);

            int requestssent = 0;

            var clickCount = Preferences.Get(ClickCountKey, 0);

            var f = UpcomingJobsList.Where(x => x.IsFavorite && !x.IsJobDisabled).ToList();

            foreach (UpcomingJobs upcomingjob in f)
            {
                try
                {
                    if (clickCount >= MaxClicksPerDay)
                    {
                        StaffApp.Common.Library.Utils.DisplayToast("Your quota (" + MaxClicksPerDay.ToString() + " requests) is fully used. Please try again tomorrow.");

                        await Task.Delay(3000);

                        break;
                    }

                    clickCount++;

                    string StaffID = DataService.GetCurrentUser.StaffItem <= 0 ? "Staff ID missing as no Staff assigned to this user" : DataService.GetCurrentUser.StaffItem.ToString();

                    upcomingjob.Username = DataService.GetCurrentUser.UserName;
                    upcomingjob.StaffID = DataService.GetCurrentUser.StaffItem <= 0 ? -1 : DataService.GetCurrentUser.StaffItem;
                    upcomingjob.isAllowSelfJobBooking = isSelfJobBooking;
                    upcomingjob.StaffIDSt = StaffID;

                    var x = await DataService.SendWorkRequestAsync(upcomingjob);

                    if (x)
                    {
                        var z = UpcomingJobsList.Where(x => x.EventStaffRequiredID == upcomingjob.EventStaffRequiredID).FirstOrDefault();

                        if (z != null)
                        {
                            z.IsJobDisabled = true;
                        }
                        else
                        {
                            z.IsJobDisabled = false;
                        }

                        var y = new UpcomingJobsHistory();

                        y.EventStaffRequiredID = upcomingjob.EventStaffRequiredID;
                        y.ModifiedOn = DateTime.Now;
                        y.WorkInterest = DateTime.Now;

                        await UpcomingJobsDatabase.SaveItemAsync(y);

                        Preferences.Set(ClickCountKey, clickCount);

                        requestssent = requestssent + 1;
                    }
                }
                catch (Exception ex)
                {
                    DataService.HandleException(ex, DataService.GetPasswordUsername(DataService._username, DataService._password), "StaffApp.ViewModels.UpcomingJobsViewModel.WorkRequest");
                }
            }

            StaffApp.Common.Library.Utils.DisplayToast(requestssent.ToString() + " request(s) to work sent.");
        }

        public static void ApplyTheme(bool isCustomSource, bool isLightTheme, bool isDarkTheme, Color themeColor, bool isSystemColor, bool isSave = true)
        {
            // Apply the theme based on the parameters

            if (isLightTheme)
            {
                ThemeManager.AndroidNavigationBarForeground = AppTheme.Light;
                ThemeManager.AndroidNavigationBarBackground = ThemeManager.Theme.Scheme.Background;
                ThemeManager.AndroidStatusBarBackground = ThemeManager.Theme.Scheme.Background;
                ThemeManager.StatusBarForeground = AppTheme.Light;

                //ThemeManager.ApplyThemeToSystemBars = true;
            }

            if (isDarkTheme)
            {
                ThemeManager.AndroidNavigationBarForeground = AppTheme.Dark;
                ThemeManager.AndroidNavigationBarBackground = ThemeManager.Theme.Scheme.Background;
                ThemeManager.AndroidStatusBarBackground = ThemeManager.Theme.Scheme.Background;
                ThemeManager.StatusBarForeground = AppTheme.Dark;

                //ThemeManager.ApplyThemeToSystemBars = true;
            }

            if (isSave)
            {
                // Save the preferences
                Preferences.Set(IsCustomSourceKey, isCustomSource);
                Preferences.Set(IsLightThemeKey, isLightTheme);
                Preferences.Set(IsDarkThemeKey, isDarkTheme);
                Preferences.Set(ThemeColorKey, themeColor.ToHex()); // Saving color as ARGB int
                Preferences.Set(IsSystemColorKey, isSystemColor);
                //Preferences.Set(ColorModelKey, DXMaterialThemesApp.ColorModel);
            }
        }

        public static void LoadAndApplyTheme()
        {
            //////// Read the preferences
            //////bool isCustomSource = Preferences.Get(IsCustomSourceKey, false);
            //////bool isLightTheme = Preferences.Get(IsLightThemeKey, false);
            //////bool isDarkTheme = Preferences.Get(IsDarkThemeKey, false);
            //////string previewColorHex = Preferences.Get(ThemeColorKey, DefaultPreviewColorHex); // Default to #552E5D if not set
            //////Color themeColor = Color.FromArgb(previewColorHex);
            //////bool isSystemColor = Preferences.Get(IsSystemColorKey, false);
            ////////DXMaterialThemesApp.ColorModel colorModel = DeserializeColorModel(Preferences.Get(ColorModelKey, string.Empty));

            //////if (isSystemColor)
            //////{
            //////    ThemeManager.UseAndroidSystemColor = true;
            //////    return;
            //////}
            //////else
            //////{
            //////    ThemeManager.UseAndroidSystemColor = false;
            //////}

            //////ThemeManager.Theme = new Theme(themeColor);

            //////// Apply the theme based on the saved preferences
            ////////ApplyTheme(isCustomSource, isLightTheme, isDarkTheme, previewColor);
            //////ApplyTheme(isCustomSource, isLightTheme, isDarkTheme, themeColor, isSystemColor);
        }

        private static string SerializeColorModel(DXMaterialThemesApp.ColorModel colorModel)
        {
            if (colorModel == null)
                return string.Empty;

            return $"{colorModel.Color.ToHex()}|{colorModel.Name}|{colorModel.DisplayName}|{colorModel.IsSystemColor}";
        }

        private static DXMaterialThemesApp.ColorModel DeserializeColorModel(string colorModelString)
        {
            if (string.IsNullOrEmpty(colorModelString))
                return null;

            var parts = colorModelString.Split('|');
            if (parts.Length != 4)
                return null;

            return new DXMaterialThemesApp.ColorModel(Color.FromArgb(parts[0]), parts[2], bool.Parse(parts[3]));

            //{
            //    color = Color.FromArgb(parts[0]),
            //    Name = parts[1],
            //    DisplayName = parts[2],
            //    IsSystemColor = bool.Parse(parts[3])
            //};
        }
    }
}
