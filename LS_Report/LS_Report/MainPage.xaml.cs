using LS_Report.ViewModels;
using Xamarin.Forms;

namespace LS_Report
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            BindingContext = new MainMenu_ViewModel();
        }
    }
}