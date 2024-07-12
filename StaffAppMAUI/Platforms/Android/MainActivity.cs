using Android.App;
using Android.Content.PM;
using Shiny;
using Shiny.Push;

namespace StaffApp.Platforms.Android
{
    //[IntentFilter(new[] {Shiny.ShinyPushIntents.NotificationClickAction}, Categories = new[] {"android.intent.category.DEFAULT"})]
    [IntentFilter(
        new[] {
            ShinyPushIntents.NotificationClickAction,
            Platform.Intent.ActionAppAction,
            global::Android.Content.Intent.ActionView
        },
        Categories = new[] {
            "android.intent.category.DEFAULT",
            global::Android.Content.Intent.CategoryDefault,
            global::Android.Content.Intent.CategoryBrowsable,
        }
    )]
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {

    }
}