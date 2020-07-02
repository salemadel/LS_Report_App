using LS_Report.Data;
using LS_Report.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login_View : ContentPage
    {
        public Login_View()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            var datastore = new DataStore();
            BindingContext = new Login_ViewModel(Navigation, datastore);
        }

        private async void CustomEntry_Focused(object sender, FocusEventArgs e)
        {
            await UserName_Frame.ScaleTo(1.2, 100);
        }

        private async void CustomEntry_Unfocused(object sender, FocusEventArgs e)
        {
            await UserName_Frame.ScaleTo(1, 100);
        }

        private async void CustomEntry_Focused_1(object sender, FocusEventArgs e)
        {
            await Password_Frame.ScaleTo(1.2, 100);
        }

        private async void CustomEntry_Unfocused_1(object sender, FocusEventArgs e)
        {
            await Password_Frame.ScaleTo(1, 100);
        }
    }
}