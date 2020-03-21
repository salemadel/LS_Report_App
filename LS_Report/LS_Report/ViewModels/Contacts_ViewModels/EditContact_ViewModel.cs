using LS_Report.Data;
using LS_Report.Interfaces;
using LS_Report.Models;
using LS_Report.Views;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.ViewModels.Contacts_ViewModels
{
    public class EditContact_ViewModel : INotifyPropertyChanged
    {
        private INavigation Navigation { get; set; }
        private IDataStore DataStore { get; set; }
        private PermissionsRequest Permissions { get; set; }
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

        private Client2 contact { get; set; }

        public Client2 Contact
        {
            get { return contact; }
            set
            {
                if (contact != value)
                    contact = value;
                OnPropertyChanged();
            }
        }

        public NewContact_Model NewContact { get; set; }
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
                        NewContact.sex = "Homme";
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
                        NewContact.sex = "Femme";
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
                OnPropertyChanged("Picture");
            }
        }

        public ImageSource Picture
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PicturePath))
                {
                    return Contact.Avatar;
                }
                else
                {
                    return ImageSource.FromFile(PicturePath);
                }
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
                    NewContact.wilaya = value.nom;
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
                NewContact.city = value.nom;
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
        private Token_Model Token { get; set; }
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
                        NewContact.speciality = string.Empty;
                        OnPropertyChanged("NewContact");
                        selected_Profession = value;
                    }
                    OnPropertyChanged();
                }
            }
        }
        private string selected_Potential { get; set; }
        public string Selected_Potential
        {
            get { return selected_Potential; }
            set
            {
                if(selected_Potential != value)
                {
                    selected_Potential = value;
                   if(NewContact.potential.Exists(i =>i.network == Token.network))
                    {
                        NewContact.potential.Single(i => i.network == Token.network).value = value;
                    }
                   else
                    {
                        NewContact.potential.Add(new Potential { network = Token.network, value = value });
                    }
                    OnPropertyChanged();
                }
            }
        }

        public Command EditContactCommand { get; set; }
        public Command GetPositionCommand { get; set; }
        public Command TakePictureCommand { get; set; }
        public Command PictureTappedCommand { get; set; }

        public EditContact_ViewModel(INavigation navigation, IDataStore dataStore,Token_Model token , List<Wilaya_Model> wilayas, List<Commune> communes, List<string> specialitys, Client2 contact)
        {
            Navigation = navigation;
            DataStore = dataStore;
            Contact = contact;
            Permissions = new PermissionsRequest();
            Wilaya = wilayas;
            Speciality = specialitys;
            All_Commune = communes;
            Token = token;
            Initilize();
            EditContactCommand = new Command(async () =>
            {
                await ExecuteOnEditContact();
            });
            GetPositionCommand = new Command(async () =>
            {
                await ExecuteOnGetPosition();
            });
            TakePictureCommand = new Command(async () =>
            {
                await ExecuteOnTakePicture();
            });
            PictureTappedCommand = new Command(async () =>
           {
               await ExecuteOnPictureTapped();
           });
        }

        private void Initilize()
        {
            NewContact = new NewContact_Model();
            NewContact.address = Contact.address;
            NewContact.age = Contact.age;
            NewContact.business_type = Contact.business_type;
            Selected_Profession = Contact.business_type;
            NewContact.city = Contact.city;
            NewContact.company = Contact.company;
            NewContact.email = Contact.email;
            NewContact.fax = Contact.fax;
            NewContact.firstname = Contact.firstname;
            NewContact.landline = Contact.landline;
            NewContact.lastname = Contact.lastname;
            NewContact.local_appearance = Contact.local_appearance;
            if (Contact.location.coordinates.Count() > 0)
            {
                if (Contact.location.coordinates[0].HasValue)
                {
                    NewContact.location.coordinates.lat = Contact.location.coordinates[0].ToString().Replace(",", ".");
                    NewContact.location.coordinates.lng = Contact.location.coordinates[1].ToString().Replace(",", ".");
                    Position = NewContact.location.coordinates.lat + ";" + NewContact.location.coordinates.lng;
                }
            }
            NewContact.phone = Contact.phone;
            NewContact.placement = Contact.placement;
            NewContact.potential = Contact.potential;
            Selected_Potential = (Contact.potential.Exists(i => i.network == Token.network)) ? Contact.potential.Single(i => i.network == Token.network).value : null;
            NewContact.prescription = Contact.prescription;
            NewContact.sector = Contact.sector;
            NewContact.sex = Contact.sex;
            NewContact.speciality = Contact.speciality;
            NewContact.wilaya = Contact.wilaya;
            NewContact._id = Contact._id;
            if (Contact.sex == "Homme")
            {
                Male_Cheked = true;
            }
            else
            {
                Femele_Cheked = true;
            }
            Speciality.Remove("Tous");
            Speciality.Remove("Pharmacien");
            Speciality.Remove("Sage-femme");
            Speciality.Remove("Chirugien-dentiste");
            Speciality.Remove("Grossiste");
        }

        private async Task ExecuteOnEditContact()
        {
            var props = typeof(NewContact_Model).GetProperties();
            string[] required = { "lastname", "firsname", "business_type", "sector", "placement", "sex", "local_appearance", "prescription", "wilaya", "city", "address" };
            bool valid = true;
            foreach (var prop in props)
            {
                if (required.Contains(prop.Name))
                {
                    object value = prop.GetValue(NewContact, null);
                    if (string.IsNullOrWhiteSpace((string)value))
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Vous devez remplir les champs obligatoires !");
                        valid = false;
                        break;
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(NewContact.location.coordinates.lat) & valid)
            {
                valid = false;
                DependencyService.Get<IMessage>().ShortAlert("Impossible d'éditer un Contact sans sa position gps !");
            }
            if (valid)
            {
                if (await DependencyService.Get<IDialog>().AlertAsync("", "Voulez Vous Modifier le Contact " + Contact.Name + " ?", "Oui", "Non"))
                {
                    var json = JsonConvert.SerializeObject(NewContact);
                    var contact_data = new EditContactToUpload_Model
                    {
                        Date = DateTime.Now,
                        Json = json,
                        Name = NewContact.lastname + " " + NewContact.firstname,
                        PicturePath = PicturePath,
                        Contact_Id = NewContact._id
                    };
                    DataStore.AddEditContactToUpload(contact_data);
                    MessagingCenter.Send(this, "UploadContactsTableModified");
                    await Navigation.PopModalAsync();
                }
            }
        }

        private async Task ExecuteOnGetPosition()
        {
            IsBusy = true;
            Position position = null;
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);

                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
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
                DependencyService.Get<IMessage>().ShortAlert("Erreur gps !");
            }
            finally
            {
                if (position != null)
                {
                    NewContact.location.coordinates.lat = position.Latitude.ToString().Replace(",", ".");
                    NewContact.location.coordinates.lng = position.Longitude.ToString().Replace(",", ".");
                    Position = NewContact.location.coordinates.lat + " ; " + NewContact.location.coordinates.lng;
                }
            }
            IsBusy = false;
        }

        private async Task ExecuteOnTakePicture()
        {
            if (await Permissions.Check_permissions("Storage") == PermissionStatus.Granted)
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
                    DependencyService.Get<IMessage>().ShortAlert("Erreur Caméra !");
                }
        }

        private async Task ExecuteOnPictureTapped()
        {
            if (Picture != null)
            {
                IsBusy = true;
                await Navigation.PushModalAsync(new Picture_View(Picture), true);
                IsBusy = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}