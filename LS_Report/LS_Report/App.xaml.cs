using LS_Report.Data;
using LS_Report.Views;
using Xamarin.Forms;

namespace LS_Report
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mjc5NDMzQDMxMzgyZTMxMmUzMER2bTdGc1JRSWNqTHdHRjJCcDYraEFiMSs5WnNoUStHM2I4Q1hSdlBOYTg9");
            InitializeComponent();
            var tokenController = new TokenController();
            if (tokenController.Token_Expired())
                MainPage = new NavigationPage(new Login_View());
            else
                MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}