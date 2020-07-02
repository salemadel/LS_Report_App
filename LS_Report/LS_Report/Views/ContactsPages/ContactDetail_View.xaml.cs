using LS_Report.Models;
using LS_Report.ViewModels.Contacts_ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.ContactsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactDetail_View : ContentPage
    {
        public ContactDetail_View(Client2 contact, Token_Model token)
        {
            InitializeComponent();
            BindingContext = new ContactDetail_ViewModel(Navigation, contact, token);
        }
    }
}