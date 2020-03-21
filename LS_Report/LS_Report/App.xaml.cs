using LS_Report.Data;
using LS_Report.Views;
using Xamarin.Forms;

namespace LS_Report
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjAzMzQyQDMxMzcyZTM0MmUzMGswQXNCTFRGMzBaQVRsNm82N3RUT3N0QmVDa0tqYWxuVy9EUWpKeFNZM2c9");
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