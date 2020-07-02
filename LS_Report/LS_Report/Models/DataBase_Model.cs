using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;

namespace LS_Report.Models
{
    public class DataBase_Model
    {
    }

    public class Stored_Data_Model
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime DateTime { get; set; }
        public string Type { get; set; }
        public string json { get; set; }
    }

    public class NewContactToUpload_Model
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public DateTime? Last_Sync { get; set; } = null;
        public string Name { get; set; }
        public string Json { get; set; }
        public string PicturePath { get; set; }
        public string Last_Error { get; set; } = "N/A";
    }

    public class EditContactToUpload_Model
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Contact_Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime? Last_Sync { get; set; } = null;
        public string Name { get; set; }
        public string Json { get; set; }
        public string PicturePath { get; set; }
        public string Last_Error { get; set; } = "N/A";
    }

    public class LocationsBackGround_Model
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [Ignore]
        public List<double> Coords
        {
            get
            {
                return JsonConvert.DeserializeObject<List<double>>(coords);
            }
            set
            {
                coords = JsonConvert.SerializeObject(value);
            }
        }

        public string coords { get; set; }
    }

    public class ReportToUpload_Model
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public DateTime? Last_Sync { get; set; } = null;
        public string Last_Error { get; set; } = "N/A";
        public string Client_name { get; set; }
        public string Global_id { get; set; }
        public string Mission_id { get; set; }
        public string Contact_id { get; set; }
        public bool IsFreeMission { get; set; }
        public string Picture_path { get; set; }

        [Ignore]
        public List<KeyValuePair<string, string>> report
        {
            get
            {
                return JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(Report);
            }
            set
            {
                Report = JsonConvert.SerializeObject(value);
            }
        }

        public string Report { get; set; }
    }

    public class UnvailibilityToUpload_Model
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public DateTime? Last_Sync { get; set; } = null;
        public string Last_Error { get; set; } = "N/A";
        public string Json { get; set; }
        public string Client_name { get; set; }
        public string Global_id { get; set; }
        public string Mission_id { get; set; }
        public string Contact_id { get; set; }
        public bool IsFreeMission { get; set; }
        public string Picture_path { get; set; }
    }

    public class Mails_Model
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public string Json { get; set; }
        public string Type { get; set; }
        public bool Read { get; set; }
    }

    public class QuastionnairesToUpload_Model
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime? Last_Sync { get; set; } = null;
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Json { get; set; }
        public string Last_Error { get; set; } = "N/A";
    }
}