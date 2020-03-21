using LS_Report.Data;
using LS_Report.Interfaces;
using LS_Report.ViewModels.Visites_ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.VisitesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VisiteHistory_View : ContentPage
    {
        public VisiteHistory_View()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            var datastore = new DataStore();
            BindingContext = new VisiteHistory_ViewModel(Navigation, datastore);
        }
    }
}