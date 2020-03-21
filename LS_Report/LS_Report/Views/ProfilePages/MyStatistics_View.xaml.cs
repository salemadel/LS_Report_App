using LS_Report.Models;
using LS_Report.ViewModels.Profile_ViewModels;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.ProfilePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyStatistics_View : ContentPage
    {
        public MyStatistics_View(List<Stats_Model> stats, Token_Model token)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            BindingContext = new MyStatistics_ViewModel(stats, token);
        }
    }
}