using LS_Report.Models;
using LS_Report.ViewModels.Visites_ViewModel;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.VisitesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Statistics_View : ContentPage
    {
        public Statistics_View(List<ContactVisiteHistory_Model> contacts)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            BindingContext = new Statistics_ViewModel(contacts);
        }
    }
}