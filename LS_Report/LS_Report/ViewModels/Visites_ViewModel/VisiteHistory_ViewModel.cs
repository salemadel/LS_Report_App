using LS_Report.Converters;
using LS_Report.Data;
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
    internal class VisiteHistory_ViewModel : INotifyPropertyChanged
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

        public ContactVisiteHistory_Model Selected_Item { get; set; }
        public DateTime Selected_MinDate { get; set; }
        public DateTime Selected_MaxDate { get; set; }
        private List<ContactVisiteHistory_Model> Contacts_List { get; set; }
        private ObservableCollection<Grouping<string, ContactVisiteHistory_Model>> contacts { get; set; }

        public ObservableCollection<Grouping<string, ContactVisiteHistory_Model>> Contacts
        {
            get { return contacts; }
            set
            {
                if (contacts != value)
                {
                    contacts = value;
                    if (value.Count > 0)
                    {
                        IsVisible = true;
                    }
                    else
                    {
                        IsVisible = false;
                    }
                    OnPropertyChanged();
                }
            }
        }

        public Command GetHistoryCommand { get; set; }
        public Command ItemTappedCommand { get; set; }
        public Command StatisticsCommand { get; set; }

        public VisiteHistory_ViewModel(INavigation navigation, IDataStore dataStore)
        {
            Navigation = navigation;
            DataStore = dataStore;
            GetHistoryCommand = new Command(async () =>
            {
                await ExecuteOnGetHistory();
            });
            ItemTappedCommand = new Command(async () =>
            {
                await ExecuteOnItemTapped();
            });
            StatisticsCommand = new Command(async () =>
            {
                await ExecuteOnstatsTapped();
            });
            Initialize();
        }

        private void Initialize()
        {
            Contacts = new ObservableCollection<Grouping<string, ContactVisiteHistory_Model>>();
            Selected_MinDate = DateTime.Now;
            Selected_MaxDate = DateTime.Now;
            var List = DataStore.GetDataStoredJson("VisiteToday").Where(i => i.DateTime.Date == DateTime.Now.Date).ToList();
            if (List.Count > 0)
            {
                try
                {
                    Contacts_List = JsonConvert.DeserializeObject<List<ContactVisiteHistory_Model>>(List[0].json);
                    var sorted = from contact in Contacts_List
                                 orderby contact.DateSort
                                 group contact by contact.DateSort into contactGroup
                                 select new Grouping<string, ContactVisiteHistory_Model>(contactGroup.Key, contactGroup);
                    Contacts = new ObservableCollection<Grouping<string, ContactVisiteHistory_Model>>(sorted);
                }
                catch (Exception e)
                {
                    DependencyService.Get<IMessage>().ShortAlert(e.Message);
                }
            }
        }

        private async Task ExecuteOnGetHistory()
        {
            IsBusy = true;
            if (Selected_MinDate <= Selected_MaxDate)
            {
                var _restService = new RESTService();
                var Result = await _restService.GetPeriodHistryAsync(Selected_MinDate, Selected_MaxDate);
                if (Result.Item1)
                {
                    try
                    {
                        Contacts_List = JsonConvert.DeserializeObject<List<ContactVisiteHistory_Model>>(Result.Item2);
                        var sorted = from contact in Contacts_List
                                     orderby contact.DateSort
                                     group contact by contact.DateSort into contactGroup
                                     select new Grouping<string, ContactVisiteHistory_Model>(contactGroup.Key, contactGroup);
                        Contacts = new ObservableCollection<Grouping<string, ContactVisiteHistory_Model>>(sorted);
                    }
                    catch (Exception e)
                    {
                        DependencyService.Get<IMessage>().ShortAlert(e.Message);
                    }
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert(Result.Item2);
                }
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Erreur Date !");
            }
            IsBusy = false;
        }

        private async Task ExecuteOnstatsTapped()
        {
            IsBusy = true;
            await Navigation.PushModalAsync(new Statistics_View(Contacts_List), true);
            IsBusy = false;
        }

        private async Task ExecuteOnItemTapped()
        {
            IsBusy = true;
            await Navigation.PushModalAsync(new ContactHistory_View(Selected_Item), true);
            IsBusy = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}