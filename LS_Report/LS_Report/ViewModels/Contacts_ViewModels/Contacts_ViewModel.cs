using LS_Report.Converters;
using LS_Report.Data;
using LS_Report.Interfaces;
using LS_Report.Models;
using LS_Report.Views.ContactsPages;
using LS_Report.Views.VisitesPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.ViewModels.Contacts_ViewModels
{
    public class Contacts_ViewModel : INotifyPropertyChanged
    {
        private INavigation Navigation { get; set; }
        private IDataStore DataStore { get; set; }
        private string Source { get; set; }
        private List<string> potentiel { get; set; }

        public List<string> Potentiel
        {
            get { return potentiel; }
            set
            {
                if (potentiel != value)
                    potentiel = value;
                OnPropertyChanged();
            }
        }

        public List<Wilaya_Model> Wilaya { get; set; }
        public List<Commune> All_Commune { get; set; }
        private List<Commune> commune { get; set; }

        public List<Commune> Commune
        {
            get { return commune; }
            set
            {
                if (commune != value)
                    commune = value;
                commune.Insert(0, new Commune
                {
                    nom = "Tous"
                });
                OnPropertyChanged();
            }
        }

        private Wilaya_Model selected_Wilaya { get; set; }

        public Wilaya_Model Selected_Wilaya
        {
            get { return selected_Wilaya; }
            set
            {
                if (selected_Wilaya != value)
                {
                    selected_Wilaya = value;
                    Commune = All_Commune.Where(i => i.wilaya_id == value.id).OrderBy(i => i.nom).ToList();
                    Selected_Commune = Commune[0];
                    Selected_Speciality = "Tous";
                    Selected_Potentiel = "Tous";
                    SearchBarText = string.Empty;
                    OnPropertyChanged();
                }
            }
        }

        private Commune selected_Commune { get; set; }

        public Commune Selected_Commune
        {
            get { return selected_Commune; }
            set
            {
                if (selected_Commune != value)
                    selected_Commune = value;
                OnPropertyChanged();
            }
        }

        private List<string> speciality { get; set; }

        public List<string> Speciality
        {
            get { return speciality; }
            set
            {
                if (speciality != value)
                    speciality = value;
                OnPropertyChanged();
            }
        }

        private string selected_Speciality { get; set; }

        public string Selected_Speciality
        {
            get { return selected_Speciality; }
            set
            {
                if (selected_Speciality != value)
                    selected_Speciality = value;
                OnPropertyChanged();
            }
        }

        private string selected_Potentiel { get; set; }

        public string Selected_Potentiel
        {
            get { return selected_Potentiel; }
            set
            {
                if (selected_Potentiel != value)
                    selected_Potentiel = value;
                OnPropertyChanged();
            }
        }

        private string searchBarText { get; set; }

        public string SearchBarText
        {
            get { return searchBarText; }
            set
            {
                if (searchBarText != value)
                    searchBarText = value;

                OnPropertyChanged();
            }
        }

        private ObservableCollection<Grouping<string, Client2>> contacts { get; set; }

        public ObservableCollection<Grouping<string, Client2>> Contacts
        {
            get { return contacts; }
            set
            {
                if (contacts != value)
                    contacts = value;
                OnPropertyChanged();
            }
        }

        private List<Client2> no_Sorted_Contacts { get; set; }

        public List<Client2> No_Sorted_Contacts
        {
            get { return no_Sorted_Contacts; }
            set
            {
                if (no_Sorted_Contacts != value)
                    no_Sorted_Contacts = value;
                OnPropertyChanged();
            }
        }

        private List<Client2> filtred_list { get; set; }

        public List<Client2> Filtred_list
        {
            get { return filtred_list; }
            set
            {
                if (filtred_list != value)
                    filtred_list = value;
                OnPropertyChanged();
            }
        }

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

        private Client2 selected_Item { get; set; }

        public Client2 Selected_Item
        {
            get { return selected_Item; }
            set
            {
                if (selected_Item != value)
                    selected_Item = value;
                OnPropertyChanged();
            }
        }

        private Token_Model Token { get; set; }

        public Contacts_ViewModel(INavigation navigation, IDataStore dataStore, Token_Model token, string source, List<Wilaya_Model> wilayas, List<Commune> communes, List<string> specialitys)
        {
            Navigation = navigation;
            DataStore = dataStore;
            Token = token;
            Source = source;
            Wilaya = wilayas;
            All_Commune = communes;
            Speciality = specialitys;

            DropDownCommand = new Command(() =>
            {
                ExecuteOnDropDown();
            });
            GetContactsCommand = new Command(async () =>
            {
                await ExecuteOnGetContacts();
            });
            FiltreCommand = new Command(() =>
            {
                ExecuteOnFiltre();
            });
            SearchCommand = new Command(() =>
            {
                ExecuteOnSearch();
            });
            ItemTappedCommand = new Command(async () =>
            {
                await ExecuteOnItemTapped();
            });
            Initialize();
            UpdateContactsListview();
        }

        public Command DropDownCommand { get; set; }
        public Command GetContactsCommand { get; set; }
        public Command FiltreCommand { get; set; }
        public Command SearchCommand { get; set; }
        public Command ItemTappedCommand { get; set; }

        private void ExecuteOnDropDown()
        {
            if (IsVisible == false)
            {
                IsVisible = true;
            }
            else
            {
                IsVisible = false;
            }
        }

        private async Task ExecuteOnGetContacts()
        {
            IsBusy = true;
            IsVisible = false;
            await GetContactsAsync();
            IsBusy = false;
        }

        private async Task GetContactsAsync()
        {
            var _restService = new RESTService();
            var result = await _restService.GetClientsAsync();

            if (result.Item1 == true)
            {
                var json = result.Item2;

                if (DataStore.GetDataStoredJson("Contacts").ToList().Count > 0)
                {
                    DataStore.UpdateData("Contacts", json);
                }
                else
                {
                    var Data = new Stored_Data_Model
                    {
                        Type = "Contacts",
                        json = json
                    };
                    DataStore.AddData(Data);
                }
                UpdateContactsListview();
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert(result.Item2);
            }
        }

        private void UpdateContactsListview()
        {
            IsBusy = true;
            var query = DataStore.GetDataStoredJson("Contacts").ToList();
            if (query.Count > 0)
            {
                No_Sorted_Contacts = JsonConvert.DeserializeObject<List<Client2>>(query[0].json);
                var sorted = from contact in No_Sorted_Contacts
                             orderby contact.Name
                             group contact by contact.NameSort into contactGroup
                             select new Grouping<string, Client2>(contactGroup.Key, contactGroup);

                Contacts = new ObservableCollection<Grouping<string, Client2>>(sorted);
                Filtred_list = No_Sorted_Contacts;
                SearchBarText = string.Empty;
            }
            IsBusy = false;
        }

        private void Initialize()
        {
            IsBusy = true;
            Speciality.Insert(0, "Tous");
            Speciality.Insert(1, "Pharmacien");
            Speciality.Insert(2, "Grossiste");
            Speciality.Insert(3, "Sage-femme");
            Speciality.Insert(4, "Chirugien-dentiste");
            Potentiel = new List<string>();
            Potentiel.Add("Tous");
            Potentiel.Add("A (> 20)");
            Potentiel.Add("B (10-20)");
            Potentiel.Add("C (< 10)");
            IsBusy = false;
        }

        private void ExecuteOnFiltre()
        {
            if (No_Sorted_Contacts != null)
            {
                Filtred_list = No_Sorted_Contacts;
                SearchBarText = string.Empty;
                if (Selected_Wilaya != null)
                {
                    Filtred_list = No_Sorted_Contacts.Where(i => i.wilaya.Contains(Selected_Wilaya.nom)).ToList();
                }
                if (Selected_Commune != null && Selected_Commune.nom != "Tous")
                {
                    Filtred_list = filtred_list.ToList().Where(i => i.city.Contains(Selected_Commune.nom)).ToList();
                }
                if (Selected_Speciality != null && Selected_Speciality != "Tous")
                {
                    Filtred_list = filtred_list.ToList().Where(i => i.Speciality.Contains(Selected_Speciality)).ToList();
                }
                if (Selected_Potentiel != null && Selected_Potentiel != "Tous")
                {
                    Filtred_list = filtred_list.Where(i => i.potential.Any(j => j.network == Token.network & j.value.Contains(Selected_Potentiel))).ToList();
                }
                var sorted = from contact in Filtred_list
                             orderby contact.Name
                             group contact by contact.NameSort into contactGroup
                             select new Grouping<string, Client2>(contactGroup.Key, contactGroup);

                Contacts = new ObservableCollection<Grouping<string, Client2>>(sorted);
            }
            IsVisible = false;
        }

        private void ExecuteOnSearch()
        {
            if (Filtred_list != null)
            {
                if (string.IsNullOrWhiteSpace(SearchBarText))
                {
                    var sorted = from contact in Filtred_list
                                 orderby contact.Name
                                 group contact by contact.NameSort into contactGroup
                                 select new Grouping<string, Client2>(contactGroup.Key, contactGroup);

                    Contacts = new ObservableCollection<Grouping<string, Client2>>(sorted);
                }
                else
                {
                    if (Filtred_list == null)
                    {
                        Filtred_list = No_Sorted_Contacts;
                    }
                    var filtred_list = Filtred_list.Where(i => i.Name.ToLower().Contains(SearchBarText.ToLower())).ToList();
                    var sorted = from contact in filtred_list
                                 orderby contact.Name
                                 group contact by contact.NameSort into contactGroup
                                 select new Grouping<string, Client2>(contactGroup.Key, contactGroup);

                    Contacts = new ObservableCollection<Grouping<string, Client2>>(sorted);
                }
            }
        }

        private async Task ExecuteOnItemTapped()
        {
            IsBusy = true;
            if (Source == "Contacts_List")
            {
                await Navigation.PushModalAsync(new ContactDetail_View(Selected_Item, Token), true);
            }
            if (Source == "Edit_Contact")
            {
                await Navigation.PushModalAsync(new EditContact_View(Token, Wilaya, All_Commune, Speciality, Selected_Item), true);
            }
            if (Source == "Add_Visite")
            {
                await Navigation.PushModalAsync(new ContactVisite_Page(Source, Selected_Item, Token, true, true), true);
            }
            if (Source == "Focus")
            {
                await Navigation.PushModalAsync(new ContactVisite_Page(Source, Selected_Item, Token, true, true), true);
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