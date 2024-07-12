//using StaffApp.Services.DataService;
using CollectionViewFilteringUI.Utils;
using DevExpress.Maui.Editors;
// using Microsoft.AppCenter.Analytics;
using StaffApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Maui.CommunityToolkit.Extensions;
// using Xamarin.Forms;
//using Xamarin.Forms.Xaml;
using DevExpress.Data;
using System.Linq.Expressions;

namespace StaffApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpcomingJobsPage : ContentPage
    {
        UpcomingJobsViewModel _viewModel;

        public string[] Weekdays = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

        public string _FilterString = "";

        public UpcomingJobsPage()
        {
            InitializeComponent();

            //UpcomingJobsListView.BindingContext = DataService.UpcomingJobsList;

            BindingContext = _viewModel = new UpcomingJobsViewModel();

            //this.filtersChipGroup.ChipIconSize = new Microsoft.Maui.Graphics.Size(0,0);
            //this.filtersChipGroup.ChipIsIconVisible = false;
            //this.filtersChipGroup.ChipIsIconVisible = true;
            //this.filtersChipGroup.ChipIconIndent = 0;
            //this.filtersChipGroup.ChipCheckIcon = null;

            //this.filtersChipGroup.ChipContentTemplate = new DataTemplate { }

            //this.UpcomingJobsListView.

            //DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = 


            //Analytics.TrackEvent("Page Opened",
            //                new Dictionary<string, string>() { { "Page", nameof(UpcomingJobsPage) } });
        }

        //protected override void OnAppearing()
        //{
        //    IsBusy = false;

        //    base.OnAppearing();

        //    _viewModel.OnAppearing();
        //}

        //private async void Button_Clicked(object sender, EventArgs e)
        //{
        //    _viewModel.ExecuteLoadItemsCommand(true);
        //}

        EnumToDescriptionConverter EnumToDescriptionConverter { get; } = new EnumToDescriptionConverter();

        void OnCustomDisplayText(object sender, FilterElementCustomDisplayTextEventArgs e)
        {
            //e.DisplayText = EnumToDescriptionConverter.Convert(e.Value).ToString();
            e.DisplayText = Weekdays[Convert.ToInt32(e.Value) - 1];
        }

        private void OnFilteringUIFormShowing(object sender, DevExpress.Maui.Core.FilteringUIFormShowingEventArgs e)
        {
            if (_viewModel.IsBusy)
            {
                e.Cancel = true;
                return;
            };

            if (e.Form is not ContentPage page)
                return;
            page.Title = "Filters";
        }

        //private void RemoveFilter()
        //{
        //    this.UpcomingJobsListView.BeginUpdate();

        //    _FilterString = this.UpcomingJobsListView.FilterString;

        //    this.UpcomingJobsListView.FilterString = "";

        //    //this.UpcomingJobsListView.FilteringUITemplate = null;

        //    this.UpcomingJobsListView.ItemsSource = null;

        //    //DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
        //}

        //private void ApplyFilter()
        //{
        //    //this.UpcomingJobsListView.ItemsSource = x;

        //    this.UpcomingJobsListView.ItemsSource = _viewModel.UpcomingJobsList;

        //    this.UpcomingJobsListView.EndUpdate();

        //    this.UpcomingJobsListView.FilterString = _FilterString;

        //    //this.UpcomingJobsListView.FilteringUITemplate = this.fil
        //}

        private void UpcomingJobsListView_LoadMore(object sender, EventArgs e)
        {
            //RemoveFilter();

            try
            {
                _viewModel.LoadMoreCommand.Execute(this);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
            finally
            {
                //ApplyFilter();
            }
        }

        private void DXButton_Pressed(object sender, EventArgs e)
        {
            //RemoveFilter();

            try
            {
                _viewModel.LoadItemsCommand.Execute(this);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
            finally
            {
                //ApplyFilter();
            }
        }

        //private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        //{
        //    await DisplayAlert("Alert", "You have been alerted", "OK");

        //    //this.UpcomingJobsListView.BindingContext
        //    //var this.UpcomingJobsListView.GetItem(this.UpcomingJobsListView.han)

        //    //UpcomingJobsViewModel.AddToFavorites(null);
        //}

        //private void Datepicker_DateSelected(object sender, Syncfusion.Maui.Pickers.DateChangedEventArgs e)
        //{
        //    StaffApp.Common.Library.Utils.DisplayAlert("DateChanged", "NewDate: " + e.NewValue + "\n" + "OldDate: " + e.OldValue, "Ok");
        //}

        //protected override void OnDisappearing()
        //{


        //    base.OnDisappearing();

        //    //_viewModel.OnDisappearing();
        //}

        //async Task testAsync()
        //{
        //    await this.DisplayToastAsync("Request to work sent.",3000);
        //}

    }
}