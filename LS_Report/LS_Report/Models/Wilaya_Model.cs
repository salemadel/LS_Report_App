using System;

namespace LS_Report.Models
{
    public class Wilaya_Model : IEquatable<Wilaya_Model>
    {
        public string id { get; set; }
        public string code { get; set; }
        public string nom { get; set; }

        public bool Equals(Wilaya_Model other)
        {
            if (other is null)
                return false;

            return this.id == other.id;
        }

        public override bool Equals(object obj) => Equals(obj as Wilaya_Model);

        public override int GetHashCode() => (id).GetHashCode();
    }

    public class Commune
    {
        public double Latitude { get; set; }
        public double Langitude { get; set; }
        public string id { get; set; }
        public string wilaya_id { get; set; }
        public string nom { get; set; }
        public string code_postal { get; set; }
    }
}