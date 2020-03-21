using LS_Report.Converters;
using LS_Report.Interfaces;
using LS_Report.Models;
using LS_Report.Services;
using LS_Report.ViewModels.Contacts_ViewModels;
using LS_Report.ViewModels.Visites_ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace LS_Report.ViewModels.MainMenu_ViewModels
{
    public class Sync_ViewModel : INotifyPropertyChanged
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

        public bool IsVisible { get; set; }
        private ObservableCollection<Grouping<string, PendingData_Model>> pendingData { get; set; }

        public ObservableCollection<Grouping<string, PendingData_Model>> PendingData
        {
            get { return pendingData; }
            set
            {
                if (pendingData != value)
                    pendingData = value;
                OnPropertyChanged();
            }
        }

        private string sync_Number { get; set; }

        public string Sync_Number
        {
            get { return sync_Number; }
            set
            {
                if (sync_Number != value)
                    sync_Number = value;
                OnPropertyChanged();
            }
        }

        public PendingData_Model Selected_Item { get; set; }
        public Command DeletePendingDataCommand { get; set; }

        public Sync_ViewModel(INavigation navigation, IDataStore dataStore)
        {
            Navigation = navigation;
            DataStore = dataStore;
            Initilize();
            DeletePendingDataCommand = new Command(() =>
            {
                ExecuteOnDeletePendingData();
            });
            MessagingCenter.Subscribe<AddContact_ViewModel>(this, "UploadContactsTableModified", (sender) =>
            {
                Initilize();
            });
            MessagingCenter.Subscribe<EditContact_ViewModel>(this, "UploadContactsTableModified", (sender) =>
            {
                Initilize();
            });
            MessagingCenter.Subscribe<Visite_ViewModel>(this, "UploadContactsTableModified", (sender) =>
            {
                Initilize();
            });
            MessagingCenter.Subscribe<ContactVisite_ViewModel>(this, "UploadContactsTableModified", (sender) =>
            {
                Initilize();
            });
            MessagingCenter.Subscribe<UploadDataService>(this, "ContactsSyncUpdated", (sender) =>
           {
               Initilize();
           });
        }

        private void Initilize()
        {
            PendingData = new ObservableCollection<Grouping<string, PendingData_Model>>();
            var tmp_PendingData = new ObservableCollection<PendingData_Model>();
            var Add_Contact = DataStore.GetContactsToUpload().ToList();
            var Edit_Contact = DataStore.GetEditContactsToUpload().ToList();
            var Reports = DataStore.GetReportsToUpload().ToList();
            var Unvailibilitys = DataStore.GetUnvailibilityToUpload().ToList();
            var Questionnaires = DataStore.GetQuastionnairesToUpload().ToList();
            if (Add_Contact.Count > 0)
            {
                foreach (var data in Add_Contact)
                {
                    var pending = new PendingData_Model
                    {
                        Title = "Ajout de Contact",
                        Content = data.Name,
                        Error = data.Last_Error,
                        Id = data.Id,
                        Last_Sync = data.Last_Sync
                    };
                    tmp_PendingData.Add(pending);
                }
                Sync_Number = Add_Contact.Count.ToString();
            }
            else
            {
                Sync_Number = null;
            }
            if (Edit_Contact.Count > 0)
            {
                foreach (var data in Edit_Contact)
                {
                    var pending = new PendingData_Model
                    {
                        Title = "Mise a jour Contact",
                        Content = data.Name,
                        Error = data.Last_Error,
                        Id = data.Id,
                        Last_Sync = data.Last_Sync
                    };
                    tmp_PendingData.Add(pending);
                }
                Sync_Number = (Convert.ToInt32(Sync_Number) + Edit_Contact.Count).ToString();
            }
            if (Reports.Count > 0)
            {
                foreach (var data in Reports)
                {
                    var pending = new PendingData_Model
                    {
                        Title = "Ajout de Rapport",
                        Content = data.Client_name,
                        Error = data.Last_Error,
                        Id = data.Id,
                        Last_Sync = data.Last_Sync
                    };
                    tmp_PendingData.Add(pending);
                }
                Sync_Number = (Convert.ToInt32(Sync_Number) + Reports.Count).ToString();
            }
            if (Unvailibilitys.Count > 0)
            {
                foreach (var data in Unvailibilitys)
                {
                    var pending = new PendingData_Model
                    {
                        Title = "Indisponibilité",
                        Content = data.Client_name,
                        Error = data.Last_Error,
                        Id = data.Id,
                        Last_Sync = data.Last_Sync
                    };
                    tmp_PendingData.Add(pending);
                }
                Sync_Number = (Convert.ToInt32(Sync_Number) + Unvailibilitys.Count).ToString();
            }
            if(Questionnaires.Count > 0)
            {
                foreach(var data in Questionnaires)
                {
                    var pending = new PendingData_Model
                    {
                        Title = "Questionnaire",
                        Content = data.Name,
                        Error = data.Last_Error,
                        Id = data.Id,
                        Last_Sync = data.Last_Sync
                    };
                    tmp_PendingData.Add(pending);
                }
                Sync_Number = (Convert.ToInt32(Sync_Number) + Questionnaires.Count).ToString();
            }
            var sorted = from pending in tmp_PendingData
                         orderby pending.Title
                         group pending by pending.Title into contactGroup
                         select new Grouping<string, PendingData_Model>(contactGroup.Key, contactGroup);

            PendingData = new ObservableCollection<Grouping<string, PendingData_Model>>(sorted);
            if (Sync_Number != null)
            {
                IsVisible = true;
                OnPropertyChanged("IsVisible");
            }
            else
            {
                IsVisible = false;
                OnPropertyChanged("IsVisible");
            }
        }

        private void ExecuteOnDeletePendingData()
        {
            if (Selected_Item != null)
            {
                switch (Selected_Item.Title)
                {
                    case "Ajout de Contact": DataStore.DeleteContactToUpdate(Selected_Item.Id); break;
                    case "Mise a jour Contact": DataStore.DeleteEditContactToUpdate(Selected_Item.Id); break;
                    case "Ajout de Rapport": DataStore.DeleteReportToUload(Selected_Item.Id); break;
                    case "Indisponibilité": DataStore.DeleteUnvailibilityToUload(Selected_Item.Id); break;
                    case "Questionnaire":DataStore.DeleteQuestionnaireToUpload(Selected_Item.Id);break;
                }
                Selected_Item = null;
                OnPropertyChanged("Selected_Item");
                Initilize();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}