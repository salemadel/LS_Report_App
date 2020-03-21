using LS_Report.Custom_Controls;
using LS_Report.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomListView), typeof(CustomListViewRenderer))]

namespace LS_Report.Droid.Renderers
{
    internal class CustomListViewRenderer : ListViewRenderer
    {
        public CustomListViewRenderer(Android.Content.Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;

            if (Control != null)
            {
                Control.NestedScrollingEnabled = false;
            }
        }
    }
}