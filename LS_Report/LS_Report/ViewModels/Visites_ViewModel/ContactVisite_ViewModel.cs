using LS_Report.Data;
using LS_Report.Interfaces;
using LS_Report.Models;
using LS_Report.ViewModels.MainMenu_ViewModels;
using LS_Report.Views;
using LS_Report.Views.ContactsPages;
using LS_Report.Views.VisitesPages;
using Newtonsoft.Json;
using Plugin.ExternalMaps;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.ViewModels.Visites_ViewModel
{
    internal class ContactVisite_ViewModel : INotifyPropertyChanged
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

        private Client2 contact { get; set; }

        public Client2 Contact
        {
            get { return contact; }
            set
            {
                if (contact != value)
                {
                    if (value.location.coordinates.Count() > 0)
                    {
                        if (value.location.coordinates[0].HasValue & value.location.coordinates[0].HasValue)
                            Position = value.location.coordinates[0].ToString() + " " + value.location.coordinates[1].ToString();
                    }
                    contact = value;
                    OnPropertyChanged();
                }
            }
        }

        private string position { get; set; }

        public string Position
        {
            get { return position; }
            set
            {
                if (position != value)
                    position = value;
                OnPropertyChanged();
            }
        }
        public string Potential { get; set; }
        private bool IsFreeMission { get; set; } = false;
        public bool CanVisite { get; set; }
        public bool IsFocus { get; set; }
        private string Global_Id { get; set; }
        private string Mission_Id { get; set; }
        private Token_Model Token { get; set; }
        private string Source { get; set; }
        public Command GoToPositionCommand { get; set; }
        public Command PictureTappedCommand { get; set; }
        public Command AddVisiteCommand { get; set; }
        public Command GetHistoryCommand { get; set; }
        public Command UnvailibilityCommand { get; set; }
        public Command AddContactFocus { get; set; }
        public ContactVisite_ViewModel(string source , INavigation navigation, IDataStore dataStore,Token_Model token , Client2 contact, bool isfreemission, bool canVisite, string global_id, string mission_id)
        {
            Source = source;
            Navigation = navigation;
            DataStore = dataStore;
            Contact = contact;
            IsFreeMission = isfreemission;
            CanVisite = canVisite;
            Global_Id = global_id;
            Mission_Id = mission_id;
            Token = token;

            Potential = (Contact.potential.Exists(i => i.network == Token.network)) ? Contact.potential.Single(i => i.network == Token.network).value : null;
            if(Source =="Focus")
            {
                CanVisite = false;
                IsFocus = true;
            }
            GoToPositionCommand = new Command(async () =>
            {
                await ExecuteOnGoToPosition();
            });
            PictureTappedCommand = new Command(async () =>
            {
                await ExecuteOnPictureTapped();
            });
            AddVisiteCommand = new Command(async () =>
            {
                await ExecuteOnAddVisite();
            });
            GetHistoryCommand = new Command(async () =>
            {
                await ExecuteOnGetHistory();
            });
            UnvailibilityCommand = new Command(async () =>
            {
                await ExecuteOnUnvailibility();
            });
            AddContactFocus = new Command(async () =>
            {
                await ExecuteOnAddContactToFocus();
            });
        }

        private async Task ExecuteOnGoToPosition()
        {
            if (Contact.location.coordinates.Count() > 0)
            {
                if (Contact.location.coordinates[0].HasValue && Contact.location.coordinates[1].HasValue)
                {
                    var lng = (double)Contact.location.coordinates[0];
                    var lat = (double)Contact.location.coordinates[1];
                    var succes = await CrossExternalMaps.Current.NavigateTo(Contact.Name, lng, lat);
                }
            }
        }

        private async Task ExecuteOnGetHistory()
        {
            IsBusy = true;
            var _restService = new RESTService();
            var Result = await _restService.GetClientsHistoryAsync(Contact._id);
            if (Result.Item1)
            {
                var contact = JsonConvert.DeserializeObject<ContactVisiteHistory_Model>(Result.Item2);
                contact.client = Contact;
                await Navigation.PushModalAsync(new ContactHistory_View(contact), true);
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert(Result.Item2);
            }
            IsBusy = false;
        }

        private async Task ExecuteOnUnvailibility()
        {
            if (!Contact.potential.Exists(i => i.network == Token.network) & !DataStore.GetEditContactsToUpload().ToList().Exists(i => i.Contact_Id == Contact._id))
            {
                if (await DependencyService.Get<IDialog>().AlertAsync("", "Le Contact n'a pas de potentiel , Vouler vous le modifier ?", "Oui", "Non"))
                {
                    
                    var lazem_nelgalha_hal = new ContactPage_ViewModel(Navigation);
                    await Navigation.PushModalAsync(new EditContact_View(Token, lazem_nelgalha_hal.Wilaya, lazem_nelgalha_hal.All_Commune, lazem_nelgalha_hal.Speciality, Contact));
                    
                }

            }
            else
            {

                if (await DependencyService.Get<IDialog>().AlertAsync("", "Voulez Vous Ajouter le rapport ?", "Oui", "Non"))
                {
                    var compareLocation = new CompareLocation();
                    var Result = compareLocation.CompareLocationFunction(DataStore, Contact);
                    string json;
                    if (IsFreeMission)
                    {
                        var Respone_Free = new Free_Unvailability();
                        Respone_Free.client = Contact._id;
                        Respone_Free.duration = Result.Item2;
                        if (Result.Item1)
                        {
                            Respone_Free.coords.Add(Result.Item3.lat);
                            Respone_Free.coords.Add(Result.Item3.lng);
                        }
                        Respone_Free.unavailability_reason = "Indisponible";
                        Respone_Free.valid = Result.Item1;
                        Respone_Free.visited = true;
                        Respone_Free.visit_time = Result.Item4;
                        json = JsonConvert.SerializeObject(Respone_Free, Formatting.None,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    }
                    else
                    {
                        var Respone = new Unvailability();
                        if (Result.Item1)
                        {
                            Respone.coords.Add(Result.Item3.lat);
                            Respone.coords.Add(Result.Item3.lng);
                        }
                        Respone.duration = Result.Item2;
                        Respone.unavailability_reason = "Indisponible";
                        Respone.valid = Result.Item1;
                        Respone.visited = true;
                        Respone.visit_time = Result.Item4;
                        json = JsonConvert.SerializeObject(Respone, Formatting.None,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    }
                    var data = new UnvailibilityToUpload_Model
                    {
                        Client_name = Contact.lastname + " " + Contact.firstname,
                        Contact_id = Contact._id,
                        Date = DateTime.Now,
                        Global_id = Global_Id,
                        IsFreeMission = IsFreeMission,
                        Json = json,
                        Mission_id = Mission_Id
                    };
                    DataStore.AddUnvailibilityToUpload(data);
                    MessagingCenter.Send(this, "UploadContactsTableModified");
                    await Navigation.PopModalAsync();
                }
            }
        }

        private async Task ExecuteOnPictureTapped()
        {
            if (!string.IsNullOrWhiteSpace(Contact.avatar))
            {
                await Navigation.PushModalAsync(new Picture_View(Contact.Avatar), true);
            }
        }

        private async Task ExecuteOnAddVisite()
        {
            if (!Contact.potential.Exists(i => i.network == Token.network) & !DataStore.GetEditContactsToUpload().ToList().Exists(i => i.Contact_Id == Contact._id))
            {
                if (await DependencyService.Get<IDialog>().AlertAsync("", "Le Contact n'a pas de potentiel , Vouler vous le modifier ?", "Oui", "Non"))
                {
                    
                    var lazem_nelgalha_hal = new ContactPage_ViewModel(Navigation);
                    await Navigation.PushModalAsync(new EditContact_View(Token,lazem_nelgalha_hal.Wilaya,lazem_nelgalha_hal.All_Commune,lazem_nelgalha_hal.Speciality,Contact));
                    
                }

            }
            else
            {
                await Navigation.PushModalAsync(new Visite_View(Source , Contact, IsFreeMission, Global_Id, Mission_Id), true);
            }
        }
        private async Task ExecuteOnAddContactToFocus()
        {
            if (await DependencyService.Get<IDialog>().AlertAsync("", "Voulez vous Ajouter le Contact Au Focus ?", "Oui", "Non"))
            {
                MessagingCenter.Send(this, "ContactFocus", Contact);
                await Navigation.PopModalAsync();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}