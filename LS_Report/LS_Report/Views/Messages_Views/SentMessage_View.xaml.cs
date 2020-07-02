using LS_Report.Data;
using LS_Report.ViewModels.Messages_ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.Messages_Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SentMessage_View : ContentPage
    {
        public SentMessage_View()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            var datastore = new DataStore();
            BindingContext = new SentMessage_ViewModel(datastore);
        }
    }
}