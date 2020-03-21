using LS_Report.Data;
using LS_Report.Interfaces;
using LS_Report.Models;
using LS_Report.ViewModels.Contacts_ViewModels;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.ContactsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Contatcs_View : ContentPage
    {
        public Contatcs_View(string source,Token_Model token ,  List<Wilaya_Model> wilayas, List<Commune> communes, List<string> speciality)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            var datastore = new DataStore();
            BindingContext = new Contacts_ViewModel(Navigation, datastore,token, source, wilayas, communes, speciality);
        }
    }
}