using LS_Report.Data;
using LS_Report.Interfaces;
using LS_Report.Models;
using LS_Report.Views;
using LS_Report.Views.ProfilePages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.ViewModels.MainMenu_ViewModels
{
    public class ProfilePage_ViewModdel : INotifyPropertyChanged
    {
        private INavigation Navigation { get; set; }
        private IDataStore DataStore { get; set; }
        private Token_Model Token { get; set; }
        public bool IsFreeMission { get; set; }
        public List<Missions_Model> Missions { get; set; }
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

        public bool TodayChecked { get; set; }
        public bool WeekChecked { get; set; }
        public Command DisconnectCommand { get; set; }
        public Command ProfilViewCommand { get; set; }
        public Command MyStatisticsCommand { get; set; }
        public Command PopUpVisibleCommand { get; set; }
        public Command EndMissionCommand { get; set; }
        public Command HidePopUpCommand { get; set; }

        public ProfilePage_ViewModdel(INavigation navigation, IDataStore dataStore)
        {
            Navigation = navigation;
            DataStore = dataStore;
            Initialize();
            DisconnectCommand = new Command(async () =>
            {
                await ExecuteOnDisconnect();
            });
            ProfilViewCommand = new Command(async () =>
            {
                await ExecuteOnViewProfil();
            });
            MyStatisticsCommand = new Command(async () =>
            {
                await ExecuteOnMyStatisticsTapped();
            });
            PopUpVisibleCommand = new Command(() =>
            {
                ExecuteOnPopUpVisible();
            });
            EndMissionCommand = new Command(async () =>
            {
                await ExecuteOnEndMission();
            });
            HidePopUpCommand = new Command(() =>
            {
                ExecuteOnPopUpInvisible();
            });
        }

        private void Initialize()
        {
            var _tokenController = new TokenController();
            var Result = _tokenController.DecodeToken();
            if (Result.Item1)
            {
                Token = Result.Item2;
                if (Result.Item2.permissions.Contains("Tournée Libre"))
                    IsFreeMission = true;
            }
            else
            {
                return;
            }
        }

        private async Task ExecuteOnDisconnect()
        {
            IsBusy = true;
            var _restService = new RESTService();
            var Result = await _restService.Disconnect_async();
            if (Result.Item1)
            {
                Application.Current.MainPage = new Login_View();
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert(Result.Item2);
            }
            IsBusy = false;
        }

        private async Task ExecuteOnViewProfil()
        {
            await Navigation.PushModalAsync(new Profile_View(Token), true);
        }

        private async Task ExecuteOnMyStatisticsTapped()
        {
            IsBusy = true;
            var _restService = new RESTService();
            var firstdayofmonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var lastdayofmonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            var Result = await _restService.GetStatsPeriode(firstdayofmonth, lastdayofmonth, Token.id);
            if (Result.Item1)
            {
                try
                {
                    List<Stats_Model> stats = JsonConvert.DeserializeObject<List<Stats_Model>>(Result.Item2);
                    if (stats.Count > 0)
                    {
                        await Navigation.PushModalAsync(new MyStatistics_View(stats, Token), true);
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Pas De Statistiques pour ce mois !");
                    }
                }
                catch
                {
                    DependencyService.Get<IMessage>().ShortAlert("Pas De Statistiques pour ce mois !");
                }
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert(Result.Item2);
            }
            IsBusy = false;
        }

        private void ExecuteOnPopUpVisible()
        {
            IsBusy = true;
            var mission = DataStore.GetDataStoredJson("Missions").ToList();
            if (mission.Count > 0)
            {
                Missions = JsonConvert.DeserializeObject<List<Missions_Model>>(mission[0].json);
                if (!Missions.Exists(i => i.global_mission_start.Date <= DateTime.Now.Date & i.global_mission_deadline.Date >= DateTime.Now.Date) && Missions.Where(i => i.global_mission_start.Date <= DateTime.Now.Date & i.global_mission_deadline.Date >= DateTime.Now.Date & i.missions.Exists(j => j.mission_deadline.Date == DateTime.Now.Date)).ToList().Count > 0)
                {
                    DependencyService.Get<IMessage>().ShortAlert("Vous N'avez Aucune Tournée à Terminer");
                }
                else
                {
                    PopUpVisible = true;
                }
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Vous N'avez Aucune Tournée à Terminer");
            }
            IsBusy = false;
        }

        private void ExecuteOnPopUpInvisible()
        {
            PopUpVisible = false;
        }

        private async Task ExecuteOnEndMission()
        {
            if (TodayChecked | WeekChecked)
            {
                TodayChecked = (WeekChecked == true) ? true : TodayChecked;
                IsBusy = true;
                PopUpVisible = false;
                var _restService = new RESTService();
                var Global_mission = Missions.Where(i => i.global_mission_start.Date <= DateTime.Now.Date & i.global_mission_deadline.Date >= DateTime.Now.Date & i.missions.Exists(j => j.mission_deadline.Date == DateTime.Now.Date)).ToList();
                foreach (var global in Global_mission)
                {
                    var missions = global.missions.Where(i => i.mission_deadline.Date == DateTime.Now.Date).ToList();
                    foreach (var mission in missions)
                    {
                        var Result = await _restService.Post_mission_completed(mission._id, global._id, WeekChecked, TodayChecked);
                        if (!Result.Item1)
                        {
                            DependencyService.Get<IMessage>().ShortAlert(Result.Item2);
                            break;
                        }
                    }
                }
                IsBusy = false;
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Veuillez Selectioner un type de Tournée à Terminer !");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}