using LS_Report.Data;
using LS_Report.Models;
using LS_Report.ViewModels.Visites_ViewModel;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.VisitesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GlobalMission_View : ContentPage
    {
        public GlobalMission_View(Token_Model token)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            var datastore = new DataStore();
            BindingContext = new GlobalMission_ViewModel(Navigation, datastore, token);
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            DatePicker.Focus();
        }
    }
}