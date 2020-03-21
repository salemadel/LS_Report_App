using LS_Report.Models;
using LS_Report.Views.ContactsPages;
using LS_Report.Views.VisitesPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.ViewModels.Visites_ViewModel
{
    public class Focus_ViewModel : INotifyPropertyChanged
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
        private ObservableCollection<Client2> contacts { get; set; }
        public ObservableCollection<Client2> Contacts
        {
            get { return contacts; }
            set
            {
                if (contacts != value)
                    contacts = value;
                OnPropertyChanged();
            }
        }
        private bool isVisible {get;set;}
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
        private Token_Model Token { get; set; }
        private List<Wilaya_Model> Wilaya { get; set; }
        private List<Commune> Commune { get; set; }
        private List<string> Speciality { get; set; }
        public Client2 Selected_Item { get; set; }

        public Command AddContactCommand { get; set; }
        public Command DeleteContactCommand { get; set; }
        public Command AddReportCommand { get; set; }
        public Focus_ViewModel(INavigation navigation , Token_Model token , List<Wilaya_Model> wilayas , List<Commune> communes , List<string> speciality)
        {
            Navigation = navigation;
            Token = token;
            Wilaya = wilayas;
            Commune = communes;
            Speciality = speciality;
            Contacts = new ObservableCollection<Client2>();
            AddContactCommand = new Command(async () =>
            {
                await ExecuteOnAddContact();
            });
            DeleteContactCommand = new Command(() =>
            {
                ExecuteOnDeleteContact();
            });
            AddReportCommand = new Command(async () =>
            {
                await ExecuteOnAddReport();
            });
            MessagingCenter.Subscribe<ContactVisite_ViewModel, Client2>(this, "ContactFocus", (sender, args) =>
          {
              Contacts.Add(args);
              IsVisible = true;
          });
            MessagingCenter.Subscribe<Visite_ViewModel>(this,"PopModalAsync" , async(sender) =>
                {
                    
                    await Navigation.PopAsync();
            });
               
        }
        private async Task ExecuteOnAddContact()
        {
            await Navigation.PushModalAsync(new Contatcs_View("Focus", Token, Wilaya, Commune, Speciality) , true);
        }
        private void ExecuteOnDeleteContact()
        {
            if(Selected_Item != null)
            {
                Contacts.Remove(Selected_Item);
                Selected_Item = null;
                if(Contacts.Count == 0)
                {
                    IsVisible = false;
                }
            }
        }
        private async Task ExecuteOnAddReport()
        {
            if(Contacts != null & Contacts.Count > 0 & Contacts.Where(i => i.business_type == "Pharmacien" | i.business_type == "Grossiste").Count() == 0)
            {
                await Navigation.PushModalAsync(new Visite_View("Focus", null, true, null, null , Contacts.ToList()), true);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
