using LS_Report.Data;
using LS_Report.Interfaces;
using LS_Report.Models;
using LS_Report.Views;
using Newtonsoft.Json;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.Services
{
    public class UploadDataService
    {
        private IDataStore DataStore { get; set; }
        private RESTService RESTService { get; set; }

        public UploadDataService(IDataStore dataStore)
        {
            DataStore = dataStore;
            RESTService = new RESTService();
        }

        public async Task UploadDataAsync()
        {
            string LogguedIn = null;
            if (CrossSecureStorage.Current.HasKey("LogguedIn"))
            {
                LogguedIn = CrossSecureStorage.Current.GetValue("LogguedIn");
            }
            if (LogguedIn == "True")
            {
                var Add_Contacts = DataStore.GetContactsToUpload().ToList();
                var Edit_Contacts = DataStore.GetEditContactsToUpload().ToList();
                var Reports = DataStore.GetReportsToUpload().ToList();
                var Unvailibilitys = DataStore.GetUnvailibilityToUpload().ToList();
                var Questionnaire = DataStore.GetQuastionnairesToUpload().ToList();
                var _tokenController = new TokenController();
                bool IsFreeMission = false;
                var Token = _tokenController.DecodeToken();
                if (Token.Item1)
                {
                    if (Token.Item2.permissions.Contains("Tournée Libre"))
                        IsFreeMission = true;
                }
                else
                {
                    CrossSecureStorage.Current.SetValue("LogguedIn", "False");
                    CrossSecureStorage.Current.DeleteKey("acces_token");
                    Application.Current.MainPage = new Login_View();
                    return;
                }
                string[] error_list = { "Contact existe déjà", "Contact non trouvé", "Client existe déjà", "Prénom ne doit pas étre vide", "Nom ne doit pas étre vide", "Sexe ne doit pas étre vide", "Sécteur ne doit pas étre vide", "Adresse ne doit pas étre vide", "Commune ne doit pas étre vide", "Spécialité ne doit pas étre vide", "Potontiel ne doit pas étre vide", "Wilaya ne doit pas étre vide" };
                if (Add_Contacts.Count > 0)
                {
                    foreach (var contact in Add_Contacts)
                    {
                        if (!error_list.Contains(contact.Last_Error))
                        {
                            var obj = JsonConvert.DeserializeObject<NewContact_Model>(contact.Json);
                            Stream picture_stream = null;
                            if (!string.IsNullOrWhiteSpace(contact.PicturePath))
                            {
                                var picture = File.ReadAllBytes(contact.PicturePath);
                                picture_stream = new MemoryStream(picture);
                            }
                            var Result = await RESTService.Post_New_Client(obj, null, picture_stream);
                            if (Result.Item1)
                            {
                                var List = DataStore.GetDataStoredJson("Contacts").ToList();
                                if (List.Count > 0)
                                {
                                    var Contacts_List = JsonConvert.DeserializeObject<List<Client2>>(List[0].json);
                                    Contacts_List.Add(Result.Item3);
                                    var json = JsonConvert.SerializeObject(Contacts_List);
                                    DataStore.UpdateData("Contacts", json);
                                    DataStore.DeleteContactToUpdate(contact.Id);
                                }
                                else
                                {
                                    var Contacts_List = new List<Client2>();
                                    Contacts_List.Add(Result.Item3);
                                    var json = JsonConvert.SerializeObject(Contacts_List);
                                    var data = new Stored_Data_Model
                                    {
                                        DateTime = DateTime.Now,
                                        json = json,
                                        Type = "Contacts"
                                    };
                                    DataStore.AddData(data);
                                    DataStore.DeleteContactToUpdate(contact.Id);
                                }
                                
                            }
                            else
                            {
                                DataStore.UpdateContactToUpload(contact.Id, Result.Item2);
                                
                            }
                        }
                        MessagingCenter.Send(this, "ContactsSyncUpdated");
                    }
                }
                if (Edit_Contacts.Count > 0)
                {
                    foreach (var contact in Edit_Contacts)
                    {
                        if (!error_list.Contains(contact.Last_Error))
                        {
                            var obj = JsonConvert.DeserializeObject<NewContact_Model>(contact.Json);
                            Stream picture_stream = null;
                            if (!string.IsNullOrWhiteSpace(contact.PicturePath))
                            {
                                var picture = File.ReadAllBytes(contact.PicturePath);
                                picture_stream = new MemoryStream(picture);
                            }
                            var Result = await RESTService.Update_client(obj, null, picture_stream);
                            if (Result.Item1)
                            {
                                var List = DataStore.GetDataStoredJson("Contacts").ToList();
                                var Contacts_List = JsonConvert.DeserializeObject<List<Client2>>(List[0].json);
                                try
                                {
                                    Contacts_List.RemoveAt(Contacts_List.IndexOf(Contacts_List.SingleOrDefault(i => i._id == obj._id)));
                                    Contacts_List.Add(Result.Item3);
                                    var json = JsonConvert.SerializeObject(Contacts_List);
                                    DataStore.UpdateData("Contacts", json);
                                    DataStore.DeleteEditContactToUpdate(contact.Id);
                                    
                                }
                                catch
                                {
                                    DataStore.UpdateEditContactToUpload(contact.Id, "Erreur Lors de Supression");
                                    
                                }
                            }
                            else
                            {
                                DataStore.UpdateEditContactToUpload(contact.Id, Result.Item2);
                                
                            }
                        }
                        MessagingCenter.Send(this, "ContactsSyncUpdated");
                    }
                }
                if (Reports.Count > 0)
                {
                    foreach (var report in Reports)
                    {
                        var Result = await RESTService.Post_Answer(report.report, report.Global_id, report.Mission_id, report.Contact_id, report.IsFreeMission);
                        if (Result.Item1)
                        {
                            DataStore.DeleteReportToUload(report.Id);
                            
                            if (report.IsFreeMission)
                            {
                            }
                        }
                        else
                        {
                            DataStore.UpdateReportToUpload(report.Id, Result.Item2);
                            
                        }
                        MessagingCenter.Send(this, "ContactsSyncUpdated");
                    }
                }
                if (Unvailibilitys.Count > 0)
                {
                    foreach (var unvailibility in Unvailibilitys)
                    {
                        var Result = await RESTService.Post_Unvailability(unvailibility.Json, unvailibility.Global_id, unvailibility.Mission_id, unvailibility.Contact_id, unvailibility.IsFreeMission);
                        if (Result.Item1)
                        {
                            DataStore.DeleteUnvailibilityToUload(unvailibility.Id);
                            
                            if (unvailibility.IsFreeMission)
                            {
                            }
                        }
                        else
                        {
                            DataStore.UpdateUnvailibilityToUpload(unvailibility.Id, Result.Item2);
                            
                        }
                        MessagingCenter.Send(this, "ContactsSyncUpdated");
                    }
                }
                if(Questionnaire.Count > 0)
                {
                    foreach(var questionnaire in Questionnaire)
                    {
                        var Result = await RESTService.PostQuestionnaireAsync(questionnaire.Json);
                        if(Result.Item1)
                        {
                            DataStore.DeleteQuestionnaireToUpload(questionnaire.Id);
                            
                        }
                        else
                        {
                            DataStore.UpdateQuestionnairesToUpload(questionnaire.Id, Result.Item2);
                        }
                        MessagingCenter.Send(this, "ContactsSyncUpdated");
                    }
                }
                var Result2 = await RESTService.GetMissionsAsync();
                if (Result2.Item1)
                {
                    var List = DataStore.GetDataStoredJson("Missions").ToList();
                    if (List.Count > 0)
                    {
                        DataStore.UpdateData("Missions", Result2.Item2);
                    }
                    else
                    {
                        var data = new Stored_Data_Model
                        {
                            DateTime = DateTime.Now,
                            json = Result2.Item2,
                            Type = "Missions"
                        };
                        DataStore.AddData(data);
                    }
                }
                if (IsFreeMission)
                {
                    var Result = await RESTService.GetPeriodHistryAsync(DateTime.Now, DateTime.Now);
                    if (Result.Item1)
                    {
                        var List = DataStore.GetDataStoredJson("VisiteToday").ToList();
                        if (List.Count > 0)
                        {
                            DataStore.UpdateData("VisiteToday", Result.Item2);
                        }
                        else
                        {
                            var data = new Stored_Data_Model
                            {
                                DateTime = DateTime.Now,
                                json = Result.Item2,
                                Type = "VisiteToday"
                            };
                            DataStore.AddData(data);
                        }
                    }
                }
            }
        }
    }
}