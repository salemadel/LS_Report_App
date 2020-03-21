using System;

namespace LS_Report.Models
{
    public class Token_Model
    {
        public string id { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string city { get; set; }
        public int objective { get; set; }
        public int objective_doctor { get; set; }
        public string network { get; set; }
        public string wilaya { get; set; }
        public DateTime created_at { get; set; }
        public double iat { get; set; }
        public double exp { get; set; }
        public string[] roles { get; set; }
        public string[] permissions { get; set; } = null;
        public string[] sectors { get; set; }
    }
}