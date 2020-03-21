using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LS_Report.ViewModels
{
    public class MainMenu_ViewModel : INotifyPropertyChanged
    {
        public MainMenu_ViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}