using LS_Report.Data;
using LS_Report.Interfaces;
using LS_Report.Models;
using LS_Report.Views.VisitesPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.ViewModels.Visites_ViewModel
{
    public class Products_ViewModel : INotifyPropertyChanged
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

        private ObservableCollection<Products_Model> products { get; set; }

        public ObservableCollection<Products_Model> Products
        {
            get { return products; }
            set
            {
                if (products != value)
                    products = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Products_Model> filtred_List { get; set; }

        public ObservableCollection<Products_Model> Filtred_List
        {
            get { return filtred_List; }
            set
            {
                if (filtred_List != value)
                    filtred_List = value;
                OnPropertyChanged();
            }
        }

        private Products_Model selected_Item { get; set; }

        public Products_Model Selected_Item
        {
            get { return selected_Item; }
            set
            {
                if (selected_Item != value)
                    selected_Item = value;
                OnPropertyChanged();
            }
        }

        private string searchBarText { get; set; }

        public string SearchBarText
        {
            get { return searchBarText; }
            set
            {
                if (searchBarText != value)
                    searchBarText = value;

                OnPropertyChanged();
            }
        }

        private List<string> Products_Presented_Ids { get; set; }

        private string Source { get; set; }
        public Command GetProductsCommand { get; set; }
        public Command SearchCommand { get; set; }
        public Command ItemTappedCommand { get; set; }

        public Products_ViewModel(INavigation navigation, IDataStore dataStore, string source, List<string> ids = null)
        {
            Navigation = navigation;
            DataStore = dataStore;
            Source = source;
            Products_Presented_Ids = ids;
            Filtred_List = new ObservableCollection<Products_Model>();
            Products = new ObservableCollection<Products_Model>();
            GetProductsCommand = new Command(async () =>
            {
                await GetProductsAsync();
            });
            SearchCommand = new Command(() =>
            {
                ExecuteOnSearch();
            });
            ItemTappedCommand = new Command(async () =>
            {
                await ExecuteOnItemTapped();
            });
            MessagingCenter.Subscribe<Visite_ViewModel, List<string>>(this, "ListIdsUpdated", (sender, args) =>
           {
               Products_Presented_Ids = args;
               UpdateProductsListview();
           });
            UpdateProductsListview();
        }

        private void UpdateProductsListview()
        {
            IsBusy = true;
            var query = DataStore.GetDataStoredJson("Products").ToList();
            if (query.Count > 0)
            {
                var _products = JsonConvert.DeserializeObject<List<Products_Model>>(query[0].json);
                Products.Clear();
                Filtred_List.Clear();
                foreach (var product in _products)
                {
                    if (Products_Presented_Ids != null && Products_Presented_Ids.Count > 0)
                    {
                        if (!Products_Presented_Ids.Contains(product._id))
                        {
                            Products.Add(product);
                            Filtred_List.Add(product);
                        }
                    }
                    else
                    {
                        Products.Add(product);
                        Filtred_List.Add(product);
                    }
                }
                SearchBarText = string.Empty;
            }
            IsBusy = false;
        }

        private async Task GetProductsAsync()
        {
            IsBusy = true;
            var _restService = new RESTService();
            var result = await _restService.GetProductsAsync();

            if (result.Item1 == true)
            {
                var json = result.Item2;

                if (DataStore.GetDataStoredJson("Products").ToList().Count > 0)
                {
                    DataStore.UpdateData("Products", json);
                }
                else
                {
                    var Data = new Stored_Data_Model
                    {
                        Type = "Products",
                        json = json
                    };
                    DataStore.AddData(Data);
                }
                UpdateProductsListview();
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert(result.Item2);
            }
            IsBusy = false;
        }

        private void ExecuteOnSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchBarText))
            {
                Filtred_List = new ObservableCollection<Products_Model>(Products);
            }
            else
            {
                var _filtred_List = Products.Where(i => i.name.ToLower().Contains(SearchBarText.ToLower())).ToList();
                Filtred_List.Clear();
                foreach (var product in _filtred_List)
                {
                    Filtred_List.Add(product);
                }
            }
        }

        private async Task ExecuteOnItemTapped()
        {
            IsBusy = true;
            if (Source == "Doctor")
            {
                await Navigation.PushModalAsync(new PresenteProductDoctro_View(Selected_Item), true);
            }
            if (Source == "Pharmacy")
            {
                await Navigation.PushModalAsync(new PresenteProductPharmacy_View(Selected_Item), true);
            }
            if (Source == "Products_List")
            {
                await Navigation.PushModalAsync(new ProductDetail_View(Selected_Item), true);
            }
            IsBusy = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}