using LS_Report.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.ViewModels.Visites_ViewModel
{
    public class PresenteProductPharmacy_ViewModel : INotifyPropertyChanged
    {
        private INavigation Navigation { get; set; }
        private string[] rotation = { "0-10/sem", "10-20/sem", "> 20/sem" };
        private string[] uvailability_reasons = { "Non connu", "Non disponible chez le fournisseur", "Non prescrit", "Non renouvelé / Refus de commander", "Renouvellement en cours" };
        private string[] refusal_reasons = { "Concurrence", "Produit cher / Psychotrope", "remises basses", "Rotation faible" };
        private string[] reorder = { "Oui", "Non" };

        public List<string> Rotation
        {
            get { return rotation.ToList(); }
        }

        public List<string> Uvailability_reasons
        {
            get { return uvailability_reasons.ToList(); }
        }

        public List<string> Refusal_reasons
        {
            get { return refusal_reasons.ToList(); }
        }

        public List<string> Reorder
        {
            get { return reorder.ToList(); }
        }

        private string selected_rotation { get; set; }

        public string Selected_Rotation
        {
            get { return selected_rotation; }
            set
            {
                if (selected_rotation != value)
                    selected_rotation = value;
                OnPropertyChanged();
            }
        }

        private string selected_unvailibility { get; set; }

        public string Selected_Unvailibility
        {
            get { return selected_unvailibility; }
            set
            {
                if (selected_unvailibility != value)
                {
                    if (value == "Non renouvelé / Refus de commander")
                    {
                        Refusal_Visible = true;
                        OnPropertyChanged("Refusal_Visible");
                    }
                    else
                    {
                        Refusal_Visible = false;
                        Selected_Refusal = null;
                        OnPropertyChanged("Refusal_Visible");
                    }
                    if (value == "Renouvellement en cours")
                    {
                        Rotation_Visible = true;
                        OnPropertyChanged("Rotation_Visible");
                    }
                    else
                    {
                        Rotation_Visible = false;
                        Selected_Rotation = null;
                        OnPropertyChanged("Rotation_Visible");
                    }
                    selected_unvailibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private string selected_refusal { get; set; }

        public string Selected_Refusal
        {
            get { return selected_refusal; }
            set
            {
                if (selected_refusal != value)
                    selected_refusal = value;
                OnPropertyChanged();
            }
        }

        private string selected_reorder { get; set; }

        public string Selected_Reorder
        {
            get { return selected_reorder; }
            set
            {
                if (selected_reorder != value)
                {
                    if (value == "Oui")
                    {
                        ReorderBool = true;
                    }
                    else
                    {
                        ReorderBool = false;
                    }
                    selected_reorder = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool availabel { get; set; }

        public bool Availabel
        {
            get { return availabel; }
            set
            {
                if (availabel != value)
                {
                    availabel = value;

                    if (value)
                    {
                        Selected_Unvailibility = null;
                        Selected_Refusal = null;
                        Rotation_Visible = value;
                        OnPropertyChanged("Rotation_Visible");
                    }

                    OnPropertyChanged();
                }
            }
        }

        private bool not_availabel { get; set; }

        public bool Not_Availabel
        {
            get { return not_availabel; }
            set
            {
                if (not_availabel != value)
                {
                    not_availabel = value;
                    if (value)
                    {
                        Selected_Rotation = null;
                        Rotation_Visible = !value;
                        OnPropertyChanged("Rotation_Visible");
                    }
                    OnPropertyChanged();
                }
            }
        }

        private bool ReorderBool { get; set; }
        public bool Rotation_Visible { get; set; }
        public bool Refusal_Visible { get; set; }
        public int Stock { get; set; }
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

        public tmp_pretented_products_pharmacy Tmp_Pretented_Products_Pharmacy { get; set; }
        public Products_Model Product { get; set; }

        public Command AddPresentePorodctCommand { get; set; }

        public PresenteProductPharmacy_ViewModel(INavigation navigation, Products_Model product)
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
            Tmp_Pretented_Products_Pharmacy = new tmp_pretented_products_pharmacy();
            Tmp_Pretented_Products_Pharmacy.available = Availabel;
            Tmp_Pretented_Products_Pharmacy.id_product = Product._id;
            Tmp_Pretented_Products_Pharmacy.product = Product.name;
            Tmp_Pretented_Products_Pharmacy.refusal_reason = Selected_Refusal;
            Tmp_Pretented_Products_Pharmacy.reorder = ReorderBool;
            Tmp_Pretented_Products_Pharmacy.rotation = Selected_Rotation;
            Tmp_Pretented_Products_Pharmacy.stock = Stock;
            Tmp_Pretented_Products_Pharmacy.unavailability_reason = Selected_Unvailibility;
            MessagingCenter.Send(this, "UpdatePresentedPharmacy", Tmp_Pretented_Products_Pharmacy);
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