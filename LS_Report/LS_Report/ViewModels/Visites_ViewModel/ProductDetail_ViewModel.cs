using LS_Report.Models;
using LS_Report.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.ViewModels.Visites_ViewModel
{
    public class ProductDetail_ViewModel : INotifyPropertyChanged
    {
        private INavigation Navigation { get; set; }
        public Products_Model Product { get; set; }
        public Rival_product Selected_RivalProduct { get; set; }
        private bool popUpVisible { get; set; }

        public bool PopUpVisible
        {
            get { return popUpVisible; }
            set
            {
                if (popUpVisible != value)
                    popUpVisible = value;
                OnPropertyChanged();
            }
        }

        public Command PictureTappedCommand { get; set; }
        public Command RivalPictureTappedCommand { get; set; }
        public Command ItemTappedCommand { get; set; }
        public Command HidePopUpCommand { get; set; }

        public ProductDetail_ViewModel(INavigation navigation, Products_Model product)
        {
            Navigation = navigation;
            Product = product;
            PictureTappedCommand = new Command(async () =>
            {
                await ExecuteOnPictureTapped();
            });
            ItemTappedCommand = new Command(() =>
            {
                ExecuteOnRivalProductTapped();
            });
            HidePopUpCommand = new Command(() =>
            {
                ExecuteOnPopUpHide();
            });
            RivalPictureTappedCommand = new Command(async () =>
            {
                await ExecuteOnRivalPictureTapped();
            });
        }

        private async Task ExecuteOnPictureTapped()
        {
            if (!string.IsNullOrWhiteSpace(Product.avatar))
                await Navigation.PushModalAsync(new Picture_View(Product.Avatar));
        }

        private async Task ExecuteOnRivalPictureTapped()
        {
            if (!string.IsNullOrWhiteSpace(Selected_RivalProduct.avatar))
                await Navigation.PushModalAsync(new Picture_View(Product.Avatar));
        }

        private void ExecuteOnRivalProductTapped()
        {
            OnPropertyChanged("Selected_RivalProduct");
            PopUpVisible = true;
        }

        private void ExecuteOnPopUpHide()
        {
            PopUpVisible = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}