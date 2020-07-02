using Android.Content;
using Android.Locations;
using LS_Report.Droid.Interfaces;
using LS_Report.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(SettingService))]

namespace LS_Report.Droid.Interfaces
{
    internal class SettingService : ISettingService
    {
        public void OpenSettings()
        {
            if (!Xamarin.Forms.Forms.IsInitialized)
            {
                return;
            }
            LocationManager LM = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);

            if (LM.IsProviderEnabled(LocationManager.GpsProvider) == false)
            {
                Context ctx = Forms.Context;
                ctx.StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
            }
            else
            {
                //this is handled in the PCL
            }
        }
    }
}