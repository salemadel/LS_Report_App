using LS_Report.Models;
using LS_Report.ViewModels.Profile_ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.ProfilePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile_View : ContentPage
    {
        public Profile_View(Token_Model token)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            BindingContext = new Profile_ViewModel(token);
        }
    }
}