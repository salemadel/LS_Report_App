using LS_Report.Data;
using LS_Report.Interfaces;
using LS_Report.ViewModels.MainMenu_ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.MainMenuPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Sync_Page_View : ContentPage
    {
        public Sync_Page_View()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            var datastore = new DataStore();
            BindingContext = new Sync_ViewModel(Navigation, datastore);
        }
    }
}