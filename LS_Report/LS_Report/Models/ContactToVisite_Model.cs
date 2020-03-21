using System;
using Xamarin.Forms;

namespace LS_Report.Models
{
    public class ContactToVisite_Model : Client2
    {
        public string Global_Id { get; set; }
        public string Mission_Id { get; set; }
        public string Categorie { get; set; }
        public DateTime VisiteDate { get; set; }
        public bool Visited { get; set; }

        public Color Visited_Color
        {
            get
            {
                if (Visited)
                {
                    return Color.FromHex("#285EB5");
                }
                else
                {
                    return Color.FromHex("#E8721C");
                }
            }
        }
    }
}