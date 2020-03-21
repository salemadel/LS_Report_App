using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Picture_View : ContentPage
    {
        public Picture_View(ImageSource source)
        {
            InitializeComponent();
            Picture.Source = source;
        }
    }
}