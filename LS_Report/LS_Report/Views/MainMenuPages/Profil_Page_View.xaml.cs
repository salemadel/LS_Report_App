using LS_Report.Data;
using LS_Report.ViewModels.MainMenu_ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.MainMenuPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profil_Page_View : ContentPage
    {
        public Profil_Page_View()
        {
            InitializeComponent();
            var datastore = new DataStore();
            BindingContext = new ProfilePage_ViewModdel(Navigation, datastore);
            if ((BindingContext as ProfilePage_ViewModdel).IsFreeMission)
            {
                Disconnect_Button.SetValue(Grid.RowProperty, 4);
            }
            else
            {
                Disconnect_Button.SetValue(Grid.RowProperty, 3);
            }
        }
    }
}