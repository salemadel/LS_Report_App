using LS_Report.Data;
using LS_Report.Interfaces;
using LS_Report.Models;
using Plugin.SecureStorage;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.ViewModels
{
    public class Login_ViewModel : INotifyPropertyChanged
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

        private string username { get; set; }

        public string UserName
        {
            get { return username; }
            set
            {
                if (username != value)
                    username = value;
                OnPropertyChanged();
            }
        }

        private string password { get; set; }

        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                    password = value;
                OnPropertyChanged();
            }
        }

        private string responce { get; set; }

        public string Responce
        {
            get { return responce; }
            set
            {
                if (responce != value)
                    responce = value;
                OnPropertyChanged();
            }
        }

        public Login_ViewModel(INavigation navigation, IDataStore dataStore)
        {
            Navigation = navigation;
            DataStore = dataStore;
            if (CrossSecureStorage.Current.HasKey("UserName") & CrossSecureStorage.Current.HasKey("PassWord"))
            {
                UserName = CrossSecureStorage.Current.GetValue("UserName");
                Password = CrossSecureStorage.Current.GetValue("PassWord");
            }
            LoginCommand = new Command(async () =>
            {
                await ExecuteOnLogin();
            });
        }

        public Command LoginCommand { get; set; }

        private async Task ExecuteOnLogin()
        {
            if (!string.IsNullOrEmpty(UserName) & !string.IsNullOrEmpty(Password))
            {
                IsBusy = true;
                var _restService = new RESTService();
                var version = await _restService.CheckVersion();
                if (version.Item1)
                {
                    var responce = await _restService.Login(UserName.TrimEnd(), Password.TrimEnd());
                    if (responce.Item1)
                    {
                        var responce2 = await _restService.Get_Contacts_Async();
                        if (responce2.Item1)
                        {
                            if (DataStore.GetDataStoredJson("Mail_Contact_List").ToList().Count > 0)
                            {
                                DataStore.UpdateData("Mail_Contact_List", responce2.Item2);
                            }
                            else
                            {
                                var Data = new Stored_Data_Model
                                {
                                    DateTime = DateTime.Now,
                                    json = responce2.Item2,
                                    Type = "Mail_Contact_List"
                                };
                                DataStore.AddData(Data);
                            }
                            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                            {
                                var coords_list = DataStore.GetLocations().Where(i => i.Date.Date < DateTime.Now.Date).ToList();
                                if (coords_list.Count > 0)
                                {
                                    foreach (var coords in coords_list)
                                    {
                                        DataStore.DeleteLocation(coords.Id);
                                    }
                                }
                            }
                            Application.Current.MainPage = new NavigationPage(new MainPage());
                        }
                        else
                        {
                            Responce = responce2.Item2;
                        }
                        var responce3 = await _restService.GetQuestionnairesAsyc();
                        if (responce3.Item1)
                        {
                            if (DataStore.GetDataStoredJson("Questionnaires").ToList().Count > 0)
                            {
                                DataStore.UpdateData("Questionnaires", responce3.Item2);
                            }
                            else
                            {
                                var Data = new Stored_Data_Model
                                {
                                    DateTime = DateTime.Now,
                                    json = responce3.Item2,
                                    Type = "Questionnaires"
                                };
                                DataStore.AddData(Data);
                            }
                        }
                    }
                    else
                    {
                        Responce = responce.Item2;
                    }
                }
                else
                {
                    Responce = version.Item2;
                }
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