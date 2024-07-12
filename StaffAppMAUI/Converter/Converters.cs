using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Collections.ObjectModel;
using StaffApp.ViewModels;
using System.Windows.Input;
//using Syncfusion.Maui.ListView;

namespace StaffApp.Converter
{
//    public class BorderMarginConverter : IValueConverter
//    {
//#nullable enable
//        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
//#nullable disable
//        {
//            var firstItem = (parameter as SfListView)?.DataSource?.DisplayItems[0];
//            if (value is DateTime && firstItem is DateTime)
//            {
//                if ((DateTime)value == (DateTime)firstItem)
//                {
//                    return new Thickness(0, 0, 0, 0);
//                }
//                else
//                {
//                    return new Thickness(-4, 0, 0, 0);
//                }
//            }

//            return new Thickness(0, 0, 0, 0);
//        }

//#nullable enable
//        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
//#nullable disable
//        {
//            throw new NotImplementedException();
//        }
//    }

    //public class CornerRadiusConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (parameter == null)
    //        {
    //            Console.WriteLine("ConverterParameter is null");
    //            return new CornerRadius(0);
    //        }

    //        // If parameter is a Binding, get the actual value it points to
    //        if (parameter is Binding binding)
    //        {
    //            // Assuming the binding is pointing to the correct property path
    //            parameter = binding.Source; // or use a similar approach to get the real value
    //        }

    //        // Check if parameter is the Page, then get its BindingContext
    //        if (parameter is ContentPage page)
    //        {
    //            parameter = page.BindingContext;
    //        }

    //        if (parameter is UpcomingJobsWeekViewModel context)
    //        {
    //            var items = context.WeekDays;
    //            if ((items != null) && (value != null))
    //            {
    //                var item = (DateTime)value;
    //                int index = items.IndexOf(item);
    //                int lastIndex = items.Count - 1;
    //                if (index == 0)
    //                {
    //                    return new CornerRadius(10, 0, 0, 10); // First item
    //                }
    //                else if (index == lastIndex)
    //                {
    //                    return new CornerRadius(0, 10, 10, 0); // Last item
    //                }
    //                else
    //                {
    //                    return new CornerRadius(0); // Middle items
    //                }
    //            }
    //            else
    //            {
    //                return new CornerRadius(0); // Middle items
    //            }
    //        }
    //        else
    //        {
    //            Console.WriteLine($"ConverterParameter is of type {parameter.GetType().Name}, expected UpcomingJobsWeekViewModel");
    //        }

    //        return new CornerRadius(0);
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class CustomWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var windowWidth = Application.Current.Windows[Application.Current.Windows.Count - 1].Width;
            double totalWidth = (double)value;
            if (totalWidth > 0)
            {
                return (totalWidth + 0) / 7;// 7 columns for days
            }
            else
            {
                return (windowWidth + 0) / 7;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //public class DateSelectionConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (parameter is Binding binding)
    //        {
    //            // Get the source (the page) and its BindingContext
    //            if (binding.Source is ContentPage page)
    //            {
    //                var viewModel = page.BindingContext as UpcomingJobsWeekViewModel;
    //                if (viewModel != null)
    //                {
    //                    parameter = viewModel.SelectedDays;
    //                }
    //            }
    //        }

    //        if (value is DateTime currentDate && parameter is ObservableRangeCollection<DayOfWeek> selectedDates)
    //        {
    //            if (selectedDates.Contains(currentDate.DayOfWeek))
    //            {
    //                return Color.FromRgb(103, 80, 164);
    //            }
    //        }
    //        return Colors.Transparent; // Default color

    //        //return Colors.LightGray;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class DateSelectionConverterText : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (parameter is Binding binding)
    //        {
    //            // Get the source (the page) and its BindingContext
    //            if (binding.Source is ContentPage page)
    //            {
    //                var viewModel = page.BindingContext as UpcomingJobsWeekViewModel;
    //                if (viewModel != null)
    //                {
    //                    parameter = viewModel.SelectedDays;
    //                }
    //            }
    //        }

    //        if (value is DateTime currentDate && parameter is ObservableRangeCollection<DayOfWeek> selectedDates)
    //        {
    //            if (selectedDates.Contains(currentDate.DayOfWeek))
    //            {
    //                //"{dx:ThemeColor Primary}"
    //                return Colors.White;
    //                //return Colors.DarkGray;
    //            }
    //        }
    //        return Color.FromRgb(103, 80, 164);  // Default color
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class WidthDivisorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double totalWidth = (double)value;
            return totalWidth / 7; // 7 columns for days
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var theValue = ((string)value);
            return string.IsNullOrWhiteSpace(theValue) ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GridRowHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var theValue = ((string)value);
            return string.IsNullOrWhiteSpace(theValue) ? 0 : 5;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //public class GridRowHeightConverterAuto : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        var theValue = (string)value;
    //        return string.IsNullOrWhiteSpace(theValue) ? 0 : Auto;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class NewRowVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var theValue = (bool)value;

            //bool x;

            //if(!Boolean.TryParse((string)value, out x))
            //{
            //    x = false;
            //}

            //return theValue ? FontAttributes.Italic : FontAttributes.None;
            //return theValue ? Color.Red : Color.Black;
            return theValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ExistingRowVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var theValue = (bool)value;

            //bool x;

            //if(!Boolean.TryParse((string)value, out x))
            //{
            //    x = false;
            //}

            //return theValue ? FontAttributes.Italic : FontAttributes.None;
            //return theValue ? Color.Red : Color.Black;
            return !theValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StripRoleExtension : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return value;

            var theValue = ((string)value);

            int index = theValue.LastIndexOf("-");
            if (index >= 0)
                theValue = theValue.Substring(0, index); // or index + 1 to keep -

            return theValue.Trim();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //public class ImageSourceConverter : IValueConverter
    //{
    //    static WebClient Client = new WebClient();

    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value == null)
    //            return null;

    //        var byteArray = Client.DownloadData(value.ToString());
    //        return ImageSource.FromStream(() => new MemoryStream(byteArray));
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class ImageSourceConverter : IValueConverter
    {
        private static readonly HttpClient Client = new HttpClient();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string url = value.ToString();

            try
            {
                byte[] byteArray = Task.Run(() => DownloadDataAsync(url)).Result;
                return ImageSource.FromStream(() => new MemoryStream(byteArray));
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log or return a default image)
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        private async Task<byte[]> DownloadDataAsync(string url)
        {
            HttpResponseMessage response = await Client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            else
            {
                // Handle non-successful response (e.g., log or return a default image)
                Console.WriteLine($"HTTP error: {response.StatusCode}");
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
            //throw new NotImplementedException();
        }
    }

    public class DateTimeToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime == DateTime.MinValue;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //public class MultiBooleanConverter : IValueConverter
    //{
    //    object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        // Implement your Boolean expression here using the values from the three properties
    //        // For example, if you want to show the control when all three properties are true:
    //        bool result = (bool)values[0] && (bool)values[1] && (bool)values[2];
    //        return result;

    //    }

    //    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class DayWithoutLeadingZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                return date.Day.ToString(); // This will not add leading zeros
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
