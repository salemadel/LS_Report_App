using LS_Report.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Plugin.Media.Abstractions;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LS_Report.Data
{
    public class RESTService
    {
        private readonly string ipAdress = "149.202.133.17";

        public async Task<(bool, string)> Login(string username, string password)
        {
            string error_msg = null;
            bool Succes = false;
            if (await Connectivity_check())
            {
                var keyvalues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("username" , username),
                new KeyValuePair<string, string>("password" , password),
                new KeyValuePair<string, string>("active" , "true")
            };
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, "http://" + ipAdress + "/api/agents/login")
                    {
                        Content = new FormUrlEncodedContent(keyvalues)
                    };
                    var client = new HttpClient();
                    client.Timeout = TimeSpan.FromSeconds(20);
                    var responce = await client.SendAsync(request);
                    var json = await responce.Content.ReadAsStringAsync();

                    dynamic obj = JsonConvert.DeserializeObject(json);
                    if (!json.Contains("name") & !json.Contains("password"))
                    {
                        Succes = true;
                        string token = obj["token"];
                        token = token.Substring(7, token.Length - 7);
                        CrossSecureStorage.Current.SetValue("UserName", username);
                        CrossSecureStorage.Current.SetValue("PassWord", password);
                        CrossSecureStorage.Current.SetValue("LogguedIn", "True");
                        CrossSecureStorage.Current.SetValue("acces_token", token);
                    }
                    else
                    {
                        string error_name = obj["name"];
                        string error_password = obj["password"];
                        error_msg = error_name ?? error_password;
                    }
                }
                catch (Exception ex)
                {
                    error_msg = ex.Message;
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }

            return (Succes, error_msg);
        }

        public async Task<(bool, string)> GetClientsAsync()
        {
            string error_msg = null;
            bool Succes = false;
            if (await Connectivity_check())
            {
                try
                {
                    var _token = CrossSecureStorage.Current.HasKey("acces_token");
                    if (_token)
                    {
                        string token = CrossSecureStorage.Current.GetValue("acces_token");
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        client.Timeout = TimeSpan.FromSeconds(120);
                        var json = await client.GetStringAsync("http://" + ipAdress + "/api/clients/all");
                        if (!json.Contains("msg") & !json.Contains("error"))
                        {
                            Succes = true;
                            error_msg = json;
                        }
                        else
                        {
                            dynamic obj = JsonConvert.DeserializeObject(json);
                            string msg = null;
                            string error = null;
                            try
                            {
                                msg = obj["msg"];
                                error = obj["error"];
                            }
                            catch
                            {
                            }
                            error_msg = msg ?? error;
                        }
                    }
                }
                catch (Exception ex)
                {
                    error_msg = "Un Probléme est survenu Lors du téléchargemt de la liste des contact ";
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, error_msg);
        }

        public async Task<(bool, string)> GetProductsAsync()
        {
            string error_msg = null;
            bool succes = false;
            //  var _tokencontroller = new Tokencontroller();
            var _token = CrossSecureStorage.Current.HasKey("acces_token");
            if (await Connectivity_check())
            {
                try
                {
                    if (_token)
                    {
                        string token = CrossSecureStorage.Current.GetValue("acces_token");
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        client.Timeout = TimeSpan.FromSeconds(30);
                        var json = await client.GetStringAsync("http://" + ipAdress + "/api/products");
                        if (!json.Contains("msg") & !json.Contains("error"))
                        {
                            succes = true;
                            error_msg = json;
                        }
                        else
                        {
                            dynamic obj = JsonConvert.DeserializeObject(json);
                            string msg = null;
                            string error = null;
                            try
                            {
                                msg = obj["msg"];
                                error = obj["error"];
                            }
                            catch
                            {
                            }
                            error_msg = msg ?? error;
                        }
                    }
                }
                catch (Exception ex)
                {
                    error_msg = "Un Probléme est survenu Lors du téléchargement de la liste des produits";
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }
            return (succes, error_msg);
        }

        public async Task<(bool, string)> Get_Contacts_Async()
        {
            string error_msg = null;
            bool Succes = false;
            //  var _tokencontroller = new Tokencontroller();
            var _token = CrossSecureStorage.Current.HasKey("acces_token");
            if (await Connectivity_check())
            {
                try
                {
                    if (_token)
                    {
                        string token = CrossSecureStorage.Current.GetValue("acces_token");

                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        var json = await client.GetStringAsync("http://" + ipAdress + "/api/mails/contacts");
                        if (!json.Contains("msg") & !json.Contains("error"))
                        {
                            Succes = true;
                            error_msg = json;
                        }
                        else
                        {
                            dynamic obj = JsonConvert.DeserializeObject(json);
                            string msg = null;
                            string error = null;
                            try
                            {
                                msg = obj["msg"];
                                error = obj["error"];
                            }
                            catch
                            {
                            }
                            error_msg = msg ?? error;
                        }
                    }
                }
                catch (Exception ex)
                {
                    error_msg = "Un probléme est survenu";
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, error_msg);
        }

        public async Task<(bool, string, Client2)> Post_New_Client(NewContact_Model new_Client, MediaFile mediafile = null, Stream picture = null)
        {
            string msg = null;
            bool Succes = false;
            Client2 contact = null;
            if (await Connectivity_check())
            {
                try
                {
                    var client = new HttpClient();
                    var content = new MultipartFormDataContent();

                    if (mediafile != null)
                    {
                        StreamContent scontnent = new StreamContent(mediafile.GetStream());
                        scontnent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            FileName = "image.jpeg",
                            Name = "avatar"
                        };
                        scontnent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                        content.Add(scontnent);
                    }
                    if (picture != null)
                    {
                        StreamContent scontnent = new StreamContent(picture);
                        scontnent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            FileName = "image.jpeg",
                            Name = "avatar"
                        };
                        scontnent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                        content.Add(scontnent);
                    }
                    var values = new Dictionary<string, string>();
                    values.Add("firstname", new_Client.firstname);
                    values.Add("lastname", new_Client.lastname);
                    values.Add("speciality", new_Client.speciality);
                    values.Add("landline", new_Client.landline);
                    values.Add("local_appearance", new_Client.local_appearance);
                    values.Add("sex", new_Client.sex);
                    values.Add("potential", JsonConvert.SerializeObject(new_Client.potential));
                    values.Add("prescription", new_Client.prescription);
                    values.Add("sector", new_Client.sector);
                    values.Add("fax", new_Client.fax);
                    values.Add("age", new_Client.age);
                    values.Add("placement", new_Client.placement);
                    values.Add("rc", new_Client.rc);
                    values.Add("bank", new_Client.bank);
                    values.Add("address", new_Client.address);
                    values.Add("email", new_Client.email);
                    values.Add("business_type", new_Client.business_type);
                    values.Add("city", new_Client.city);
                    values.Add("company", new_Client.company);
                    values.Add("phone", new_Client.phone);
                    values.Add("wilaya", new_Client.wilaya);
                    values.Add("type", new_Client.location.type);
                    values.Add("lat", new_Client.location.coordinates.lat);
                    values.Add("lng", new_Client.location.coordinates.lng);
                    var suppliers = (new_Client.suppliers == null) ? null : JsonConvert.SerializeObject(new_Client.suppliers);

                    values.Add("suppliers", suppliers);

                    /* var stringcontent = new StringContent(JsonConvert.SerializeObject(values) , Encoding.UTF8, "application/json");
                     Debug.WriteLine(await stringcontent.ReadAsStringAsync());
                     content.Add(new StringContent(JsonConvert.SerializeObject(values) , Encoding.UTF8, "application/json"));*/

                    foreach (var value in values)
                    {
                        if (value.Value != null)
                        {
                            content.Add(new StringContent(value.Value), value.Key);
                        }
                    }

                    string token = CrossSecureStorage.Current.GetValue("acces_token");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.Timeout = TimeSpan.FromSeconds(30);
                    var responce = await client.PostAsync("http://" + ipAdress + "/api/clients/new", content);

                    var result = await responce.Content.ReadAsStringAsync();

                    if (result.Contains("_id"))
                    {
                        contact = JsonConvert.DeserializeObject<Client2>(result);
                        Succes = true;
                    }
                    else
                    {
                        var obj = JObject.Parse(result);
                        var msgs = obj.ToObject<Dictionary<string, string>>();
                        foreach (var entry in msgs)
                        {
                            msg = entry.Value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = "Un Probléme est survenu ";
                }
            }
            else
            {
                msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, msg, contact);
        }

        public async Task<(bool, string, Client2)> Update_client(NewContact_Model new_Client, MediaFile mediafile = null, Stream picture = null)
        {
            string msg = null;
            bool Succes = false;
            Client2 contact = null;
            if (await Connectivity_check())
            {
                try
                {
                    var client = new HttpClient();
                    var content = new MultipartFormDataContent();
                    if (mediafile != null)
                    {
                        StreamContent scontnent = new StreamContent(mediafile.GetStream());
                        scontnent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            FileName = "image.jpeg",
                            Name = "avatar"
                        };
                        scontnent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                        content.Add(scontnent);
                    }
                    if (picture != null)
                    {
                        StreamContent scontnent = new StreamContent(picture);
                        scontnent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            FileName = "image.jpeg",
                            Name = "avatar"
                        };
                        scontnent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                        content.Add(scontnent);
                    }
                    var values = new Dictionary<string, string>();

                    values.Add("_id", new_Client._id);
                    values.Add("firstname", new_Client.firstname);
                    values.Add("lastname", new_Client.lastname);
                    values.Add("speciality", new_Client.speciality);
                    values.Add("landline", new_Client.landline);
                    values.Add("local_appearance", new_Client.local_appearance);
                    values.Add("sex", new_Client.sex);
                    values.Add("potential", JsonConvert.SerializeObject(new_Client.potential));
                    values.Add("prescription", new_Client.prescription);
                    values.Add("sector", new_Client.sector);
                    values.Add("fax", new_Client.fax);
                    values.Add("age", new_Client.age);
                    values.Add("placement", new_Client.placement);
                    values.Add("rc", new_Client.rc);
                    values.Add("bank", new_Client.bank);
                    values.Add("address", new_Client.address);
                    values.Add("email", new_Client.email);
                    values.Add("business_type", new_Client.business_type);
                    values.Add("city", new_Client.city);
                    values.Add("company", new_Client.company);
                    values.Add("phone", new_Client.phone);
                    values.Add("wilaya", new_Client.wilaya);
                    values.Add("type", new_Client.location.type);
                    values.Add("lat", new_Client.location.coordinates.lat);
                    values.Add("lng", new_Client.location.coordinates.lng);
                    var suppliers = (new_Client.suppliers == null) ? null : JsonConvert.SerializeObject(new_Client.suppliers);

                    values.Add("suppliers", suppliers);
                    foreach (var keyValuePair in values)
                    {
                        if (keyValuePair.Value != null)
                        {
                            content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                        }
                    }

                    string token = CrossSecureStorage.Current.GetValue("acces_token");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.Timeout = TimeSpan.FromSeconds(30);
                    var responce = await client.PostAsync("http://" + ipAdress + "/api/clients/update", content);

                    var result = await responce.Content.ReadAsStringAsync();

                    if (result.Contains("_id"))
                    {
                        contact = JsonConvert.DeserializeObject<Client2>(result);
                        Succes = true;
                    }
                    else
                    {
                        var obj = JObject.Parse(result);
                        var msgs = obj.ToObject<Dictionary<string, string>>();
                        foreach (var entry in msgs)
                        {
                            msg = entry.Value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = "Un probléme est survenu ";
                    //  Shared_data.write_log(ex.ToString());
                }
            }
            else
            {
                msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, msg, contact);
        }

        public async Task<(bool, string)> Post_Answer(List<KeyValuePair<string, string>> json, string id_global_mission, string id_mission, string id_client, bool IsFreeMission, MediaFile mediafile = null, Stream picture = null)
        {
            string msg = null;
            bool Succes = false;
            if (await Connectivity_check())
            {
                try
                {
                    var client = new HttpClient();
                    var content = new MultipartFormDataContent();
                    if (mediafile != null)
                    {
                        StreamContent scontnent = new StreamContent(mediafile.GetStream());
                        scontnent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            FileName = "image.jpeg",
                            Name = "proof"
                        };
                        scontnent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                        content.Add(scontnent);
                    }
                    if (picture != null)
                    {
                        StreamContent scontnent = new StreamContent(picture);
                        scontnent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            FileName = "image.jpeg",
                            Name = "proof"
                        };
                        scontnent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                        content.Add(scontnent);
                    }

                    if (json.Count > 0)
                    {
                        foreach (var kvp in json)
                        {
                            if (kvp.Value != null)
                            {
                                content.Add(new StringContent(kvp.Value), kvp.Key);
                            }
                        }

                        string token = CrossSecureStorage.Current.GetValue("acces_token");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage responce = new HttpResponseMessage();
                        if (IsFreeMission)
                        {
                            responce = await client.PostAsync("http://" + ipAdress + "/api/missions/free", content);
                        }
                        else
                        {
                            responce = await client.PostAsync("http://" + ipAdress + "/api/missions/" + id_global_mission + "/" + id_mission + "/" + id_client, content);
                        }

                        var result = await responce.Content.ReadAsStringAsync();

                        if (!result.Contains("error"))
                        {
                            Succes = true;
                        }
                        else
                        {
                            dynamic obj = JsonConvert.DeserializeObject(result);
                            try
                            {
                                msg = obj["error"];
                            }
                            catch
                            {
                            }
                        }
                    }
                    else
                    {
                        msg = "Un probléme est survenu";
                    }
                }
                catch (Exception ex)
                {
                    msg = "Un probléme est survenu";
                }
            }
            else
            {
                msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, msg);
        }

        public async Task<(bool, string)> GetMissionsAsync()
        {
            string error_msg = null;
            bool Succes = false;

            if (await Connectivity_check())
            {
                try
                {
                    var _token = CrossSecureStorage.Current.HasKey("acces_token");
                    if (_token)
                    {
                        string token = CrossSecureStorage.Current.GetValue("acces_token");

                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        client.Timeout = TimeSpan.FromSeconds(30);
                        var json = await client.GetStringAsync("http://" + ipAdress + "/api/missions/today");
                        if (!json.Contains("msg") & !json.Contains("error"))
                        {
                            Succes = true;
                            error_msg = json;
                        }
                        else
                        {
                            dynamic obj = JsonConvert.DeserializeObject(json);
                            string msg = null;
                            string error = null;
                            try
                            {
                                msg = obj["msg"];
                                error = obj["error"];
                            }
                            catch
                            {
                            }
                        }
                    }
                    else
                    {
                        error_msg = null;
                    }
                }
                catch (Exception ex)
                {
                    error_msg = "Un Probléme est survenu ";
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, error_msg);
        }

        public async Task<(bool, string)> GetPeriodHistryAsync(DateTime startdate, DateTime enddate)
        {
            string error_msg = null;
            bool Succes = false;
            if (await Connectivity_check())
            {
                bool _token = CrossSecureStorage.Current.HasKey("acces_token");
                if (_token)
                {
                    try
                    {
                        string token = CrossSecureStorage.Current.GetValue("acces_token");
                        var keyvalues = new List<KeyValuePair<string, string>>{
                          new KeyValuePair<string, string>("startDate" , startdate.Date.ToString("yyyy-MM-dd")),
                          new KeyValuePair<string, string>("endDate" , enddate.Date.ToString("yyyy-MM-dd"))
                        };
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        client.Timeout = TimeSpan.FromSeconds(30);
                        var request = new HttpRequestMessage(HttpMethod.Post, "http://" + ipAdress + "/api/repports/history");
                        request.Content = new FormUrlEncodedContent(keyvalues);

                        var responce = await client.SendAsync(request);
                        var content = await responce.Content.ReadAsStringAsync();
                        if (!content.Contains("msg") & !content.Contains("error"))
                        {
                            error_msg = content;
                            Succes = true;
                        }
                        else
                        {
                            dynamic obj = JsonConvert.DeserializeObject(content);
                            string msg = null;
                            string error = null;
                            try
                            {
                                msg = obj["msg"];
                                error = obj["error"];
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        error_msg = "Un probléme est survenu";
                        //  Shared_data.write_log(ex.ToString());
                    }
                }
                else
                {
                    error_msg = null;
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, error_msg);
        }

        public async Task<(bool, string)> GetClientsHistoryAsync(string client_id)
        {
            string error_msg = null;
            bool Succes = false;

            if (await Connectivity_check())
            {
                try
                {
                    bool _token = CrossSecureStorage.Current.HasKey("acces_token");
                    if (_token)
                    {
                        string token = CrossSecureStorage.Current.GetValue("acces_token");

                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        client.Timeout = TimeSpan.FromSeconds(30);
                        var json = await client.GetStringAsync("http://" + ipAdress + "/api/clients/history/" + client_id);

                        if (!json.Contains("msg") & !json.Contains("error"))
                        {
                            error_msg = json;
                            Succes = true;
                        }
                        else
                        {
                            dynamic obj = JsonConvert.DeserializeObject(json);
                            string msg = null;
                            string error = null;
                            try
                            {
                                msg = obj["msg"];
                                error = obj["error"];
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    error_msg = "Un probléme est survenu";
                    //  Shared_data.write_log(ex.ToString());
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, error_msg);
        }

        public async Task<(bool, string)> Post_Unvailability(string json, string id_global_mission, string id_mission, string id_client, bool isfreemission, MediaFile mediafile = null)
        {
            string error_msg = null;
            bool Succes = false;
            if (await Connectivity_check())
            {
                try
                {
                    var client = new HttpClient();
                    var content = new MultipartFormDataContent();
                    if (mediafile != null)
                    {
                        StreamContent scontnent = new StreamContent(mediafile.GetStream());
                        scontnent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            FileName = "image.jpeg",
                            Name = "proof"
                        };
                        scontnent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                        content.Add(scontnent);
                    }

                    var values = new List<KeyValuePair<string, string>>();
                    if (isfreemission)
                    {
                        values.Add(new KeyValuePair<string, string>("client", json));
                    }
                    else
                    {
                        values.Add(new KeyValuePair<string, string>("mission", json));
                    }

                    values.Add(new KeyValuePair<string, string>("repport", "{}"));
                    values.Add(new KeyValuePair<string, string>("order", "{}"));

                    if (values.Count > 0)
                    {
                        foreach (var kvp in values)
                        {
                            content.Add(new StringContent(kvp.Value), kvp.Key);
                        }

                        string token = CrossSecureStorage.Current.GetValue("acces_token");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage responce = new HttpResponseMessage();
                        client.Timeout = TimeSpan.FromSeconds(30);
                        if (isfreemission)
                        {
                            responce = await client.PostAsync("http://" + ipAdress + "/api/missions/free", content);
                        }
                        else
                        {
                            responce = await client.PostAsync("http://" + ipAdress + "/api/missions/" + id_global_mission + "/" + id_mission + "/" + id_client, content);
                        }

                        var result = await responce.Content.ReadAsStringAsync();

                        if (!result.Contains("error"))
                        {
                            Succes = true;
                        }
                        else
                        {
                            dynamic obj = JsonConvert.DeserializeObject(json);
                            string msg = null;
                            string error = null;
                            try
                            {
                                msg = obj["msg"];
                                error = obj["error"];
                                error_msg = msg ??= error;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    error_msg = "Un probléme est survenu";
                    // Shared_data.write_log(ex.ToString());
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, error_msg);
        }

        public async Task<(bool, string)> Disconnect_async()
        {
            string error_msg = null;
            bool Succes = false;
            if (await Connectivity_check())
            {
                try
                {
                    string token = CrossSecureStorage.Current.GetValue("acces_token");
                    var keyvalues = new List<KeyValuePair<string, string>>{
                          new KeyValuePair<string, string>("active" , "false")
                        };
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.Timeout = TimeSpan.FromSeconds(30);
                    var request = new HttpRequestMessage(HttpMethod.Post, "http://" + ipAdress + "/api/agents/logout");
                    request.Content = new FormUrlEncodedContent(keyvalues);

                    client.Timeout = TimeSpan.FromSeconds(30);
                    var responce = await client.SendAsync(request);
                    var content = await responce.Content.ReadAsStringAsync();
                    if (!content.Contains("error"))
                    {
                        CrossSecureStorage.Current.DeleteKey("acces_token");
                        CrossSecureStorage.Current.SetValue("LogguedIn", "False");
                        Succes = true;
                    }
                    else
                    {
                        dynamic obj = JsonConvert.DeserializeObject(content);
                        string msg = null;
                        string error = null;
                        try
                        {
                            msg = obj["msg"];
                            error = obj["error"];
                            error_msg = msg ??= error;
                        }
                        catch
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    error_msg = "Un probléme est survenu";
                    //  Shared_data.write_log(ex.ToString());
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, error_msg);
        }

        public async Task<(bool, string)> Change_password_async(string new_password)
        {
            string error_msg = null;
            bool Succes = false;
            if (await Connectivity_check())
            {
                try
                {
                    string token = CrossSecureStorage.Current.GetValue("acces_token");
                    var keyvalues = new List<KeyValuePair<string, string>>{
                          new KeyValuePair<string, string>("password" , new_password)
                        };
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.Timeout = TimeSpan.FromSeconds(30);
                    var request = new HttpRequestMessage(HttpMethod.Post, "http://" + ipAdress + "/api/agents/resetpassword");
                    request.Content = new FormUrlEncodedContent(keyvalues);

                    client.Timeout = TimeSpan.FromSeconds(30);
                    var responce = await client.SendAsync(request);
                    var content = await responce.Content.ReadAsStringAsync();
                    if (!content.Contains("error"))
                    {
                        Succes = true;
                    }
                    else
                    {
                        dynamic obj = JsonConvert.DeserializeObject(content);
                        string msg = null;
                        string error = null;
                        try
                        {
                            msg = obj["msg"];
                            error = obj["error"];
                            error_msg = msg ??= error;
                        }
                        catch
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    error_msg = "Un probléme est survenu";
                    //  Shared_data.write_log(ex.ToString());
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, error_msg);
        }

        public async Task<(bool, string)> Post_mission_completed(string mission_id, string global_mission, bool global_mission_completed, bool mission_completed)
        {
            string error_msg = null;
            bool Succes = false;
            if (await Connectivity_check())
            {
                try
                {
                    string token = CrossSecureStorage.Current.GetValue("acces_token");
                    var keyvalues = new List<KeyValuePair<string, string>>{
                          new KeyValuePair<string, string>("mission_id" , mission_id),
                           new KeyValuePair<string, string>("mission_completed" , mission_completed.ToString().ToLower()),
                          new KeyValuePair<string, string>("global_mission" , global_mission),
                           new KeyValuePair<string, string>("global_mission_completed" , global_mission_completed.ToString().ToLower())
                        };
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var request = new HttpRequestMessage(HttpMethod.Post, "http://" + ipAdress + "/api/missions/completed");
                    request.Content = new FormUrlEncodedContent(keyvalues);
                    client.Timeout = TimeSpan.FromSeconds(20);

                    var responce = await client.SendAsync(request);
                    var content = await responce.Content.ReadAsStringAsync();
                    if (!content.Contains("error"))
                    {
                        Succes = true;
                    }
                    else
                    {
                        dynamic obj = JsonConvert.DeserializeObject(content);
                        string msg = null;
                        string error = null;
                        try
                        {
                            msg = obj["msg"];
                            error = obj["error"];
                            error_msg = msg ??= error;
                        }
                        catch
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    error_msg = "Un probléme est survenu";
                    //  Shared_data.write_log(ex.ToString());
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, error_msg);
        }

        public async Task<(bool, string)> Post_absence(string json, bool isfreemission, string id_global_mission, string id_mission, string id_client)
        {
            string error_msg = null;
            bool Succes = false;
            if (await Connectivity_check())
            {
                try
                {
                    var client = new HttpClient();

                    var values = new List<KeyValuePair<string, string>>();
                    if (isfreemission)
                    {
                        values.Add(new KeyValuePair<string, string>("client", json));
                    }
                    else
                    {
                        values.Add(new KeyValuePair<string, string>("mission", json));
                    }

                    values.Add(new KeyValuePair<string, string>("repport", "{}"));
                    values.Add(new KeyValuePair<string, string>("order", "{}"));
                    if (values.Count > 0)
                    {
                        string token = CrossSecureStorage.Current.GetValue("acces_token");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                        HttpResponseMessage responce = new HttpResponseMessage();
                        client.Timeout = TimeSpan.FromSeconds(30);
                        if (isfreemission)
                        {
                            var request = new HttpRequestMessage(HttpMethod.Post, "http://" + ipAdress + "/api/missions/free");
                            request.Content = new FormUrlEncodedContent(values);
                            responce = await client.SendAsync(request);
                        }
                        else
                        {
                            var request = new HttpRequestMessage(HttpMethod.Post, "http://" + ipAdress + "/api/missions/" + id_global_mission + "/" + id_mission + "/" + id_client);
                            request.Content = new FormUrlEncodedContent(values);
                            responce = await client.SendAsync(request);
                        }

                        var result = await responce.Content.ReadAsStringAsync();

                        if (!result.Contains("error"))
                        {
                            Succes = true;
                        }
                        else
                        {
                            dynamic obj = JsonConvert.DeserializeObject(result);
                            string msg = null;
                            string error = null;
                            try
                            {
                                msg = obj["msg"];
                                error = obj["error"];
                                error_msg = msg ??= error;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    error_msg = "Un probléme est survenu";
                    // Shared_data.write_log(ex.ToString());
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, error_msg);
        }

        public async Task<(bool, string)> Get_mails_Sent_Async()
        {
            string error_msg = null;

            bool Succes = false;
            if (await Connectivity_check())
            {
                try
                {
                    var _token = CrossSecureStorage.Current.HasKey("acces_token");
                    if (_token)
                    {
                        string token = CrossSecureStorage.Current.GetValue("acces_token");

                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        client.Timeout = TimeSpan.FromSeconds(20);
                        var json = await client.GetStringAsync("http://" + ipAdress + "/api/mails/sent");

                        if (!json.Contains("error"))
                        {
                            Succes = true;
                            error_msg = json;
                        }
                        else
                        {
                            dynamic obj = JsonConvert.DeserializeObject(json);
                            string msg = null;
                            string error = null;
                            try
                            {
                                msg = obj["msg"];
                                error = obj["error"];
                                error_msg = msg ??= error;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    error_msg = "Un probléme est survenu";
                    //  Shared_data.write_log(ex.ToString());
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, error_msg);
        }

        public async Task<(bool, string)> Get_mails_Received_Async()
        {
            string error_msg = null;
            bool Succes = false;
            if (await Connectivity_check())
            {
                try
                {
                    var _token = CrossSecureStorage.Current.HasKey("acces_token");
                    if (_token)
                    {
                        string token = CrossSecureStorage.Current.GetValue("acces_token");

                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        client.Timeout = TimeSpan.FromSeconds(20);
                        var json = await client.GetStringAsync("http://" + ipAdress + "/api/mails/received");

                        if (!json.Contains("error"))
                        {
                            Succes = true;
                            error_msg = json;
                        }
                        else
                        {
                            dynamic obj = JsonConvert.DeserializeObject(json);
                            string msg = null;
                            string error = null;
                            try
                            {
                                msg = obj["msg"];
                                error = obj["error"];
                                error_msg = msg ??= error;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    error_msg = "Un probléme est survenu";
                    //  Shared_data.write_log(ex.ToString());
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, error_msg);
        }

        public async Task<(bool, string)> Send_Mail_Async(string id_receiver, string priority, string title, string message, string fullname)
        {
            string error_msg = null;
            bool Succes = false;
            if (await Connectivity_check())
            {
                try
                {
                    string token = CrossSecureStorage.Current.GetValue("acces_token");
                    var keyvalues = new List<KeyValuePair<string, string>>{
                          new KeyValuePair<string, string>("receiver" , id_receiver),
                          new KeyValuePair<string, string>("priority" , priority),
                          new KeyValuePair<string, string>("title" , title),
                           new KeyValuePair<string, string>("fullname" , fullname),
                          new KeyValuePair<string, string>("message" , message)
                        };
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var request = new HttpRequestMessage(HttpMethod.Post, "http://" + ipAdress + "/api/mails/new");
                    request.Content = new FormUrlEncodedContent(keyvalues);
                    client.Timeout = TimeSpan.FromSeconds(20);

                    var responce = await client.SendAsync(request);
                    var content = await responce.Content.ReadAsStringAsync();
                    if (!content.Contains("error"))
                    {
                        Succes = true;
                    }
                    else
                    {
                        dynamic obj = JsonConvert.DeserializeObject(content);
                        string msg = null;
                        string error = null;
                        try
                        {
                            msg = obj["msg"];
                            error = obj["error"];
                            error_msg = msg ??= error;
                        }
                        catch
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    error_msg = "Un probléme est survenu";
                    //  Shared_data.write_log(ex.ToString());
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, error_msg);
        }

        public async Task<(bool, string)> GetQuestionnairesAsyc()
        {
            string error_msg = null;
            bool Succes = false;
            if (await Connectivity_check())
            {
                try
                {
                    var _token = CrossSecureStorage.Current.HasKey("acces_token");
                    if (_token)
                    {
                        string token = CrossSecureStorage.Current.GetValue("acces_token");
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        client.Timeout = TimeSpan.FromSeconds(120);
                        var json = await client.GetStringAsync("http://" + ipAdress + "/api/questionnaires/questions");
                        if (!json.Contains("msg") & !json.Contains("error"))
                        {
                            Succes = true;
                            error_msg = json;
                        }
                        else
                        {
                            dynamic obj = JsonConvert.DeserializeObject(json);
                            string msg = null;
                            string error = null;
                            try
                            {
                                msg = obj["msg"];
                                error = obj["error"];
                            }
                            catch
                            {
                            }
                            error_msg = msg ?? error;
                        }
                    }
                }
                catch (Exception ex)
                {
                    error_msg = "Un Probléme est survenu Lors du téléchargemt de la liste des contact ";
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, error_msg);
        }

        public async Task<(bool, string)> PostQuestionnaireAsync(string Json)
        {
            string error_msg = null;
            bool Succes = false;
            if (await Connectivity_check())
            {
                try
                {
                    string token = CrossSecureStorage.Current.GetValue("acces_token");
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                    KeyValuePair<string, string> keyValue = new KeyValuePair<string, string>("data", Json);
                    keyValuePairs.Add(keyValue);
                    var request = new HttpRequestMessage(HttpMethod.Post, "http://" + ipAdress + "/api/questionnaires/answer");
                    request.Content = new FormUrlEncodedContent(keyValuePairs);
                    client.Timeout = TimeSpan.FromSeconds(20);

                    var responce = await client.SendAsync(request);
                    var content = await responce.Content.ReadAsStringAsync();
                    if (!content.Contains("error"))
                    {
                        Succes = true;
                    }
                    else
                    {
                        dynamic obj = JsonConvert.DeserializeObject(content);
                        string msg = null;
                        string error = null;
                        try
                        {
                            msg = obj["msg"];
                            error = obj["error"];
                            error_msg = msg ??= error;
                        }
                        catch
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    error_msg = "Un probléme est survenu";
                    //  Shared_data.write_log(ex.ToString());
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, error_msg);
        }

        public async Task<(bool, string)> CheckVersion()
        {
            string error_msg = null;
            bool succes = false;

            if (await Connectivity_check())
            {
                try
                {
                    HttpClient client = new HttpClient();

                    client.Timeout = TimeSpan.FromSeconds(30);
                    var json = await client.GetStringAsync("http://" + ipAdress + "/api?version=2.0.2.0");
                    dynamic obj = JsonConvert.DeserializeObject(json);
                    if (obj.status == 200)
                    {
                        succes = true;
                    }
                    else
                    {
                        error_msg = "Veuillez mettre à jour l'application avant de se connecter";
                    }
                }
                catch (Exception ex)
                {
                    error_msg = "Un Probléme est survenu";
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }
            return (succes, error_msg);
        }

        public async Task<(bool, string)> GetStatsPeriode(DateTime startdate, DateTime enddate, string agent)
        {
            string error_msg = null;
            bool Succes = false;
            if (await Connectivity_check())
            {
                bool _token = CrossSecureStorage.Current.HasKey("acces_token");
                if (_token)
                {
                    try
                    {
                        string token = CrossSecureStorage.Current.GetValue("acces_token");
                        var keyvalues = new List<KeyValuePair<string, string>>{
                            new KeyValuePair<string, string>("agent" ,agent),
                          new KeyValuePair<string, string>("startDate" , startdate.Date.ToString("yyyy-MM-dd")),
                          new KeyValuePair<string, string>("endDate" , enddate.Date.ToString("yyyy-MM-dd"))
                        };
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        client.Timeout = TimeSpan.FromSeconds(30);
                        var request = new HttpRequestMessage(HttpMethod.Post, "http://" + ipAdress + "/api/stats/agent");
                        request.Content = new FormUrlEncodedContent(keyvalues);

                        var responce = await client.SendAsync(request);
                        var content = await responce.Content.ReadAsStringAsync();
                        if (!content.Contains("msg") & !content.Contains("error"))
                        {
                            error_msg = content;
                            Succes = true;
                        }
                        else
                        {
                            dynamic obj = JsonConvert.DeserializeObject(content);
                            string msg = null;
                            string error = null;
                            try
                            {
                                msg = obj["msg"];
                                error = obj["error"];
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        error_msg = "Un probléme est survenu";
                        //  Shared_data.write_log(ex.ToString());
                    }
                }
                else
                {
                    error_msg = null;
                }
            }
            else
            {
                error_msg = "Veuillez verifier votre connexion internet";
            }
            return (Succes, error_msg);
        }

        private async Task<bool> Connectivity_check()
        {
            var connectivity = CrossConnectivity.Current;
            if (!connectivity.IsConnected)
                return false;

            var reachable = await connectivity.IsRemoteReachable(ipAdress);

            return reachable;
        }
    }
}