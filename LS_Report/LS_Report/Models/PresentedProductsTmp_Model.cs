using System.Collections.Generic;

namespace LS_Report.Models
{
    internal class PresentedProductsTmp_Model
    {
    }

    public class tmp_presented_products_doctor
    {
        public string id_product { get; set; }
        public string product { get; set; }
        public bool prescribed { get; set; }
        public string prescribed_frequency { get; set; }
        public string no_prescribed_reason { get; set; }
        public string no_interessed_reason { get; set; }
        public string rival_product { get; set; }
        public int samples_delivred { get; set; }

        public string Prescribed
        {
            get
            {
                if (prescribed == true)
                {
                    return "Oui ,prescription " + prescribed_frequency;
                }
                else
                {
                    if (prescribed == false)
                    {
                        if (!string.IsNullOrWhiteSpace(no_interessed_reason))
                        {
                            return "Non ," + no_interessed_reason;
                        }
                        else
                        {
                            return "Non ," + no_prescribed_reason;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            set
            {
                if (value == "Oui")
                {
                    prescribed = true;
                }
                else
                {
                    prescribed = false;
                }
            }
        }
    }

    public class tmp_pretented_products_pharmacy
    {
        public string id_product { get; set; }
        public string product { get; set; }
        public bool available { get; set; }
        public string rotation { get; set; }
        public string unavailability_reason { get; set; }
        public string refusal_reason { get; set; }
        public string rival_product { get; set; }
        public bool reorder { get; set; }
        public int stock { get; set; }

        public string Available
        {
            get
            {
                if (available == true)
                {
                    return "Oui ," + rotation;
                }
                else
                {
                    if (available == false)
                    {
                        if (!string.IsNullOrWhiteSpace(unavailability_reason))
                        {
                            if (unavailability_reason == "Non renouvelé / Refus de commandé")
                            {
                                return "Non ," + refusal_reason;
                            }
                            else
                            {
                                return "Non ," + unavailability_reason;
                            }
                        }
                        else
                        {
                            return "Non";
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            set
            {
                if (value == "Oui")
                {
                    available = true;
                }
                else
                {
                    available = false;
                }
            }
        }
    }

    public class products_stats
    {
        public string product_name { get; set; }
        public int repeat { get; set; }
        public string taux { get; set; }
        public int positif_repeat { get; set; }
        public string prescribed_availabel { get; set; }
        public string type { get; set; }
    }

    public class tmp_questionnaire
    {
        public string id_questionnaire { get; set; }
        public string title { get; set; }
        public string question { get; set; }
        public string id_question { get; set; }
        public string answer { get; set; }
        public List<string> answers { get; set; }
    }
}