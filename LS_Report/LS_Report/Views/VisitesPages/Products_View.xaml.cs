using LS_Report.Data;
using LS_Report.ViewModels.Visites_ViewModel;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.VisitesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Products_View : ContentPage
    {
        public Products_View(string source, List<string> ids = null)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            var datastore = new DataStore();
            BindingContext = new Products_ViewModel(Navigation, datastore, source, ids);
        }
    }
}