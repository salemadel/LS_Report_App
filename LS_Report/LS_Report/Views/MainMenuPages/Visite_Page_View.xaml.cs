using LS_Report.ViewModels.MainMenu_ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.MainMenuPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Visite_Page_View : ContentPage
    {
        public Visite_Page_View()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            BindingContext = new VisitePage_ViewModel(Navigation);
        }
    }
}