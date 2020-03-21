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
    public class Visite_ViewModel : INotifyPropertyChanged
    {
        private INavigation Navigation { get; set; }
        private IDataStore DataStore { get; set; }
        private string[] quality = {  "Bonne" , "Moyenne", "Mauvaise" };
        private string[] wait_time = { "< 30 min", "30 - 60 min", "> 60 min" };
        private string[] duration = { "< 15 min", "15 - 30 min", "> 30 min" };

        private string[] categorie = { "Commune", "Sous résidence", "Hors résidence" };
        private string[] doctor_authority = { "Médecin", "Remplacent" };
        private string[] pharmacy_authority = { "Vendeur", "Pharmacien", "Acheteur", "Autre" };

        public List<string> Quality
        {
            get { return quality.ToList(); }
        }

        public List<string> Wait_Time
        {
            get { return wait_time.ToList(); }
        }

        public List<string> Duration
        {
            get { return duration.ToList(); }
        }

        public List<string> Categorie
        {
            get { return categorie.ToList(); }
        }

        public List<Contacts_List> Supercisors_List
        {
            get
            {
                return JsonConvert.DeserializeObject<List<Contacts_List>>(DataStore.GetDataStoredJson("Mail_Contact_List").ToList()[0].json);
            }
        }

        public List<Products_Model> Products_to_Deliver
        {
            get
            {
                if (DataStore.GetDataStoredJson("Products").Count() > 0)
                {
                    return JsonConvert.DeserializeObject<List<Products_Model>>(DataStore.GetDataStoredJson("Products").ToList()[0].json).Where(i => i.category != null && i.category.Contains("à remettre")).ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        private ObservableCollection<object> Selected_Products_To_Deliver { get; set; }
        public Contacts_List Selected_Supervisor { get; set; }

        public List<string> Authority
        {
            get
            {
                if (Business_type == "Médecin" || Business_type == "Sage-femme" || Business_type == "Chirugien dentiste")
                {
                    return doctor_authority.ToList();
                }
                else
                {
                    return pharmacy_authority.ToList();
                }
            }
        }

        public string Business_type
        {
            get { return Contact.business_type; }
        }

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

        private ObservableCollection<tmp_presented_products_doctor> tmp_Presented_Products_Doctor { get; set; }

        public ObservableCollection<tmp_presented_products_doctor> Tmp_Presented_Products_Doctor
        {
            get { return tmp_Presented_Products_Doctor; }
            set
            {
                if (tmp_Presented_Products_Doctor != value)
                    tmp_Presented_Products_Doctor = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<tmp_pretented_products_pharmacy> tmp_Pretented_Products_Pharmacies { get; set; }

        public ObservableCollection<tmp_pretented_products_pharmacy> Tmp_Pretented_Products_Pharmacies
        {
            get { return tmp_Pretented_Products_Pharmacies; }
            set
            {
                if (tmp_Pretented_Products_Pharmacies != value)
                    tmp_Pretented_Products_Pharmacies = value;
                OnPropertyChanged();
            }
        }
        private List<tmp_questionnaire> tmp_Questionnaires { get; set; }
        private bool material_delivred { get; set; }

        public bool Material_Delivred
        {
            get { return material_delivred; }
            set
            {
                if (material_delivred != value)
                {
                    material_delivred = value;
                    if (value == false)
                    {
                        Selected_Products_To_Deliver.Clear();
                        Material_Delivred_Quantity = null;
                        OnPropertyChanged("Material_Delivred_Quantity");
                        MessagingCenter.Send(this, "SelectedItemChanged");
                    }
                    OnPropertyChanged();
                }
            }
        }

        public string Material_Delivred_Quantity { get; set; }
        private List<string> Products_Presented_Ids { get; set; } = new List<string>();
        private bool visite_duo { get; set; }

        public bool Visite_Duo
        {
            get { return visite_duo; }
            set
            {
                if (visite_duo != value)
                {
                    visite_duo = value;
                    if (!value)
                    {
                        Selected_Supervisor = null;
                        OnPropertyChanged("Selected_Supervisor");
                    }
                    OnPropertyChanged();
                }

                OnPropertyChanged();
            }
        }

        public string Selected_Quality { get; set; }
        public string Selected_Duration { get; set; }
        public string Selected_Wait_Time { get; set; }
        public string Selected_Categorie { get; set; }
        public string Selected_Authority { get; set; }
        public string Selected_Receptivity { get; set; }
        public string Agent_Note { get; set; }
        public string VisitedPerson_Note { get; set; }
        private Responce_Model Responce { get; set; }
        private Responce_free_Model Responce_Free { get; set; }

        public bool IsFreeMission { get; set; } = false;
        private Client2 Contact { get; set; }
        private string Global_Id { get; set; }
        private string Mission_Id { get; set; }
        private string Source { get; set; }
        public string VisiteType { get; set; }
        private bool Focus { get; set; } = false;
        private List<Client2> FocusContacts { get; set; }
        public Command PresenteProductCommand { get; set; }
        public Command PresentedProductsListCommand { get; set; }
        public Command AddVisiteCommand { get; set; }
        public Command QuestionnaireCommand { get; set; }
        public Visite_ViewModel(string source , INavigation navigation, IDataStore dataStore, Client2 contact, bool isfreemission, string global_id, string mission_id , List<Client2> focuscontacts = null)
        {
            Navigation = navigation;
            DataStore = dataStore;
            Contact = contact;
            IsFreeMission = isfreemission;
            Global_Id = global_id;
            Mission_Id = mission_id;
            Source = source;
            if(Source == "Focus")
            {
                VisiteType = "Rapport Focus.";
                Focus = true;
                FocusContacts = focuscontacts;
                Contact = FocusContacts.First();
            }
            else
            {
                VisiteType = "Ajouter Visite.";
            }
            Responce = new Responce_Model();
            Responce_Free = new Responce_free_Model();
            Tmp_Presented_Products_Doctor = new ObservableCollection<tmp_presented_products_doctor>();
            Tmp_Pretented_Products_Pharmacies = new ObservableCollection<tmp_pretented_products_pharmacy>();
            tmp_Questionnaires = new List<tmp_questionnaire>();
            if (DataStore.GetDataStoredJson("Questionnaires").ToList().Count > 0 & Source != "Focus")
            {
                var list = JsonConvert.DeserializeObject<List<Questionnaire_Model>>(DataStore.GetDataStoredJson("Questionnaires").ToList()[0].json);
                foreach(var questionnaire in list)
                {
                    foreach(var question in questionnaire.questions)
                    {
                        var data = new tmp_questionnaire
                        {
                            answers = question.answers,
                            question = question.question,
                            id_question = question._id,
                            id_questionnaire = questionnaire._id,
                            title = questionnaire.title
                        };
                        tmp_Questionnaires.Add(data);
                    }
                }
            }
                Selected_Products_To_Deliver = new ObservableCollection<object>();

            PresenteProductCommand = new Command(async () =>
            {
                await ExecuteOnPresenteProduct();
            });
            PresentedProductsListCommand = new Command(async () =>
            {
                await ExecuteOnPresenteProductsList();
            });
            AddVisiteCommand = new Command(async () =>
            {
                await ExecuteOnAddVisite();
            });
            QuestionnaireCommand = new Command(async () =>
            {
                await ExecuteOnQuestionnaire();
            });
            MessagingCenter.Subscribe<PresenteProductDoctor_ViewModel, tmp_presented_products_doctor>(this, "UpdatePresentedDoctor", (sender, args) =>
         {
             Tmp_Presented_Products_Doctor.Add(args);
             Products_Presented_Ids.Add(args.id_product);
             MessagingCenter.Send(this, "ListIdsUpdated", Products_Presented_Ids);
         });
            MessagingCenter.Subscribe<PresenteProductPharmacy_ViewModel, tmp_pretented_products_pharmacy>(this, "UpdatePresentedPharmacy", (sender, args) =>
            {
                Tmp_Pretented_Products_Pharmacies.Add(args);
                Products_Presented_Ids.Add(args.id_product);
                MessagingCenter.Send(this, "ListIdsUpdated", Products_Presented_Ids);
            });
            MessagingCenter.Subscribe<PresentedProductsList_ViewModel, tmp_presented_products_doctor>(this, "DeletePresentedDoctor", (sender, args) =>
            {
                Tmp_Presented_Products_Doctor.Remove(args);
                Products_Presented_Ids.Remove(args.id_product);
            });
            MessagingCenter.Subscribe<PresentedProductsList_ViewModel, tmp_pretented_products_pharmacy>(this, "DeletePresentedPharmacy", (sender, args) =>
            {
                Tmp_Pretented_Products_Pharmacies.Remove(args);
                Products_Presented_Ids.Remove(args.id_product);
            });
            MessagingCenter.Subscribe<Questionnaire_View, List<tmp_questionnaire>>(this, "QuestionnaireUpdate", (sender, args) =>
            {
                tmp_Questionnaires = args;
            });
            MessagingCenter.Subscribe<Visite_View, ObservableCollection<object>>(this, "SelectedItemsChanged", (sender, args) =>
         {
             Selected_Products_To_Deliver = args;
         });
        }

        private async Task ExecuteOnPresenteProduct()
        {
            IsBusy = true;
            if (Business_type == "Médecin" || Business_type == "Sage-femme" || Business_type == "Chirugien dentiste")
            {
                await Navigation.PushModalAsync(new Products_View("Doctor", Products_Presented_Ids), true);
            }
            else
            {
                await Navigation.PushModalAsync(new Products_View("Pharmacy", Products_Presented_Ids), true);
            }
            IsBusy = false;
        }

        private async Task ExecuteOnPresenteProductsList()
        {
            IsBusy = true;
            if (Business_type == "Médecin" || Business_type == "Sage-femme" || Business_type == "Chirugien-dentiste")
            {
                if (Tmp_Presented_Products_Doctor.Count > 0)
                    await Navigation.PushModalAsync(new ProdcutPresentedList_View(Tmp_Presented_Products_Doctor, null), true);
            }
            else
            {
                if (Tmp_Pretented_Products_Pharmacies.Count > 0)
                    await Navigation.PushModalAsync(new ProdcutPresentedList_View(null, Tmp_Pretented_Products_Pharmacies), true);
            }
            IsBusy = false;
        }
        private async Task ExecuteOnQuestionnaire()
        {
            await Navigation.PushModalAsync(new Questionnaire_View(tmp_Questionnaires));
        }

        private async Task ExecuteOnAddVisite()
        {
            if (!Focus)
            {
                List<KeyValuePair<string, string>> Result = new List<KeyValuePair<string, string>>();
                if (IsFreeMission)
                {
                    Result = SerializeFreeMissionVisiteJson();
                }
                else
                {
                    Result = SerializeVisiteJson();
                }
                if (Result != null)
                {
                    if (await DependencyService.Get<IDialog>().AlertAsync("", "Voulez Vous Ajouter le Rapport ?", "Oui", "Non"))
                    {
                        IsBusy = true;
                        string questionnaireJson = SerializeQuestionnaire();
                        if (questionnaireJson != null)
                        {
                            var questionnaire = new QuastionnairesToUpload_Model
                            {
                                Date = DateTime.Now,
                                Json = questionnaireJson,
                                Name = Contact.Name,
                            };
                            DataStore.AddQuastionnairesToUpload(questionnaire);
                        }
                        var report = new ReportToUpload_Model
                        {
                            Client_name = Contact.Name,
                            Date = DateTime.Now,
                            report = Result,
                            Contact_id = Contact._id,
                            Global_id = Global_Id,
                            IsFreeMission = IsFreeMission,
                            Mission_id = Mission_Id,
                        };
                        DataStore.AddReportToUpload(report);
                        MessagingCenter.Send(this, "UploadContactsTableModified");
                        await Navigation.PopModalAsync();
                        IsBusy = false;
                    }
                }
            }
            else
            {
                if (FocusContacts != null & FocusContacts.Count > 0)
                {
                    if (await DependencyService.Get<IDialog>().AlertAsync("", "Voulez Vous Ajouter le Rapport Focus ?", "Oui", "Non"))
                    {
                        IsBusy = true;
                        foreach (var contact in FocusContacts)
                        {
                            List<KeyValuePair<string, string>> Result = new List<KeyValuePair<string, string>>();
                            Result = SerializeFreeMissionVisiteJson();
                            if (Result != null)
                            {
                                var report = new ReportToUpload_Model
                                {
                                    Client_name = contact.Name,
                                    Date = DateTime.Now,
                                    report = Result,
                                    Contact_id = contact._id,
                                    Global_id = Global_Id,
                                    IsFreeMission = true,
                                    Mission_id = Mission_Id,
                                };
                                DataStore.AddReportToUpload(report);
                                MessagingCenter.Send(this, "UploadContactsTableModified");
                            }
                        }
                        await Navigation.PopModalAsync();
                        MessagingCenter.Send(this, "PopModalAsync");
                        IsBusy = false;
                    }
                }
            }
        }

        private List<KeyValuePair<string, string>> SerializeFreeMissionVisiteJson()
        {
            IsBusy = true;
            if (!CheckRequiredData())
            {
                DependencyService.Get<IMessage>().ShortAlert("Vous devez remplir les champs obligatoires !");
                IsBusy = false;
                return null;
            }
            var Material_delivred = QuantityDelivredValidation();
            if (!Material_delivred.Item1)
            {
                DependencyService.Get<IMessage>().ShortAlert("Erreur Materiél Remis !");
                IsBusy = false;
                return null;
            }
            var compareLocation = new CompareLocation();
            var Result = compareLocation.CompareLocationFunction(DataStore, Contact);
            Responce_Free.report_date = DateTime.Now;
            Responce_Free.client.city = Contact.city;
            Responce_Free.client.client = Contact._id;
            Responce_Free.client.focus = Focus;
            if (Result.Item1)
            {
                Responce_Free.client.coords.Add(Result.Item3.lat);
                Responce_Free.client.coords.Add(Result.Item3.lng);
            }
            Responce_Free.client.valid = Result.Item1;
            Responce_Free.client.visited = true;
            Responce_Free.client.visit_end_time = Result.Item5;
            Responce_Free.client.visit_time = Result.Item4;
            Responce_Free.client.wilaya = Contact.wilaya;
            Responce_Free.client.dual = (Selected_Supervisor == null) ? false : true;
            Responce_Free.repport.gps_duration = Result.Item2;
            Responce_Free.repport.products_presented.AddRange(productsPresentedSerialize());
            Responce_Free.repport.material_delivered.AddRange(Material_delivred.Item2);
            Responce_Free.repport.agent_note = Agent_Note;
            Responce_Free.repport.authority = Selected_Authority;
            Responce_Free.repport.category = Selected_Categorie;
            Responce_Free.repport.client_note = VisitedPerson_Note;
            Responce_Free.repport.dual_with = (Selected_Supervisor == null) ? null : Selected_Supervisor._id;
            Responce_Free.repport.duration = Selected_Duration;
            Responce_Free.repport.receptivity = Selected_Receptivity;
            Responce_Free.repport.visit_quality = Selected_Quality;
            Responce_Free.repport.waiting_time = Selected_Wait_Time;
            Responce_Free.repport.focus = Focus;
            List<Product> Products_sales = new List<Product>();
            Responce_Free.order.products.AddRange(Products_sales);
            Responce_Free.order.total = 0;
            List<MissionAnswer> missionAnswer = new List<MissionAnswer>();
            Responce_Free.repport.mission_answers.AddRange(missionAnswer);
            var mission_json = JsonConvert.SerializeObject(Responce_Free.client, Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
            var repport_json = JsonConvert.SerializeObject(Responce_Free.repport, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            var order_json = JsonConvert.SerializeObject(Responce_Free.order, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            var report_date = JsonConvert.SerializeObject(Responce_Free.report_date, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("client", mission_json));
            values.Add(new KeyValuePair<string, string>("repport", repport_json));
            values.Add(new KeyValuePair<string, string>("order", order_json));
            values.Add(new KeyValuePair<string, string>("report_date", report_date));
            IsBusy = false;
            return values;
        }

        private List<KeyValuePair<string, string>> SerializeVisiteJson()
        {
            IsBusy = true;
            if (!CheckRequiredData())
            {
                DependencyService.Get<IMessage>().ShortAlert("Vous devez remplir les champs obligatoires !");
                IsBusy = false;
                return null;
            }
            var Material_delivred = QuantityDelivredValidation();
            if (!Material_delivred.Item1)
            {
                DependencyService.Get<IMessage>().ShortAlert("Erreur Materiél Remis !");
                IsBusy = false;
                return null;
            }
            var compareLocation = new CompareLocation();
            var Result = compareLocation.CompareLocationFunction(DataStore, Contact);
            if (Result.Item1)
            {
                Responce.mission.coords.Add(Result.Item3.lat);
                Responce.mission.coords.Add(Result.Item3.lng);
            }
            Responce.mission.valid = Result.Item1;
            Responce.mission.visited = true;
            Responce.mission.visit_end_time = Result.Item5;
            Responce.mission.visit_time = Result.Item4;
            Responce.mission.dual = (Selected_Supervisor == null) ? false : true;
            Responce.repport.gps_duration = Result.Item2;
            Responce.repport.products_presented.AddRange(productsPresentedSerialize());
            Responce.repport.material_delivered.AddRange(Material_delivred.Item2);
            Responce.repport.agent_note = Agent_Note;
            Responce.repport.authority = Selected_Authority;
            Responce.repport.category = Selected_Categorie;
            Responce.repport.client_note = VisitedPerson_Note;
            Responce.repport.dual_with = (Selected_Supervisor == null) ? null : Selected_Supervisor._id;
            Responce.repport.duration = Selected_Duration;
            Responce.repport.receptivity = Selected_Receptivity;
            Responce.repport.visit_quality = Selected_Quality;
            Responce.repport.waiting_time = Selected_Wait_Time;
            List<Product> Products_sales = new List<Product>();
            Responce.order.products.AddRange(Products_sales);
            Responce.order.total = 0;
            List<MissionAnswer> missionAnswer = new List<MissionAnswer>();
            Responce.repport.mission_answers.AddRange(missionAnswer);
            var mission_json = JsonConvert.SerializeObject(Responce.mission, Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
            var repport_json = JsonConvert.SerializeObject(Responce.repport, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            var order_json = JsonConvert.SerializeObject(Responce.order, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("mission", mission_json));
            values.Add(new KeyValuePair<string, string>("repport", repport_json));
            values.Add(new KeyValuePair<string, string>("order", order_json));

            IsBusy = false;
            return values;
        }

        private (bool, List<Material_Delivred>) QuantityDelivredValidation()
        {
            List<string> quantity_List = new List<string>();
            bool valid = true;
            List<Material_Delivred> material_Delivreds = new List<Material_Delivred>();
            try
            {
                if (!string.IsNullOrWhiteSpace(Material_Delivred_Quantity))
                {
                    quantity_List = Material_Delivred_Quantity.Split(';').ToList();
                }
            }
            catch
            {
                valid = false;
            }
            if (valid && quantity_List.Count > 0)
            {
                foreach (var quantity in quantity_List)
                {
                    int value;
                    if (!Int32.TryParse(quantity, out value))
                    {
                        valid = false;
                        break;
                    }
                }
            }
            if (valid && quantity_List.Count != Selected_Products_To_Deliver.Count)
            {
                valid = false;
            }
            if (valid)
            {
                var quantities_delivred = quantity_List.Select(x => Int32.Parse(x)).ToArray();

                for (int i = 0; i < quantities_delivred.Length; i++)
                {
                    var material = new Material_Delivred();
                    material.product = ((Products_Model)Selected_Products_To_Deliver[i])._id;
                    material.quantity = quantities_delivred[i];
                }
            }
            return (valid, material_Delivreds);
        }

        private List<products_presented> productsPresentedSerialize()
        {
            var Product_presented_List = new List<products_presented>();
            if (Business_type == "Médecin" || Business_type == "Sage-femme" || Business_type == "Chirugien-dentiste")
            {
                foreach (var product_presented in Tmp_Presented_Products_Doctor)
                {
                    var Product_presented = new products_presented();
                    Product_presented.product = product_presented.id_product;
                    Product_presented.doctor.no_interessed_reason = (product_presented.no_interessed_reason == "") ? null : product_presented.no_interessed_reason;
                    Product_presented.doctor.no_prescribed_reason = (product_presented.no_prescribed_reason == "") ? null : product_presented.no_prescribed_reason;
                    Product_presented.doctor.prescribed = product_presented.prescribed;
                    Product_presented.doctor.rival_product = (product_presented.rival_product == "") ? null : product_presented.rival_product;
                    Product_presented.doctor.prescription_frequency = (product_presented.prescribed_frequency == "") ? null : product_presented.prescribed_frequency;
                    Product_presented.doctor.samples = product_presented.samples_delivred;
                    Product_presented.doctor.rival_product = (product_presented.rival_product == "") ? null : product_presented.rival_product;
                    Product_presented.pharmacy = null;
                    Product_presented_List.Add(Product_presented);
                }
            }
            else
            {
                foreach (var product_presented in tmp_Pretented_Products_Pharmacies)
                {
                    var Product_presented = new products_presented();
                    Product_presented.product = product_presented.id_product;
                    Product_presented.pharmacy.rotation = (product_presented.rotation == "") ? null : product_presented.rotation;
                    Product_presented.pharmacy.available = product_presented.available;
                    Product_presented.pharmacy.rival_product = (product_presented.rival_product == "") ? null : product_presented.rival_product;
                    Product_presented.pharmacy.refusal_reason = (product_presented.refusal_reason == "") ? null : product_presented.refusal_reason;
                    Product_presented.pharmacy.rival_product = (product_presented.rival_product == "") ? null : product_presented.rival_product;
                    Product_presented.pharmacy.unavailability_reason = (product_presented.unavailability_reason == "") ? null : product_presented.unavailability_reason;
                    Product_presented.pharmacy.stock = product_presented.stock;
                    Product_presented.pharmacy.reorder = product_presented.reorder;
                    Product_presented.doctor = null;
                    Product_presented_List.Add(Product_presented);
                }
            }
            return Product_presented_List;
        }
        private string SerializeQuestionnaire()
        {
            if(tmp_Questionnaires.All(i => i.answer != null))
            {
                var Questionnaire_Responce = new List<Questionnaire_Responce_Model>();
                
                foreach(var questionnaire in tmp_Questionnaires)
                {
                    
                    if(!Questionnaire_Responce.Exists(i => i.questionnaire == questionnaire.id_questionnaire))
                    {
                        var data = new Questionnaire_Responce_Model
                        {
                             questionnaire = questionnaire.id_questionnaire,
                             client = Contact._id
                        };
                        Questionnaire_Responce.Add(data);
                    }
                    var question = new Questionnaire_Responce_Model.Questions();
                    question.question = questionnaire.question;
                    question.answer = questionnaire.answer;
                    Questionnaire_Responce.Single(i => i.questionnaire == questionnaire.id_questionnaire).questions.Add(question);
                }
                return JsonConvert.SerializeObject(Questionnaire_Responce);
            }
            else
            {
                return null;
            }
        }
        private bool CheckRequiredData()
        {
            if (Selected_Authority == null | Selected_Duration == null | Selected_Quality == null | Selected_Receptivity == null | Selected_Wait_Time == null)
            {
                return false;
            }
            else
            {
                if (IsFreeMission)
                {
                    if (Selected_Categorie == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}