using LS_Report.Models;
using LS_Report.ViewModels.Visites_ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.VisitesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PresenteProductDoctro_View : ContentPage
    {
        public PresenteProductDoctro_View(Products_Model product)
        {
            InitializeComponent();
            BindingContext = new PresenteProductDoctor_ViewModel(Navigation, product);
        }
    }
}