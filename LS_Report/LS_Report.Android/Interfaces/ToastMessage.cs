using Android.App;
using Android.Widget;
using LS_Report.Droid.Interfaces;
using LS_Report.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(ToastMessage))]

namespace LS_Report.Droid.Interfaces
{
    internal class ToastMessage : IMessage
    {
         
        public void LongAlert(string message)
        {
            if (!Xamarin.Forms.Forms.IsInitialized)
            {
                return;
            }
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            if (!Xamarin.Forms.Forms.IsInitialized)
            {
                return;
            }
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}