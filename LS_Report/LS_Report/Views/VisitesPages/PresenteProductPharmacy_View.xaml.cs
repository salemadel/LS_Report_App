using LS_Report.Models;
using LS_Report.ViewModels.Visites_ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.VisitesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PresenteProductPharmacy_View : ContentPage
    {
        public PresenteProductPharmacy_View(Products_Model product)
        {
            InitializeComponent();
            BindingContext = new PresenteProductPharmacy_ViewModel(Navigation, product);
        }
    }
}