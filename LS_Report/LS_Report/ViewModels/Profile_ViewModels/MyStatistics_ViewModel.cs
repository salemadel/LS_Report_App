using LS_Report.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LS_Report.ViewModels.Profile_ViewModels
{
    public class MyStatistics_ViewModel : INotifyPropertyChanged
    {
        private List<Stats_Model> Stats_List { get; set; }
        private Token_Model Token { get; set; }
        public Stats_Model Stats { get; set; }
        public string Visite_Objectif
        {
            get { return Stats.visited.ToString() + " / " + (Token.objective * 4).ToString(); }
        }
        public string Doctor_Objectif
        {
            get { return Stats.doctors.ToString() + " / " + (Token.objective_doctor * 4).ToString(); }
        }
        public string Pharmacy_Objectif
        {
            get { return Stats.pharmacies.ToString() + " / " + ((Token.objective - Token.objective_doctor) * 4).ToString(); }
        }
        public string Title
        {
            get { return "Mes Statistiques mois "+DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("fr")) +"."; }
        }
        public MyStatistics_ViewModel(List<Stats_Model> stats, Token_Model token)
        {
            Stats_List = stats;
            Token = token;
            if (Stats_List.Count > 0)
            {
                Stats = Stats_List.First();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}