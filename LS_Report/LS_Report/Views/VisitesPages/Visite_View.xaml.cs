using LS_Report.Data;
using LS_Report.Interfaces;
using LS_Report.Models;
using LS_Report.ViewModels.Visites_ViewModel;
using Syncfusion.XForms.ComboBox;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.VisitesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Visite_View : ContentPage
    {
        public Visite_View(string source , Client2 contact, bool isfreemission, string global_id, string mission_id , List<Client2> focuscontacts = null)
        {
            InitializeComponent();
            var datastore = new DataStore();
            BindingContext = new Visite_ViewModel(source, Navigation, datastore, contact, isfreemission, global_id, mission_id , focuscontacts);
            MessagingCenter.Subscribe<Visite_ViewModel>(this, "SelectedItemChanged", (sender) =>
          {
              Material_Combobox.SelectedItem = null;
          });
        }

        private void SfComboBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            ObservableCollection<object> SelectedItems = new ObservableCollection<object>();
            SelectedItems = (sender as SfComboBox).SelectedItem as ObservableCollection<object>;
            if ((sender as SfComboBox).SelectedItem != null)
                MessagingCenter.Send(this, "SelectedItemsChanged", SelectedItems);
        }
    }
}