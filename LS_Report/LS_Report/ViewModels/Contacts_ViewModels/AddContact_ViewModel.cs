using LS_Report.Data;
using LS_Report.Interfaces;
using LS_Report.Models;
using LS_Report.Views;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Media;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LS_Report.ViewModels.Contacts_ViewModels
{
    public class AddContact_ViewModel : INotifyPropertyChanged
    {
        private INavigation Navigation { get; set; }
        private IDataStore DataStore { get; set; }
        private PermissionsRequest PermissionsRequest { get; set; }
        private string[] professions = { "Chirugien dentiste", "Médecin", "Pharmacien", "Grossiste", "Sage-femme" };
        private string[] types_m = { "Bon", "Moyen", "Mauvais" };
        private string[] types_f = { "Bonne", "Moyenne", "Mauvaise" };
        private string[] age = { "30-40", "40-50", "50-60", "< 30", "> 60" };
        private string[] potentiel = { "A (> 20)", "B (10-20)", "C (< 10)" };
        private string[] sector = { "Privé", "Public" };

        public List<string> Professions
        {
            get { return professions.ToList(); }
        }

        public List<string> Types_m
        {
            get { return types_m.ToList(); }
        }

        public List<string> Types_f
        {
            get { return types_f.ToList(); }
        }

        public List<string> Age
        {
            get { return age.ToList(); }
        }

        public List<string> Potentiel
        {
            get { return potentiel.ToList(); }
        }

        public List<string> Sector
        {
            get { return sector.ToList(); }
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

        private bool male_Cheked { get; set; }

        public bool Male_Cheked
        {
            get { return male_Cheked; }
            set
            {
                if (male_Cheked != value)
                    if (value == true)
                    {
                        Contact.sex = "Homme";
                    }
                male_Cheked = value;
                OnPropertyChanged();
            }
        }

        private bool femele_Cheked { get; set; }

        public bool Femele_Cheked
        {
            get { return femele_Cheked; }
            set
            {
                if (femele_Cheked != value)
                    if (value == true)
                    {
                        Contact.sex = "Femme";
                    }
                femele_Cheked = value;
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

        private string position { get; set; } = null;

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

        private string picturePath { get; set; }

        public string PicturePath
        {
            get { return picturePath; }
            set
            {
                if (picturePath != value)
                    picturePath = value;
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

        private NewContact_Model contact { get; set; }

        public NewContact_Model Contact
        {
            get { return contact; }
            set
            {
                if (contact != value)
                    contact = value;
                OnPropertyChanged();
            }
        }

        private string selected_Potential { get; set; }

        public string Selected_Potential
        {
            get { return selected_Potential; }
            set
            {
                if (selected_Potential != value)
                {
                    selected_Potential = value;
                    Contact.potential.RemoveAll(i => i.network == Token.network);
                    Contact.potential.Add(new Potential { network = Token.network, value = value });
                }
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

        private string selected_Profession { get; set; }

        public string Selected_Profession
        {
            get { return selected_Profession; }
            set
            {
                if (selected_Profession != value)
                {
                    if (value == "Médecin")
                    {
                        IsVisible = true;
                        selected_Profession = value;
                    }
                    else
                    {
                        IsVisible = false;
                        Contact.speciality = string.Empty;
                        Selected_Speciality = string.Empty;
                        selected_Profession = value;
                    }
                    OnPropertyChanged();
                }
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

        private Token_Model Token { get; set; }

        public AddContact_ViewModel(INavigation navigation, IDataStore dataStore, Token_Model token, List<Wilaya_Model> wilayas, List<Commune> communes, List<string> speciality)
        {
            Navigation = navigation;
            DataStore = dataStore;
            Contact = new NewContact_Model();
            Wilaya = wilayas;
            Speciality = speciality;
            All_Commune = communes;
            Token = token;
            PermissionsRequest = new PermissionsRequest();
            Speciality.Remove("Tous");
            Speciality.Remove("Pharmacien");
            Speciality.Remove("Sage-femme");
            Speciality.Remove("Chirugien-dentiste");
            Speciality.Remove("Grossiste");
            GetPositionCommand = new Command(async () =>
            {
                await ExecuteOnGetPosition();
            });
            TakePictureCommand = new Command(async () =>
            {
                await ExecuteOnTakePicture();
            });
            AddContactCommand = new Command(async () =>
            {
                await ExecuteOnAddContact();
            });
            PictureTappedCommand = new Command(async () =>
            {
                await ExecuteOnPictureTapped();
            });
        }

        public Command GetPositionCommand { get; set; }
        public Command TakePictureCommand { get; set; }
        public Command AddContactCommand { get; set; }
        public Command PictureTappedCommand { get; set; }

        private async Task ExecuteOnGetPosition()
        {
            IsBusy = true;
            var test = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            Position position = null;
            try
            {
                var status = await Xamarin.Essentials.Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    var results = await Xamarin.Essentials.Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                    if (results == PermissionStatus.Granted)
                        status = PermissionStatus.Granted;
                }
                if (status == PermissionStatus.Granted)
                {
                    var locator = CrossGeolocator.Current;

                    locator.DesiredAccuracy = 100;

                    if (locator.IsGeolocationAvailable && locator.IsGeolocationEnabled)
                    {
                        position = await locator.GetPositionAsync(TimeSpan.FromSeconds(30), null, true);
                    }
                    else
                    {
                        DependencyService.Get<ISettingService>().OpenSettings();
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (position != null)
                {
                    Contact.location.coordinates.lat = position.Latitude.ToString().Replace(",", ".");
                    Contact.location.coordinates.lng = position.Longitude.ToString().Replace(",", ".");
                    Position = Contact.location.coordinates.lat + " ; " + Contact.location.coordinates.lng;
                }
            }
            IsBusy = false;
        }

        private async Task ExecuteOnTakePicture()
        {
            if (await PermissionsRequest.Check_permissions("Storage") == PermissionStatus.Granted)
                try
                {
                    await CrossMedia.Current.Initialize();
                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        return;
                    }

                    var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                        CompressionQuality = 50,
                        Directory = "LS_Report_pictures",
                        Name = "Ls_" + DateTime.Now.ToString() + ".jpg"
                    });

                    if (file == null)
                        return;

                    PicturePath = file.Path;
                }
                catch
                {
                }
        }

        private async Task ExecuteOnAddContact()
        {
            var props = typeof(NewContact_Model).GetProperties();
            string[] required = { "lastname", "firsname", "business_type", "sector", "placement", "sex", "local_appearance", "prescription", "wilaya", "city", "address" };
            bool valid = true;
            foreach (var prop in props)
            {
                if (required.Contains(prop.Name))
                {
                    object value = prop.GetValue(Contact, null);
                    if (string.IsNullOrWhiteSpace((string)value))
                    {
                        IsBusy = false;
                        DependencyService.Get<IMessage>().ShortAlert("Vous devez remplir les champs obligatoires !");
                        valid = false;

                        return;
                    }
                }
            }
            if (CheckIfContactExiste())
            {
                IsBusy = false;
                DependencyService.Get<IMessage>().ShortAlert("Contact Exsite Déja !");
                return;
            }
            if (string.IsNullOrWhiteSpace(Contact.location.coordinates.lat) & valid)
            {
                valid = false;
                DependencyService.Get<IMessage>().ShortAlert("Impossible d'ajouter un Contact sans sa position gps !");
            }
            if (valid)
            {
                if (await DependencyService.Get<IDialog>().AlertAsync("", "Voulez Vous Ajouter le Contact " + Contact.lastname + " " + Contact.firstname + " ?", "Oui", "Non"))
                {
                    var json = JsonConvert.SerializeObject(Contact);
                    System.Diagnostics.Debug.WriteLine(json);
                    var contact_data = new NewContactToUpload_Model
                    {
                        Date = DateTime.Now,
                        Json = json,
                        Name = Contact.lastname + " " + Contact.firstname,
                        PicturePath = PicturePath
                    };
                    DataStore.AddContactToUpload(contact_data);
                    MessagingCenter.Send(this, "UploadContactsTableModified");
                    await Navigation.PopModalAsync();
                }
            }
        }

        private async Task ExecuteOnPictureTapped()
        {
            if (!string.IsNullOrWhiteSpace(PicturePath))
            {
                IsBusy = true;
                await Navigation.PushModalAsync(new Picture_View(ImageSource.FromFile(PicturePath)), true);
                IsBusy = false;
            }
        }

        private bool CheckIfContactExiste()
        {
            bool Existe = false;
            var List = DataStore.GetDataStoredJson("Contacts").ToList();
            if (List.Count > 0)
            {
                var Contacts = JsonConvert.DeserializeObject<List<Client2>>(List[0].json);
                Existe = Contacts.Exists(i => i.firstname.ToLower() == Contact.firstname.ToLower() & i.lastname.ToLower() == Contact.lastname.ToLower() & i.wilaya == Contact.wilaya & i.city == Contact.city & i.business_type == Contact.business_type & i.speciality == Contact.speciality);
            }
            return Existe;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}