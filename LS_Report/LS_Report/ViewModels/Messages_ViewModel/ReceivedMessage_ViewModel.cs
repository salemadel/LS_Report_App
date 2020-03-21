using LS_Report.Interfaces;
using LS_Report.Models;
using LS_Report.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace LS_Report.ViewModels.Messages_ViewModel
{
    public class ReceivedMessage_ViewModel : INotifyPropertyChanged
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

        public Command ItemTappedCommand { get; set; }
        public Command HidePopUpCommand { get; set; }

        public ReceivedMessage_ViewModel(IDataStore dataStore)
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
            Initialize();
            MessagingCenter.Subscribe<MessagesListnerService>(this, "MessageReceived", (sender) =>
          {
              Initialize();
          });
        }

        private void Initialize()
        {
            var MessagesList = DataStore.GetMails().Where(i => i.Type == "Received").ToList();
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
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}