using System;

namespace LS_Report.Models
{
    public class Mail_Model
    {
        public dynamic sender { get; set; }
        public dynamic receiver { get; set; }

        public string Sender
        {
            get
            {
                return sender.lastname + " " + sender.firstname;
            }
        }

        public string Receiver
        {
            get
            {
                return receiver.lastname + " " + receiver.firstname;
            }
        }

        public bool priority { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public DateTime created_at { get; set; }

        public string Date
        {
            get
            {
                if (created_at.Date == DateTime.Now.Date)
                {
                    return created_at.ToString("HH:mm");
                }
                else
                {
                    return created_at.ToString("yyyy/MM/dd");
                }
            }
        }

        public string Priority
        {
            get
            {
                if (priority == true)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
        }
    }

    public class Contacts_List
    {
        public string _id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }

        public string Name
        {
            get
            {
                return lastname + " " + firstname;
            }
        }

        public string email { get; set; }
        public string role { get; set; }
        public string network { get; set; }
        public int __v { get; set; }
    }
}