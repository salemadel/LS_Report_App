using LS_Report.Models;
using LS_Report.ViewModels.Visites_ViewModel;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.VisitesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Focus_View : ContentPage
    {
        public Focus_View(Token_Model token, List<Wilaya_Model> wilayas, List<Commune> communes, List<string> speciality)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            BindingContext = new Focus_ViewModel(Navigation, token, wilayas, communes, speciality);
        }
    }
}