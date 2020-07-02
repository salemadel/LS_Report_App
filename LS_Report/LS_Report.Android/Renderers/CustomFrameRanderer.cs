using Android.Views;
using LS_Report.Custom_Controls;
using LS_Report.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomFrame), typeof(CustomFrameRanderer))]

namespace LS_Report.Droid.Renderers
{
    public class CustomFrameRanderer : FrameRenderer
    {
        public CustomFrameRanderer(Android.Content.Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                ViewGroup.SetBackgroundResource(Resource.Drawable.Shadow);
            }
        }
    }
}