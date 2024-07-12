using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Sentry;
using Sentry.Protocol;
using StaffApp.Services;
using StaffApp.ViewModels;
using StaffApp.Views;
using System;
using System.Linq.Expressions;

namespace StaffApp
{
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            InitializeComponent();

            Current.MainPage = new AppShell();

            _ = PushRegistration.CheckPermission();

            //DependencyService.Register<NavigationService>();
            //Routing.RegisterRoute(typeof(UpcomingJobsPage).FullName, typeof(UpcomingJobsPage));
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                System.Exception ex = (System.Exception)e.ExceptionObject;

                SentrySdk.CaptureException(ex);

            }
            catch (Exception ex)
            {
                    SentrySdk.CaptureException(ex);
                }
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            try
            {
                System.Exception ex = (System.Exception)e.Exception;

                SentrySdk.CaptureException(ex);

                e.SetObserved();
            }
            catch (Exception ex)
            {
                    SentrySdk.CaptureException(ex);
                }
        }

        protected override void OnStart()
        {
            base.OnStart();


        }

    }
}
