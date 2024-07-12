using DevExpress.Maui;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using Shiny;

namespace StaffApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var currentVersion = VersionTracking.CurrentVersion;
            var currentbuild = VersionTracking.CurrentBuild;

            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                .UseShiny()
                .UseDevExpress()
                .UseDevExpressCollectionView()
                .UseDevExpressControls()
                .UseDevExpressScheduler()
                .UseDevExpressHtmlEditor()
                .UseDevExpressEditors()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("ionicons-woff.ttf", "ionicons");

                    fonts.AddFont("Avenir Black.ttf", "AvenirBlack");
                    fonts.AddFont("Avenir Heavy.ttf", "AvenirHeavy");
                    fonts.AddFont("Avenir Regular.ttf", "AvenirRegular");

                    fonts.AddFont("Berthold Baskerville Bold.ttf", "BertholdBaskervilleBold");
                    fonts.AddFont("Berthold Baskerville Medium Italic.ttf", "BertholdBaskervilleMediumItalic");
                    fonts.AddFont("Berthold Baskerville Regular.ttf", "BertholdBaskervilleRegular");

                }).ConfigureEffects((effects) =>
                {
                    effects.AddCompatibilityEffects(AppDomain.CurrentDomain.GetAssemblies());
                })
                ;

            DevExpress.Maui.CollectionView.Initializer.Init();
            DevExpress.Maui.Controls.Initializer.Init();
            DevExpress.Maui.Scheduler.Initializer.Init();
            DevExpress.Maui.HtmlEditor.Initializer.Init();
            DevExpress.Maui.Editors.Initializer.Init();

            builder.Services.AddPush<PushDelegate>(
#if ANDROID
    new Shiny.Push.FirebaseConfig(
        false, // false to ignore embedded configuration
        "1:33676796911:android:b23148cba4619915e50c1a", // appid
        "33676796911", // sender id/project_number
        "hs-staff-app-maui-3-1", // project_id
        "AIzaSyA2im9mGEylGc-9nQWZXHiq_BFUStkLQs0" // api_key
    )
#endif
);
            builder.Services.AddPushFirebaseMessaging<PushDelegate>();

            return builder.Build();
        }
    }
}