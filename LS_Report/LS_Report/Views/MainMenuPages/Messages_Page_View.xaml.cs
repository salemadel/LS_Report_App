using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.MainMenuPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Messages_Page_View : TabbedPage
    {
        public Messages_Page_View()
        {
            // NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }
    }
}