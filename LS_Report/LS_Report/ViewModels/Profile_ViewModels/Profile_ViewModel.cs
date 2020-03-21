using LS_Report.Data;
using LS_Report.Interfaces;
using LS_Report.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.ViewModels.Profile_ViewModels
{
    internal class Profile_ViewModel : INotifyPropertyChanged
    {
        public Token_Model Token { get; set; }
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

        public string Sectors { get; set; }
        public string PassWord { get; set; }
        public string Confirm_PassWord { get; set; }
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

        public Command ChangePasswordVisibleCommand { get; set; }
        public Command ChangePasswordCommand { get; set; }

        public Profile_ViewModel(Token_Model token)
        {
            Token = token;
            Initialize();
            ChangePasswordVisibleCommand = new Command(() =>
            {
                ExecuteOnChangePasswordVisible();
            });
            ChangePasswordCommand = new Command(async () =>
            {
                await ExecuteOnChangePassword();
            });
        }

        private void ExecuteOnChangePasswordVisible()
        {
            if (IsVisible)
                IsVisible = false;
            else
                IsVisible = true;
        }

        private async Task ExecuteOnChangePassword()
        {
            IsBusy = true;
            if (!string.IsNullOrEmpty(PassWord) && PassWord == Confirm_PassWord)
            {
                if (await DependencyService.Get<IDialog>().AlertAsync("", "Voulez Vous Changer le Mot de Passe ?", "Oui", "Non"))
                {
                    var _restService = new RESTService();
                    var Result = await _restService.Change_password_async(PassWord);
                    if (Result.Item1)
                    {
                        PassWord = string.Empty;
                        Confirm_PassWord = string.Empty;
                        OnPropertyChanged("PassWord");
                        OnPropertyChanged("Confirm_PassWord");
                        IsVisible = false;
                        DependencyService.Get<IMessage>().ShortAlert("Mot de Passe Chnger avec Succes");
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert(Result.Item2);
                    }
                }
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Veuillez Confirmer le Mot de Pass !");
            }
            IsBusy = false;
        }

        private void Initialize()
        {
            Sectors = string.Join(" , ", Token.sectors);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}