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
    public partial class EditContact_View : ContentPage
    {
        public EditContact_View(Token_Model token , List<Wilaya_Model> wilayas, List<Commune> communes, List<string> specialitys, Client2 contact)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            var datastore = new DataStore();
            BindingContext = new EditContact_ViewModel(Navigation, datastore, token , wilayas, communes, specialitys, contact);
        }
    }
}