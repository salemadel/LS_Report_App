using LS_Report.Converters;
using LS_Report.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LS_Report.ViewModels.Visites_ViewModel
{
    public class Statistics_ViewModel
    {
        private List<ContactVisiteHistory_Model> Contacts_List { get; set; }
        private List<tmp_presented_products_doctor> products_Doctors = new List<tmp_presented_products_doctor>();
        private List<tmp_pretented_products_pharmacy> products_Pharmacies = new List<tmp_pretented_products_pharmacy>();
        public ObservableCollection<Grouping<string, products_stats>> Stats { get; set; }
        public int Doctors_Number { get; set; }
        public int Pharmacy_Number { get; set; }

        public Statistics_ViewModel(List<ContactVisiteHistory_Model> contacts_list)
        {
            Contacts_List = contacts_list;
            Initialize();
        }

        private void Initialize()
        {
            List<products_stats> stats_doctor = new List<products_stats>();
            List<products_stats> stats_pharmacy = new List<products_stats>();

            foreach (var history in Contacts_List)
            {
                if (history != null)
                {
                    if (history.products_presented != null && history.products_presented.Count > 0)
                    {
                        if (history.client.business_type == "Médecin" || history.client.business_type == "Sage-femme" || history.client.business_type == "Chirugien-dentiste")
                        {
                            Doctors_Number += 1;
                            foreach (var presented in history.products_presented)
                            {
                                tmp_presented_products_doctor tmp = new tmp_presented_products_doctor();
                                tmp.product = presented.product.name;

                                tmp.prescribed = presented.doctor.prescribed;
                                tmp.no_interessed_reason = presented.doctor.no_interessed_reason;
                                tmp.no_prescribed_reason = presented.doctor.no_prescribed_reason;
                                tmp.prescribed_frequency = presented.doctor.prescription_frequency;
                                tmp.samples_delivred = presented.doctor.samples;
                                tmp.rival_product = presented.doctor.rival_product;
                                products_Doctors.Add(tmp);
                            }
                        }
                        else
                        {
                            Pharmacy_Number += 1;
                            foreach (var presented in history.products_presented)
                            {
                                tmp_pretented_products_pharmacy tmp = new tmp_pretented_products_pharmacy();
                                tmp.product = presented.product.name;
                                tmp.available = presented.pharmacy.available;
                                tmp.refusal_reason = presented.pharmacy.refusal_reason;
                                tmp.rival_product = presented.pharmacy.rival_product;
                                tmp.rotation = presented.pharmacy.rotation;
                                tmp.unavailability_reason = presented.pharmacy.unavailability_reason;
                                products_Pharmacies.Add(tmp);
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }

            foreach (var doctor in products_Doctors)
            {
                if (!stats_doctor.Exists(i => i.product_name == doctor.product))
                {
                    var stat = new products_stats();
                    stat.product_name = doctor.product;

                    stat.positif_repeat = (doctor.prescribed == true) ? 1 : 0;
                    stat.repeat = 1;
                    stat.taux = Math.Round(((Convert.ToDouble(stat.positif_repeat) / Convert.ToDouble(stat.repeat)) * 100), 2).ToString() + " %";
                    stat.prescribed_availabel = stat.positif_repeat.ToString() + "/" + stat.repeat.ToString();
                    stat.type = "Produit Présenter au Médecins";
                    stats_doctor.Add(stat);
                }
                else
                {
                    var old_stat = stats_doctor.Single(i => i.product_name == doctor.product);
                    var new_stat = new products_stats();
                    new_stat.positif_repeat = (doctor.prescribed == true) ? old_stat.positif_repeat + 1 : old_stat.positif_repeat;
                    new_stat.product_name = old_stat.product_name;
                    new_stat.repeat = old_stat.repeat + 1;
                    new_stat.type = "Produit Présenter au Médecins";
                    new_stat.prescribed_availabel = new_stat.positif_repeat.ToString() + "/" + new_stat.repeat.ToString();
                    new_stat.taux = Math.Round(((Convert.ToDouble(new_stat.positif_repeat) / Convert.ToDouble(new_stat.repeat)) * 100), 2).ToString() + " %";
                    stats_doctor.Remove(old_stat);
                    stats_doctor.Add(new_stat);
                }
            }
            foreach (var pharmacy in products_Pharmacies)
            {
                if (!stats_pharmacy.Exists(i => i.product_name == pharmacy.product))
                {
                    var stat = new products_stats();
                    stat.product_name = pharmacy.product;
                    stat.positif_repeat = (pharmacy.available == true) ? 1 : 0;
                    stat.repeat = 1;
                    stat.taux = Math.Round(((Convert.ToDouble(stat.positif_repeat) / Convert.ToDouble(stat.repeat)) * 100), 2).ToString() + " %";
                    stat.prescribed_availabel = stat.positif_repeat.ToString() + "/" + stat.repeat.ToString();
                    stat.type = "Produit Présenter au Pharmaciens";
                    stats_pharmacy.Add(stat);
                }
                else
                {
                    var old_stat = stats_pharmacy.Single(i => i.product_name == pharmacy.product);
                    var new_stat = new products_stats();
                    new_stat.positif_repeat = (pharmacy.available == true) ? old_stat.positif_repeat + 1 : old_stat.positif_repeat;
                    new_stat.product_name = old_stat.product_name;
                    new_stat.repeat = old_stat.repeat + 1;
                    new_stat.type = "Produit Présenter au Pharmaciens";
                    new_stat.prescribed_availabel = new_stat.positif_repeat.ToString() + "/" + new_stat.repeat.ToString();
                    new_stat.taux = Math.Round(((Convert.ToDouble(new_stat.positif_repeat) / Convert.ToDouble(new_stat.repeat)) * 100), 2).ToString() + " %";
                    stats_pharmacy.Remove(old_stat);
                    stats_pharmacy.Add(new_stat);
                }
            }

            Stats = new ObservableCollection<Grouping<string, products_stats>>();
            var tmp_stats = new List<products_stats>();
            tmp_stats.AddRange(stats_doctor);
            tmp_stats.AddRange(stats_pharmacy);
            var sorted = from contact in tmp_stats
                         orderby contact.type
                         group contact by contact.type into contactGroup
                         select new Grouping<string, products_stats>(contactGroup.Key, contactGroup);
            Stats = new ObservableCollection<Grouping<string, products_stats>>(sorted);
        }
    }
}