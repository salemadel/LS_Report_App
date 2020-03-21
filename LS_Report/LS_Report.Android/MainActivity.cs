using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using LS_Report.Droid.Interfaces;
using LS_Report.Droid.Services;
using Plugin.CurrentActivity;
using Plugin.LocalNotifications;

namespace LS_Report.Droid
{
    [Activity(Label = "LS Report", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop) ]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjAzMzQyQDMxMzcyZTM0MmUzMGswQXNCTFRGMzBaQVRsNm82N3RUT3N0QmVDa0tqYWxuVy9EUWpKeFNZM2c9");
            
            
            Xamarin.Forms.DependencyService.Register<ToastMessage>();
            LoadApplication(new App());
            Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Android.Graphics.Color.Rgb(99, 110, 114));
            var intent = new Intent(this, typeof(BackGroundService));
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            ContextCompat.StartForegroundService(this, intent);
            LocalNotificationsImplementation.NotificationIconId = Resource.Drawable.icon;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}