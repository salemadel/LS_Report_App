using System;
using System.Collections.Generic;

namespace LS_Report.Models
{
    public class Mission
    {
        public bool valid { get; set; }
        public bool visited { get; set; }
        public bool dual { get; set; }
        public DateTime? visit_time { get; set; } = null;
        public DateTime? visit_end_time { get; set; } = null;
        public List<string> coords { get; set; }

        public Mission()
        {
            coords = new List<string>();
        }
    }

    public class MissionAnswer
    {
        public string qestionnaire { get; set; }
        public List<Questions> data { get; set; }

        public MissionAnswer()
        {
            data = new List<Questions>();
        }
    }

    public class Questions
    {
        public string question { get; set; }
        public string answer { get; set; }
    }

    public class ProductAnswer
    {
        public string product { get; set; }
        public List<Questions> questions { get; set; }

        public ProductAnswer()
        {
            questions = new List<Questions>();
        }
    }

    public class Repport
    {
        public List<MissionAnswer> mission_answers { get; set; }

        public List<products_presented> products_presented { get; set; }

        public string duration { get; set; }
        public string receptivity { get; set; }
        public string authority { get; set; }
        public string visit_quality { get; set; }
        public string waiting_time { get; set; }
        public string gps_duration { get; set; }
        public string category { get; set; } = null;
        public string dual_with { get; set; } = null;
        public DateTime? created_at { get; set; } = null;
        public List<Material_Delivred> material_delivered { get; set; }
        public bool focus { get; set; }
        public string client_note { get; set; }
        public string agent_note { get; set; }
        public bool important { get; set; } = false;

        public Repport()
        {
            mission_answers = new List<MissionAnswer>();
            products_presented = new List<products_presented>();
            material_delivered = new List<Material_Delivred>();
        }
    }

    public class Product
    {
        public string product { get; set; }
        public double price { get; set; }
        public double quantity { get; set; }
        public double? discount { get; set; }
    }

    public class Order
    {
        public List<Product> products { get; set; }
        public double? total { get; set; }

        public Order()
        {
            products = new List<Product>();
        }
    }

    public class Responce_Model
    {
        public Mission mission { get; set; }

        public Repport repport { get; set; }

        public Order order { get; set; }

        public Responce_Model()
        {
            mission = new Mission();
            repport = new Repport();
            order = new Order();
        }
    }

    public class unvailability_mission
    {
        public Unvailability mission { get; set; }

        public unvailability_mission()
        {
            mission = new Unvailability();
        }
    }

    public class Unvailability
    {
        public bool visited { get; set; }
        public string duration { get; set; }
        public DateTime? visit_time { get; set; } = null;
        public List<string> coords { get; set; }
        public bool valid { get; set; }
        public string unavailability_reason { get; set; }
        public string replacement { get; set; }

        public Unvailability()
        {
            coords = new List<string>();
        }
    }

    public class products_presented
    {
        public string product { get; set; }
        public doctor doctor { get; set; }

        public pharmacy pharmacy { get; set; }

        public products_presented()
        {
            doctor = new doctor();
            pharmacy = new pharmacy();
        }
    }

    public class doctor
    {
        public bool prescribed { get; set; }
        public string prescription_frequency { get; set; }
        public string no_prescribed_reason { get; set; }
        public string no_interessed_reason { get; set; }
        public string rival_product { get; set; }
        public int samples { get; set; } = 0;
    }

    public class pharmacy
    {
        public bool available { get; set; }
        public string unavailability_reason { get; set; }
        public string refusal_reason { get; set; }
        public string rival_product { get; set; }
        public bool reorder { get; set; }
        public string rotation { get; set; }
        public int stock { get; set; } = 0;
    }

    public class Material_Delivred
    {
        public string product { get; set; }
        public int quantity { get; set; }
    }

    public class Absence_mission
    {
        public string absence { get; set; }
    }

    public class Responce_free_Model
    {
        public Free_client client { get; set; }

        public Repport repport { get; set; }

        public Order order { get; set; }
        public DateTime report_date { get; set; }

        public Responce_free_Model()
        {
            client = new Free_client();
            repport = new Repport();
            order = new Order();
            report_date = new DateTime();
        }
    }

    public class Free_client
    {
        public bool valid { get; set; }
        public bool visited { get; set; }
        public string client { get; set; }
        public string wilaya { get; set; }
        public string city { get; set; }
        public bool focus { get; set; }
        public bool dual { get; set; }
        public DateTime? visit_time { get; set; } = null;
        public DateTime? visit_end_time { get; set; } = null;
        public List<string> coords { get; set; }

        public Free_client()
        {
            coords = new List<string>();
        }
    }

    public class Free_Unvailability
    {
        public bool visited { get; set; }
        public string duration { get; set; }
        public DateTime? visit_time { get; set; } = null;
        public List<string> coords { get; set; }
        public bool valid { get; set; }
        public string unavailability_reason { get; set; }
        public string client { get; set; }

        public Free_Unvailability()
        {
            coords = new List<string>();
        }
    }
}