using LS_Report.Models;
using LS_Report.ViewModels.Visites_ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.VisitesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetail_View : ContentPage
    {
        public ProductDetail_View(Products_Model product)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            BindingContext = new ProductDetail_ViewModel(Navigation, product);
        }
    }
}