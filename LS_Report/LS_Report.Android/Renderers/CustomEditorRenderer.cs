using LS_Report.Custom_Controls;
using LS_Report.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]

namespace LS_Report.Droid.Renderers
{
    public class CustomEditorRenderer : EditorRenderer
    {
        public CustomEditorRenderer(Android.Content.Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;
            }
        }
    }
}