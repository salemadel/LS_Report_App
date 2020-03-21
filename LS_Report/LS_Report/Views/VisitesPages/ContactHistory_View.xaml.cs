using LS_Report.Models;
using LS_Report.ViewModels.Visites_ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.VisitesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactHistory_View : ContentPage
    {
        public ContactHistory_View(ContactVisiteHistory_Model contact)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            BindingContext = new ContactHistory_ViewModel(Navigation, contact);
        }
    }
}