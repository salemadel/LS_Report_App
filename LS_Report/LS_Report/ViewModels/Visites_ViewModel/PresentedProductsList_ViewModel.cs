using LS_Report.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace LS_Report.ViewModels.Visites_ViewModel
{
    public class PresentedProductsList_ViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<tmp_presented_products_doctor> tmp_Presented_Products_Doctor { get; set; }

        public ObservableCollection<tmp_presented_products_doctor> Tmp_Presented_Products_Doctor
        {
            get { return tmp_Presented_Products_Doctor; }
            set
            {
                if (tmp_Presented_Products_Doctor != value)
                    tmp_Presented_Products_Doctor = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<tmp_pretented_products_pharmacy> tmp_Pretented_Products_Pharmacies { get; set; }

        public ObservableCollection<tmp_pretented_products_pharmacy> Tmp_Pretented_Products_Pharmacies
        {
            get { return tmp_Pretented_Products_Pharmacies; }
            set
            {
                if (tmp_Pretented_Products_Pharmacies != value)
                    tmp_Pretented_Products_Pharmacies = value;
                OnPropertyChanged();
            }
        }

        private bool isVisible { get; set; }

        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (isVisible != value)
                    isVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsHistory { get; set; }
        public tmp_presented_products_doctor Selected_Doctor { get; set; }
        public tmp_pretented_products_pharmacy Selected_Pharmacy { get; set; }

        public Command DeleteItem { get; set; }

        public PresentedProductsList_ViewModel(ObservableCollection<tmp_presented_products_doctor> tmp_Presented_Products_Doctors, ObservableCollection<tmp_pretented_products_pharmacy> tmp_Pretented_Products_Pharmacies, bool ishistory)
        {
            Tmp_Presented_Products_Doctor = tmp_Presented_Products_Doctors;
            Tmp_Pretented_Products_Pharmacies = tmp_Pretented_Products_Pharmacies;
            IsHistory = ishistory;
            if (tmp_Presented_Products_Doctors != null)
            {
                IsVisible = true;
                DeleteItem = new Command(() =>
                {
                    ExecuteOnDeletePresentedProductDoctor();
                });
            }
            else
            {
                DeleteItem = new Command(() =>
                {
                    ExecuteOnDeletePresentedProductParmacy();
                });
            }
        }

        private void ExecuteOnDeletePresentedProductDoctor()
        {
            if (Selected_Doctor != null)
            {
                Tmp_Presented_Products_Doctor.Remove(Selected_Doctor);
                MessagingCenter.Send(this, "DeletePresentedDoctor", Selected_Doctor);
                Selected_Doctor = null;
            }
        }

        private void ExecuteOnDeletePresentedProductParmacy()
        {
            if (Selected_Pharmacy != null)
            {
                Tmp_Pretented_Products_Pharmacies.Remove(Selected_Pharmacy);
                MessagingCenter.Send(this, "DeletePresentedPharmacy", Selected_Pharmacy);
                Selected_Pharmacy = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}