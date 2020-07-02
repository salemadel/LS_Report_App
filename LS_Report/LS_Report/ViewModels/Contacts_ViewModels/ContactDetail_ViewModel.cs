using LS_Report.Data;
using LS_Report.Interfaces;
using LS_Report.Models;
using LS_Report.Views;
using LS_Report.Views.VisitesPages;
using Newtonsoft.Json;
using Plugin.ExternalMaps;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.ViewModels.Contacts_ViewModels
{
    public class ContactDetail_ViewModel : INotifyPropertyChanged
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

        public string Potential { get; set; }

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

        private Token_Model Token { get; set; }
        public Command GoToPositionCommand { get; set; }
        public Command PictureTappedCommand { get; set; }
        public Command GetHistoryCommand { get; set; }

        public ContactDetail_ViewModel(INavigation navigation, Client2 contact, Token_Model token)
        {
            Navigation = navigation;
            Contact = contact;
            Token = token;

            Potential = (Contact.potential.Exists(i => i.network == Token.network)) ? Contact.potential.Single(i => i.network == Token.network).value : null;
            GoToPositionCommand = new Command(async () =>
            {
                await ExecuteOnGoToPosition();
            });
            PictureTappedCommand = new Command(async () =>
            {
                await ExecuteOnPictureTapped();
            });
            GetHistoryCommand = new Command(async () =>
            {
                await ExecuteOnGetHistory();
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

        private async Task ExecuteOnPictureTapped()
        {
            if (!string.IsNullOrWhiteSpace(Contact.avatar))
            {
                await Navigation.PushModalAsync(new Picture_View(Contact.Avatar), true);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}