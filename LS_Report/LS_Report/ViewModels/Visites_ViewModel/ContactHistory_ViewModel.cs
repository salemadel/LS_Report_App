using LS_Report.Models;
using LS_Report.Views.VisitesPages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.ViewModels.Visites_ViewModel
{
    public class ContactHistory_ViewModel : INotifyPropertyChanged
    {
        private INavigation Navigation { get; set; }

        private bool isBusy { get; set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy != value)
                    isBusy = value;
                OnPropertyChanged();
            }
        }

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

        public bool HasHistory { get; set; }
        public string Contact_Name { get; set; }

        public Command PresentedProductsListCommand { get; set; }
        public ContactVisiteHistory_Model Contact { get; set; }

        public ContactHistory_ViewModel(INavigation navigation, ContactVisiteHistory_Model contact)
        {
            Navigation = navigation;
            Contact = contact;
            PresentedProductsListCommand = new Command(async () =>
            {
                await ExecuteOnPresenteProductsList();
            });

            Initialize();
        }

        private void Initialize()
        {
            if (Contact != null & Contact.authority != null)
            {
                Tmp_Presented_Products_Doctor = new ObservableCollection<tmp_presented_products_doctor>();
                Tmp_Pretented_Products_Pharmacies = new ObservableCollection<tmp_pretented_products_pharmacy>();
                HasHistory = true;
                Contact_Name = "Historique : " + Contact.client.lastname + " " + Contact.client.firstname + ".";
                if (Contact.products_presented != null && Contact.products_presented.Count > 0)
                {
                    if (Contact.products_presented[0].doctor != null)
                    {
                        foreach (var presented in Contact.products_presented)
                        {
                            tmp_presented_products_doctor tmp = new tmp_presented_products_doctor();
                            tmp.product = presented.product.name;
                            tmp.prescribed = presented.doctor.prescribed;
                            tmp.no_interessed_reason = presented.doctor.no_interessed_reason;
                            tmp.no_prescribed_reason = presented.doctor.no_prescribed_reason;
                            tmp.prescribed_frequency = presented.doctor.prescription_frequency;
                            tmp.samples_delivred = presented.doctor.samples;
                            tmp.rival_product = presented.doctor.rival_product;
                            Tmp_Presented_Products_Doctor.Add(tmp);
                        }
                    }
                    else
                    {
                        foreach (var presented in Contact.products_presented)
                        {
                            tmp_pretented_products_pharmacy tmp = new tmp_pretented_products_pharmacy();
                            tmp.product = presented.product.name;
                            tmp.available = presented.pharmacy.available;
                            tmp.refusal_reason = presented.pharmacy.refusal_reason;
                            tmp.rival_product = presented.pharmacy.rival_product;
                            tmp.rotation = presented.pharmacy.rotation;
                            tmp.stock = presented.pharmacy.stock;
                            tmp.unavailability_reason = presented.pharmacy.unavailability_reason;
                            Tmp_Pretented_Products_Pharmacies.Add(tmp);
                        }
                    }
                }
            }
            else
            {
                Contact_Name = "Pas d'historique.";
                HasHistory = false;
            }
        }

        private async Task ExecuteOnPresenteProductsList()
        {
            IsBusy = true;
            if (Tmp_Presented_Products_Doctor.Count > 0)
            {
                await Navigation.PushModalAsync(new ProdcutPresentedList_View(Tmp_Presented_Products_Doctor, null, true), true);
            }
            else
            {
                if (Tmp_Pretented_Products_Pharmacies.Count > 0)
                    await Navigation.PushModalAsync(new ProdcutPresentedList_View(null, Tmp_Pretented_Products_Pharmacies, true), true);
            }
            IsBusy = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}