using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Shiny;
using Shiny.Extensions.Push;
using Shiny.Hosting;
using Shiny.Push;
using StaffApp;

public static class PushRegistration
{
    public static async Task CheckPermission()
    {
        var push = Host.Current.Services.GetService<Shiny.Push.IPushManager>();

        var result = await push.RequestAccess();

        if (result.Status == AccessState.Available)
        {
            var token = result.RegistrationToken;

            Debug.WriteLine("Token: " + token);

            Preferences.Set("PushNotificationsToken", token);

            if (push.Tags != null)
            {
                await push.Tags.ClearTags();

                await push.Tags.SetTags("all");
            }

        }
    }
}