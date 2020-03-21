using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LS_Report.Models
{
    public class Products_Model : IEquatable<Products_Model>
    {
        public int quantity { get; set; }
        public string _id { get; set; }
        public string name { get; set; }
        public string soundex { get; set; }
        public string[] category { get; set; }
        public string type { get; set; }
        public string dci { get; set; }
        public string form { get; set; }
        public double price { get; set; }
        public string unit { get; set; }
        public double VAT { get; set; }
        public List<Rival_product> concurrents { get; set; }
        public List<Product_questions> questionnaires { get; set; }
        public List<TechnicalSheet> technical_sheet { get; set; }
        public string avatar { get; set; }

        public int __v { get; set; }
        public double? discount { get; set; } = 0;

        public ImageSource Avatar
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(avatar))
                {
                    return new UriImageSource
                    {
                        Uri = new Uri("http://149.202.133.17/" + avatar),
                    };
                }
                else
                {
                    return ImageSource.FromFile("box.png");
                }
            }
        }

        public bool Equals(Products_Model other)
        {
            if (other is null)
                return false;

            return this._id == other._id;
        }

        public override bool Equals(object obj) => Equals(obj as Products_Model);

        public override int GetHashCode() => (_id).GetHashCode();

        public Products_Model()
        {
            //  concurrent = new List<Rival_product>();
            questionnaires = new List<Product_questions>();
            technical_sheet = new List<TechnicalSheet>();
        }
    }

    public class Product_questions
    {
        public List<string> answers { get; set; }
        public string id { get; set; }
        public string question { get; set; }
    }

    public class Rival_product
    {
        public string labo { get; set; }
        public string name { get; set; }
        public string dci { get; set; }
        public string type { get; set; }
        public int packaging { get; set; } = 0;
        public string form { get; set; }
        public string dose { get; set; }
        public double price { get; set; }
        public bool discount { get; set; }

        public bool promotion { get; set; }

        [JsonIgnore]
        public string Promotion
        {
            get
            {
                if (promotion)
                    return "Oui";
                else
                    return "Non";
            }
        }

        [JsonIgnore]
        public string Discount
        {
            get
            {
                if (discount)
                    return "Oui";
                else
                    return "Non";
            }
        }

        public string rotation { get; set; }
        public string concurrent_of { get; set; }
        public string avatar { get; set; }

        public ImageSource Avatar
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(avatar))
                {
                    return new UriImageSource
                    {
                        Uri = new Uri("http://149.202.133.17/" + avatar),
                    };
                }
                else
                {
                    return null;
                }
            }
        }
    }

    public class TechnicalSheet
    {
        public string _id { get; set; }
        public string title { get; set; }
        public string info { get; set; }
    }
}