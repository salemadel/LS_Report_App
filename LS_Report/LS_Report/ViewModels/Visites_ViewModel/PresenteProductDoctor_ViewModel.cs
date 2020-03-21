using LS_Report.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.ViewModels.Visites_ViewModel
{
    public class PresenteProductDoctor_ViewModel : INotifyPropertyChanged
    {
        private INavigation Navigation { get; set; }
        private string[] prescribed_list = { "Fréquente", "Moyenne", "Rare" };
        private string[] no_prescribed_list = { "Non présenté", "Non intéressé", "Oublier" };
        private string[] interessed_list = { "Antécédents d'effets indisérable", "Contre les génériques", "Non concerné par le produit", "Non disponible en offcine", "Produit concurrent" };

        public List<string> Prescribed_list
        {
            get { return prescribed_list.ToList(); }
        }

        public List<string> No_prescribed_list
        {
            get { return no_prescribed_list.ToList(); }
        }

        public List<string> Interessed_list
        {
            get { return interessed_list.ToList(); }
        }

        private string selected_prescribed { get; set; }

        public string Selected_Prescribed
        {
            get { return selected_prescribed; }
            set
            {
                if (selected_prescribed != value)
                    selected_prescribed = value;
                OnPropertyChanged();
            }
        }

        private string selected_no_prescribed { get; set; }

        public string Selected_No_Prescribed
        {
            get { return selected_no_prescribed; }
            set
            {
                if (selected_no_prescribed != value)
                    if (value == "Non intéressé")
                    {
                        No_Interssed_List_Visible = true;
                    }
                    else
                    {
                        No_Interssed_List_Visible = false;
                        Selected_Interessed = null;
                    }

                selected_no_prescribed = value;
                OnPropertyChanged();
                OnPropertyChanged("No_Interssed_List_Visible");
            }
        }

        private string selected_interessed { get; set; }

        public string Selected_Interessed
        {
            get { return selected_interessed; }
            set
            {
                if (selected_interessed != value)
                    selected_interessed = value;
                OnPropertyChanged();
            }
        }

        private bool prescribed { get; set; }

        public bool Prescribed
        {
            get { return prescribed; }
            set
            {
                if (prescribed != value)
                    if (value)
                    {
                        Selected_Interessed = null;
                        Selected_No_Prescribed = null;
                        No_Interssed_List_Visible = false;
                    }
                prescribed = value;

                OnPropertyChanged();
            }
        }

        private bool not_prescribed { get; set; }

        public bool Not_Prescribed
        {
            get { return not_prescribed; }
            set
            {
                if (not_prescribed != value)
                    not_prescribed = value;
                Selected_Prescribed = null;
                OnPropertyChanged();
            }
        }

        public bool No_Interssed_List_Visible { get; set; }

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

        public int Samples { get; set; }
        public tmp_presented_products_doctor Tmp_Presented_Products_Doctor { get; set; }
        public Products_Model Product { get; set; }

        public Command AddPresentePorodctCommand { get; set; }

        public PresenteProductDoctor_ViewModel(INavigation navigation, Products_Model product)
        {
            Navigation = navigation;
            Product = product;
            AddPresentePorodctCommand = new Command(async () =>
            {
                await ExecuteOnAddPresentedProduct();
            });
        }

        private async Task ExecuteOnAddPresentedProduct()
        {
            IsBusy = true;
            Tmp_Presented_Products_Doctor = new tmp_presented_products_doctor();
            Tmp_Presented_Products_Doctor.id_product = Product._id;
            Tmp_Presented_Products_Doctor.no_interessed_reason = Selected_Interessed;
            Tmp_Presented_Products_Doctor.no_prescribed_reason = Selected_No_Prescribed;
            Tmp_Presented_Products_Doctor.prescribed_frequency = Selected_Prescribed;
            Tmp_Presented_Products_Doctor.product = Product.name;
            Tmp_Presented_Products_Doctor.samples_delivred = Samples;
            Tmp_Presented_Products_Doctor.prescribed = Prescribed;
            MessagingCenter.Send(this, "UpdatePresentedDoctor", Tmp_Presented_Products_Doctor);
            await Navigation.PopModalAsync();
            IsBusy = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}