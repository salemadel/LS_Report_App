using System;
using System.Collections.Generic;

namespace LS_Report.Models
{
    public class PL_Model
    {
        public string title { get; set; }
        public string type { get; set; }
        public string wilaya { get; set; }
        public string city { get; set; }
        public string description { get; set; }
        public DateTime mission_deadline { get; set; }

        public List<pl_client> clients { get; set; }

        public PL_Model()
        {
            clients = new List<pl_client>();
        }
    }

    public class pl_client
    {
        public string client { get; set; }
    }

    public class Global_PL_Model
    {
        public string title { get; set; }
        public DateTime global_mission_deadline { get; set; }

        public DateTime global_mission_start { get; set; }

        public List<PL_Model> missions { get; set; }

        public Global_PL_Model()
        {
            missions = new List<PL_Model>();
        }
    }
}