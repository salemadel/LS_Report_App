using LS_Report.Models;
using LS_Report.ViewModels.Visites_ViewModel;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.VisitesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProdcutPresentedList_View : ContentPage
    {
        public ProdcutPresentedList_View(ObservableCollection<tmp_presented_products_doctor> tmp_Presented_Products_Doctors, ObservableCollection<tmp_pretented_products_pharmacy> tmp_Pretented_Products_Pharmacies, bool ishistory = false)
        {
            InitializeComponent();
            BindingContext = new PresentedProductsList_ViewModel(tmp_Presented_Products_Doctors, tmp_Pretented_Products_Pharmacies, ishistory);
        }
    }
}