using System;
using System.Collections.Generic;

namespace LS_Report.Models
{
    public class ContactVisiteHistory_Model
    {
        public string _id { get; set; }
        public Client2 client { get; set; }
        public string category { get; set; }
        public string receptivity { get; set; }
        public string authority { get; set; }
        public string visit_quality { get; set; }
        public string waiting_time { get; set; }
        public string duration { get; set; }
        public string agent_note { get; set; }
        public string client_note { get; set; }
        public List<H_ProductsPresented> products_presented { get; set; }
        public DateTime created_at { get; set; }

        public string DateSort
        {
            get { return created_at.ToString("dd/MM/yyyy"); }
        }
    }

    public class H_ProductsPresented
    {
        public pharmacy pharmacy { get; set; }
        public string _id { get; set; }
        public Products_Model product { get; set; }
        public doctor doctor { get; set; }
    }
}