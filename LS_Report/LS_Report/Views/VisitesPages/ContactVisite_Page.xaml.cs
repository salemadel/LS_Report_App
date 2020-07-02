using LS_Report.Data;
using LS_Report.Models;
using LS_Report.ViewModels.Visites_ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.VisitesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactVisite_Page : ContentPage
    {
        public ContactVisite_Page(string source, Client2 contact, Token_Model token, bool isfreemission, bool CanVisite, string global_id = null, string mission_id = null)
        {
            InitializeComponent();
            var datastore = new DataStore();
            BindingContext = new ContactVisite_ViewModel(source, Navigation, datastore, token, contact, isfreemission, CanVisite, global_id, mission_id);
        }
    }
}