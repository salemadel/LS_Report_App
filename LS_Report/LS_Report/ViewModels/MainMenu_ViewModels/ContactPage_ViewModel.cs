using LS_Report.Data;
using LS_Report.Models;
using LS_Report.Views;
using LS_Report.Views.ContactsPages;
using Newtonsoft.Json;
using Plugin.SecureStorage;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.ViewModels.MainMenu_ViewModels
{
    public class ContactPage_ViewModel : INotifyPropertyChanged
    {
        private INavigation Navigation { get; set; }
        public List<Wilaya_Model> Wilaya { get; set; }
        public List<Commune> All_Commune { get; set; }
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

        public ContactPage_ViewModel(INavigation navigation)
        {
            Navigation = navigation;
            ContactsCommand = new Command(async () =>
           {
               await ExecuteOnContactsTapped();
           });
            AddContactCommand = new Command(async () =>
            {
                await ExecuteOnAddContactTapped();
            });
            EditContactCommand = new Command(async () =>
            {
                await ExecuteOnEditContactTapped();
            });
            GetDataFromJsonFiles();
        }

        private Token_Model Token { get; set; }
        public Command ContactsCommand { get; set; }
        public Command AddContactCommand { get; set; }
        public Command EditContactCommand { get; set; }

        private async Task ExecuteOnContactsTapped()
        {
            IsBusy = true;
            await Navigation.PushAsync(new Contatcs_View("Contacts_List", Token, Wilaya, All_Commune, Speciality), true);
            IsBusy = false;
        }

        private async Task ExecuteOnAddContactTapped()
        {
            IsBusy = true;
            await Navigation.PushModalAsync(new AddContact_View(Token, Wilaya, All_Commune, Speciality), true);
            IsBusy = false;
        }

        private async Task ExecuteOnEditContactTapped()
        {
            IsBusy = true;
            await Navigation.PushAsync(new Contatcs_View("Edit_Contact", Token, Wilaya, All_Commune, Speciality), true);
            IsBusy = false;
        }

        private void GetDataFromJsonFiles()
        {
            IsBusy = true;
            var _tokenController = new TokenController();
            var token = _tokenController.DecodeToken();
            if (token.Item1)
            {
                Token = token.Item2;
            }
            else
            {
                CrossSecureStorage.Current.SetValue("LogguedIn", "False");
                CrossSecureStorage.Current.DeleteKey("acces_token");
                Application.Current.MainPage = new Login_View();
                return;
            }
            string JsonFile = "wilayas.json";
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(ContactPage_ViewModel)).Assembly;
            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{JsonFile}");
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                Wilaya = JsonConvert.DeserializeObject<List<Wilaya_Model>>(json);
            }
            JsonFile = "communes.json";
            assembly = IntrospectionExtensions.GetTypeInfo(typeof(ContactPage_ViewModel)).Assembly;
            stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{JsonFile}");
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                All_Commune = JsonConvert.DeserializeObject<List<Commune>>(json);
            }
            JsonFile = "speciality.json";
            assembly = IntrospectionExtensions.GetTypeInfo(typeof(ContactPage_ViewModel)).Assembly;
            stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{JsonFile}");
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                Speciality = JsonConvert.DeserializeObject<List<string>>(json);
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