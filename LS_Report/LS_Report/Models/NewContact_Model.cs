using System.Collections.Generic;

namespace LS_Report.Models
{
    public class Coordinates
    {
        public string lat { get; set; }

        public string lng { get; set; }
    }

    public class Localistation
    {
        public Localistation()
        {
            coordinates = new Coordinates();
        }

        public Coordinates coordinates { get; set; }

        public string type
        {
            get
            {
                return "Point";
            }
        }
    }

    public class NewContact_Model
    {
        public NewContact_Model()
        {
            location = new Localistation();
            suppliers = new List<string>();
            potential = new List<Potential>();
        }

        public string _id { get; set; }
        public string address { get; set; }
        public string age { get; set; }
        public string bank { get; set; }
        public string business_type { get; set; }
        public string city { get; set; }
        public string company { get; set; }
        public string email { get; set; }
        public string fax { get; set; }
        public string firstname { get; set; }
        public string landline { get; set; }
        public string lastname { get; set; }
        public string local_appearance { get; set; }
        public Localistation location { get; set; }
        public string phone { get; set; }
        public string placement { get; set; }
        public List<Potential> potential { get; set; }
        public string prescription { get; set; }
        public string rc { get; set; }
        public string sector { get; set; }
        public string sex { get; set; }
        public string speciality { get; set; }
        public List<string> suppliers { get; set; }
        public string wilaya { get; set; }
    }
}