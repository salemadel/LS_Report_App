using LS_Report.Data;
using LS_Report.Interfaces;
using LS_Report.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.ViewModels.Messages_ViewModel
{
    public class SentMessage_ViewModel : INotifyPropertyChanged
    {
        private IDataStore DataStore { get; set; }
        private ObservableCollection<Mail_Model> mails { get; set; }

        public ObservableCollection<Mail_Model> Mails
        {
            get { return mails; }
            set
            {
                if (mails != value)
                    mails = value;
                OnPropertyChanged();
            }
        }

        private Mail_Model selected_item { get; set; }

        public Mail_Model Selected_Item
        {
            get { return selected_item; }
            set
            {
                if (selected_item != value)
                    selected_item = value;
                OnPropertyChanged();
            }
        }

        private bool isVisble { get; set; }

        public bool IsVisible
        {
            get { return isVisble; }
            set
            {
                if (isVisble != value)
                    isVisble = value;
                OnPropertyChanged();
            }
        }

        private bool sendMessageVisible { get; set; }

        public bool SendMessageVisible
        {
            get { return sendMessageVisible; }
            set
            {
                if (sendMessageVisible != value)
                    sendMessageVisible = value;
                OnPropertyChanged();
            }
        }

        public Contacts_List Selected_Contact { get; set; }

        public List<Contacts_List> Contacts_List
        {
            get
            {
                return JsonConvert.DeserializeObject<List<Contacts_List>>(DataStore.GetDataStoredJson("Mail_Contact_List").ToList()[0].json);
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

        public string Title { get; set; }
        public string Message { get; set; }
        public Command ItemTappedCommand { get; set; }
        public Command HidePopUpCommand { get; set; }
        public Command ShowSendCommand { get; set; }
        public Command SendMessageCommand { get; set; }

        public SentMessage_ViewModel(IDataStore dataStore)
        {
            DataStore = dataStore;
            ItemTappedCommand = new Command(() =>
            {
                ExecuteOnItemTapped();
            });
            HidePopUpCommand = new Command(() =>
            {
                ExecuteOnHidePopUp();
            });
            ShowSendCommand = new Command(() =>
            {
                ExecuteOnShowSend();
            });
            SendMessageCommand = new Command(async () =>
            {
                await ExecuteOnSendMessage();
            });
            Initialize();
        }

        private void Initialize()
        {
            var MessagesList = DataStore.GetMails().Where(i => i.Type == "Sent").ToList();
            if (MessagesList.Count > 0)
            {
                Mails = new ObservableCollection<Mail_Model>(JsonConvert.DeserializeObject<List<Mail_Model>>(MessagesList[0].Json));
            }
        }

        private void ExecuteOnItemTapped()
        {
            IsVisible = true;
        }

        private void ExecuteOnHidePopUp()
        {
            IsVisible = false;
            SendMessageVisible = false;
            Selected_Contact = null;
            Title = null;
            Message = null;
            OnPropertyChanged("Selected_Contact");
            OnPropertyChanged("Title");
            OnPropertyChanged("Message");
        }

        private void ExecuteOnShowSend()
        {
            SendMessageVisible = true;
        }

        private async Task ExecuteOnSendMessage()
        {
            if (Selected_Contact != null & Title != null & Message != null)
            {
                IsBusy = true;
                var _restService = new RESTService();
                var Result = await _restService.Send_Mail_Async(Selected_Contact._id, "False", Title, Message, Selected_Contact.Name);
                if (Result.Item1)
                {
                    DependencyService.Get<IMessage>().ShortAlert("Message Envoyé Avec Succes");
                    Initialize();
                    SendMessageVisible = false;
                    Selected_Contact = null;
                    Title = null;
                    Message = null;
                    OnPropertyChanged("Selected_Contact");
                    OnPropertyChanged("Title");
                    OnPropertyChanged("Message");
                }
                else
                {
                    SendMessageVisible = false;
                    DependencyService.Get<IMessage>().ShortAlert(Result.Item2);
                }
                IsBusy = false;
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Veuillez Remplir les Champs Obligatoire !");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}