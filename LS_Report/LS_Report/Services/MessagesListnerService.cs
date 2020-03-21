using LS_Report.Data;
using LS_Report.Interfaces;
using LS_Report.Models;
using Newtonsoft.Json;
using Plugin.LocalNotifications;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LS_Report.Services
{
    public class MessagesListnerService
    {
        private IDataStore DataStore { get; set; }
        private RESTService RESTService { get; set; }

        public MessagesListnerService(IDataStore dataStore)
        {
            DataStore = dataStore;
            RESTService = new RESTService();
        }

        public async Task Listner()
        {
            string LogguedIn = null;
            if (CrossSecureStorage.Current.HasKey("LogguedIn"))
            {
                LogguedIn = CrossSecureStorage.Current.GetValue("LogguedIn");
            }
            if (LogguedIn == "True")
            {
                var RecivedResult = await RESTService.Get_mails_Received_Async();
                if (RecivedResult.Item1)
                {
                    var MessagesList = DataStore.GetMails().Where(i => i.Type == "Received").ToList();
                    if (MessagesList.Count > 0)
                    {
                        var OldMessages = JsonConvert.DeserializeObject<List<Mail_Model>>(MessagesList[0].Json);
                        var NewMessages = JsonConvert.DeserializeObject<List<Mails_Model>>(RecivedResult.Item2);

                        if (NewMessages.Count > 0 && NewMessages.Count > OldMessages.Count)
                        {
                            DataStore.UpdateMails(MessagesList[0].Id, RecivedResult.Item2);
                            MessagingCenter.Send(this, "MessageReceived");
                            CrossLocalNotifications.Current.Show("Message Reçu", "Vous Avez Reçu " + (NewMessages.Count - OldMessages.Count).ToString() + "Message(s)");
                        }
                    }
                    else
                    {
                        var data = new Mails_Model
                        {
                            Date = DateTime.Now,
                            Json = RecivedResult.Item2,
                            Read = false,
                            Type = "Received"
                        };
                        DataStore.AddMails(data);
                        MessagingCenter.Send(this, "MessageReceived");
                    }
                }
            }
        }
    }
}