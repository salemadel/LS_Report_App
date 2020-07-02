using LS_Report.Data;
using LS_Report.Interfaces;
using LS_Report.Models;
using LS_Report.Views;
using LS_Report.Views.ContactsPages;
using LS_Report.Views.VisitesPages;
using Newtonsoft.Json;
using Plugin.SecureStorage;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.ViewModels.MainMenu_ViewModels
{
    public class VisitePage_ViewModel : INotifyPropertyChanged
    {
        private INavigation Navigation { get; set; }
        private bool IsFreeMission { get; set; } = false;
        private bool isBusy { get; set; }
        private string[] absent = { "Congé", "Formation", "Maladie", "Manifestation", "Récupération", "Réunion", "Focus" };

        public List<string> Abscence_List
        {
            get { return absent.ToList(); }
        }

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

        public string Selected_Item { get; set; }

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

        private Token_Model Token { get; set; }
        public Command AddVisiteCommand { get; set; }
        public Command ProductsListCommand { get; set; }
        public Command HistoryCommand { get; set; }
        public Command PopUpCommand { get; set; }
        public Command AbsenceCommand { get; set; }

        public VisitePage_ViewModel(INavigation navigation)
        {
            Initialize();
            Navigation = navigation;
            AddVisiteCommand = new Command(async () =>
            {
                await ExecuteOnAddVisitTapped();
            });
            ProductsListCommand = new Command(async () =>
            {
                await ExecuteOnProductsTapped();
            });
            HistoryCommand = new Command(async () =>
            {
                await ExecuteOnHistoryTapped();
            });
            PopUpCommand = new Command(() =>
            {
                ExecuteOnPopUp();
            });
            AbsenceCommand = new Command(async () =>
            {
                await ExecuteOnAbscence();
            });
        }

        private async Task ExecuteOnAddVisitTapped()
        {
            IsBusy = true;
            if (IsFreeMission)
            {
                var ContactsPage_ViewModel = new ContactPage_ViewModel(Navigation);
                await Navigation.PushAsync(new Contatcs_View("Add_Visite", Token, ContactsPage_ViewModel.Wilaya, ContactsPage_ViewModel.All_Commune, ContactsPage_ViewModel.Speciality), true);
            }
            else
            {
                await Navigation.PushAsync(new GlobalMission_View(Token), true);
            }
            IsBusy = false;
        }

        private async Task ExecuteOnProductsTapped()
        {
            IsBusy = true;
            await Navigation.PushAsync(new Products_View("Products_List"), true);
            IsBusy = false;
        }

        private async Task ExecuteOnHistoryTapped()
        {
            IsBusy = true;
            await Navigation.PushAsync(new VisiteHistory_View(), true);
            IsBusy = false;
        }

        private void ExecuteOnPopUp()
        {
            if (PopUpVisible)
            {
                PopUpVisible = false;
                Selected_Item = null;
                OnPropertyChanged("Selected_Item");
            }
            else
            {
                PopUpVisible = true;
            }
        }

        private async Task ExecuteOnAbscence()
        {
            if (Selected_Item != null)
            {
                PopUpVisible = false;
                IsBusy = true;
                if (Selected_Item == "Focus")
                {
                    var lazem_nelgalha_hal = new ContactPage_ViewModel(Navigation);
                    await Navigation.PushAsync(new Focus_View(Token, lazem_nelgalha_hal.Wilaya, lazem_nelgalha_hal.All_Commune, lazem_nelgalha_hal.Speciality), true);
                    IsBusy = false;
                }
                else
                {
                    var _restService = new RESTService();
                    var abscence = new Absence_mission();
                    abscence.absence = Selected_Item;
                    var json = JsonConvert.SerializeObject(abscence, Formatting.None,
                           new JsonSerializerSettings
                           {
                               NullValueHandling = NullValueHandling.Ignore
                           });
                    var Result = await _restService.Post_absence(json, IsFreeMission, null, null, null);
                    if (Result.Item1)
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Absence Syncroniser Avec Succes !");
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert(Result.Item2);
                    }
                    IsBusy = false;
                }
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Veuillez Selectionner un Motif !");
            }
        }

        private void Initialize()
        {
            var _tokenController = new TokenController();
            var token = _tokenController.DecodeToken();
            if (token.Item1)
            {
                if (token.Item2.permissions.Contains("Tournée Libre"))
                {
                    IsFreeMission = true;
                }
                Token = token.Item2;
            }
            else
            {
                CrossSecureStorage.Current.SetValue("LogguedIn", "False");
                CrossSecureStorage.Current.DeleteKey("acces_token");
                Application.Current.MainPage = new Login_View();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}