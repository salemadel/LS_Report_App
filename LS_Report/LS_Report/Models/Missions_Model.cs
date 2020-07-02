using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LS_Report.Models
{
    public class Missions_Model
    {
        public bool global_mission_completed { get; set; }
        public string _id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string manager { get; set; }
        public string agent { get; set; }
        public DateTime global_mission_deadline { get; set; }
        public DateTime global_mission_start { get; set; }
        public List<Missions_VM> missions { get; set; }

        public string Date
        {
            get
            {
                return global_mission_start.ToString("dd MMMM") + " - " + global_mission_deadline.ToString("dd MMMM");
            }
        }

        public Missions_Model()
        {
            missions = new List<Missions_VM>();
        }
    }

    public class Missions_VM
    {
        public bool mission_completed { get; set; }
        public string _id { get; set; }
        public string title { get; set; }
        public string mission_wilaya { get; set; }
        public string description { get; set; }
        public DateTime mission_deadline { get; set; }
        public List<Client> clients { get; set; }
        public DateTime created_at { get; set; }
        public int __v { get; set; }

        public Missions_VM()
        {
            // questionnaires = new List<Questionnaire>();
            clients = new List<Client>();
        }
    }

    public class Client
    {
        public bool visited { get; set; }
        public Client2 client { get; set; }

        public Client()
        {
            client = new Client2();
        }
    }

    public class Client2 : IEquatable<Client2>
    {
        public Location location { get; set; }
        public string _id { get; set; }
        public string firstname { get; set; } = "";
        public string company { get; set; } = "";
        public string lastname { get; set; } = "";
        public string email { get; set; } = "";
        public string business_type { get; set; } = "";
        public string sector { get; set; } = "";
        public string phone { get; set; } = "";
        public string sex { get; set; } = "";
        public string age { get; set; } = "";
        public string speciality { get; set; } = "";
        public string landline { get; set; } = "";
        public string fax { get; set; } = "";
        public string rc { get; set; } = "";
        public List<Potential> potential { get; set; }
        public string prescription { get; set; } = "";
        public List<string> suppliers { get; set; }
        public string placement { get; set; } = "";
        public string local_appearance { get; set; } = "";
        public string avatar { get; set; }

        public ImageSource Avatar
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(avatar))
                {
                    return new UriImageSource
                    {
                        Uri = new System.Uri("http://149.202.133.17/" + avatar),
                        CachingEnabled = true,
                        CacheValidity = TimeSpan.FromDays(30)
                    };
                }
                else
                {
                    return ImageSource.FromFile("info1.jpg");
                }
            }
        }

        public string Name
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(company))
                {
                    return company + " " + lastname + " " + firstname;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(lastname) || !string.IsNullOrWhiteSpace(firstname))
                    {
                        return lastname + " " + firstname;
                    }
                    else
                    {
                        return "Sans nom";
                    }
                }
            }
        }

        public string NameSort
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Name) || Name.Length == 0)
                    return "?";

                return Name[0].ToString().ToUpper();
            }
        }

        public string Speciality
        {
            get
            {
                if (business_type == "Médecin")
                {
                    return speciality;
                }
                else
                {
                    return business_type;
                }
            }
        }

        public string Address
        {
            get
            {
                return wilaya + " " + city;
            }
        }

        public string address { get; set; }
        public string city { get; set; }
        public string wilaya { get; set; }
        public DateTime last_visit { get; set; }

        public int __v { get; set; }

        public Client2()
        {
            location = new Location();
            suppliers = new List<string>();
            potential = new List<Potential>();
        }

        public bool Equals(Client2 other)
        {
            if (other is null)
                return false;

            return this._id == other._id;
        }

        public override bool Equals(object obj) => Equals(obj as Client2);

        public override int GetHashCode() => (_id).GetHashCode();
    }

    public class Questionnaire
    {
    }

    public class Potential
    {
        public string network { get; set; }
        public string value { get; set; }
    }

    public class Location
    {
        public double?[] coordinates { get; set; }

        public string type { get; set; }
    }
}