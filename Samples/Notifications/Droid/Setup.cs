using System;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Cheesebaron.MvxPlugins.Notifications;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using Notifications.Sample.Core;

namespace Notifications.Sample.Droid
{
    public class Setup 
        : MvxAndroidSetup
    {
        public Setup(Context applicationContext) 
            : base(applicationContext) { }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override IMvxPluginConfiguration GetPluginConfiguration(Type plugin)
        {
            if (plugin == typeof(PluginLoader))
            {
                return new DroidNotificationConfiguration
                {
                    // Id of the Application in the Google Console
                    SenderIds = new[] { "771992631451" }
                };
            }

            return base.GetPluginConfiguration(plugin);
        }
    }

    /// <summary>
    /// This is where you handle your notifications, you cannot use any MvvmCross specific things in here.
    /// You can call Android services, content providers etc. but you cannot be sure that your application
    /// is alive when this is being called.
    /// </summary>
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { Constants.IntentFilter }, Categories = new[] { Constants.Category })]
    public class MyNotificationBroadcastReceiver 
        : BaseNotificationBroadcastReceiver
    {
        public override void ProcessNotification(Context context, string notificationType, string json)
        {
            var manager =
                    (NotificationManager)context.GetSystemService(Context.NotificationService);

            var intent =
                context.PackageManager.GetLaunchIntentForPackage(context.PackageName);
            intent.AddFlags(ActivityFlags.ClearTop);

            var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.UpdateCurrent);

            var builder = new NotificationCompat.Builder(context)
                .SetSmallIcon(Android.Resource.Drawable.StarBigOn)
                .SetContentTitle("Boop!")
                .SetStyle(new NotificationCompat.BigTextStyle().BigText(json))
                .SetContentText(json)
                .SetContentIntent(pendingIntent);

            manager.Notify(1, builder.Build());
        }
    }
}