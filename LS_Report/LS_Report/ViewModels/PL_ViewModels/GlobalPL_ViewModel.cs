using LS_Report.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LS_Report.ViewModels.PL_ViewModels
{
    public class GlobalPL_ViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<PL_Model> missions { get; set; }

        public ObservableCollection<PL_Model> Missions
        {
            get { return missions; }
            set
            {
                if (missions != value)
                    missions = value;
                OnPropertyChanged();
            }
        }

        public GlobalPL_ViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}