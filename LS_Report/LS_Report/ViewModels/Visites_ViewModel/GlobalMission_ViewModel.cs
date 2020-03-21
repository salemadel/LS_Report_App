using LS_Report.Interfaces;
using LS_Report.Models;
using LS_Report.Views.VisitesPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.ViewModels.Visites_ViewModel
{
    public class GlobalMission_ViewModel : INotifyPropertyChanged
    {
        private INavigation Navigation { get; set; }
        private IDataStore DataStore { get; set; }
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

        public ContactToVisite_Model Selected_Contact { get; set; }
        private List<ContactToVisite_Model> Contacts { get; set; }
        private ObservableCollection<ContactToVisite_Model> filtred_List { get; set; }

        public ObservableCollection<ContactToVisite_Model> Filtred_List
        {
            get { return filtred_List; }
            set
            {
                if (filtred_List != value)
                    filtred_List = value;
                OnPropertyChanged();
            }
        }

        public DateTime Min_Date { get; set; }
        public DateTime Max_Date { get; set; }
        private DateTime selected_date { get; set; }

        public DateTime Selected_Date
        {
            get { return selected_date; }
            set
            {
                if (selected_date != value)
                {
                    selected_date = value;
                    Filtred_List = new ObservableCollection<ContactToVisite_Model>(Contacts.Where(i => i.VisiteDate.Date == value.Date).ToList());
                    OnPropertyChanged();
                }
            }
        }
        private Token_Model Token { get; set; }
        public ObservableCollection<Missions_Model> Global_Missions { get; set; }

        public Command ItemTappedCommand { get; set; }

        public GlobalMission_ViewModel(INavigation navigation, IDataStore dataStore , Token_Model token)
        {
            Navigation = navigation;
            DataStore = dataStore;
            Token = token;
            Global_Missions = new ObservableCollection<Missions_Model>();
            Contacts = new List<ContactToVisite_Model>();
            Initialize();
            ItemTappedCommand = new Command(async () =>
            {
                await ExecuteOnItemTapped();
            });
        }

        private void Initialize()
        {
            var List = DataStore.GetDataStoredJson("Missions").ToList();
            if (List.Count > 0)
            {
                Global_Missions = new ObservableCollection<Missions_Model>(JsonConvert.DeserializeObject<List<Missions_Model>>(List[0].json));
                foreach (var global in Global_Missions)
                {
                    foreach (var mission in global.missions)
                    {
                        foreach (var contact in mission.clients)
                        {
                            var contacttovisite = new ContactToVisite_Model
                            {
                                address = contact.client.address,
                                age = contact.client.age,
                                avatar = contact.client.avatar,
                                business_type = contact.client.business_type,
                                city = contact.client.city,
                                company = contact.client.company,
                                email = contact.client.email,
                                fax = contact.client.fax,
                                firstname = contact.client.firstname,
                                Global_Id = global._id,
                                landline = contact.client.landline,
                                lastname = contact.client.lastname,
                                local_appearance = contact.client.local_appearance,
                                location = contact.client.location,
                                Mission_Id = mission._id,
                                phone = contact.client.phone,
                                placement = contact.client.placement,
                                potential = contact.client.potential,
                                prescription = contact.client.prescription,
                                rc = contact.client.rc,
                                sector = contact.client.sector,
                                sex = contact.client.sex,
                                speciality = contact.client.speciality,
                                suppliers = contact.client.suppliers,
                                VisiteDate = mission.mission_deadline,
                                wilaya = contact.client.wilaya,
                                _id = contact.client._id,
                                Categorie = mission.title,
                                Visited = contact.visited
                            };
                            Contacts.Add(contacttovisite);
                        }
                    }
                }
                Min_Date = Global_Missions.OrderBy(i => i.global_mission_start).First().global_mission_start;

                Max_Date = Global_Missions.OrderBy(i => i.global_mission_deadline).Last().global_mission_deadline;
            }
            Selected_Date = DateTime.Now.Date;
        }

        private async Task ExecuteOnItemTapped()
        {
            Client2 Contact = new Client2
            {
                address = Selected_Contact.address,
                age = Selected_Contact.age,
                avatar = Selected_Contact.avatar,
                business_type = Selected_Contact.business_type,
                city = Selected_Contact.city,
                company = Selected_Contact.company,
                email = Selected_Contact.email,
                fax = Selected_Contact.fax,
                firstname = Selected_Contact.firstname,
                landline = Selected_Contact.landline,
                lastname = Selected_Contact.lastname,
                local_appearance = Selected_Contact.local_appearance,
                location = Selected_Contact.location,
                phone = Selected_Contact.phone,
                placement = Selected_Contact.placement,
                potential = Selected_Contact.potential,
                prescription = Selected_Contact.prescription,
                rc = Selected_Contact.rc,
                sector = Selected_Contact.sector,
                sex = Selected_Contact.sex,
                speciality = Selected_Contact.speciality,
                suppliers = Selected_Contact.suppliers,
                wilaya = Selected_Contact.wilaya,
                _id = Selected_Contact._id,
            };
            bool CanVisite = (Selected_Contact.VisiteDate.Date == DateTime.Now.Date) ? true : false;
            CanVisite = (CanVisite) ? !Selected_Contact.Visited : CanVisite;
            await Navigation.PushModalAsync(new ContactVisite_Page(null, Contact,Token , false, CanVisite, Selected_Contact.Global_Id, Selected_Contact.Mission_Id));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}