using LS_Report.Data;
using LS_Report.ViewModels.Messages_ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.Messages_Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecevedMessage_View : ContentPage
    {
        public RecevedMessage_View()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            var datastore = new DataStore();
            BindingContext = new ReceivedMessage_ViewModel(datastore);
        }
    }
}