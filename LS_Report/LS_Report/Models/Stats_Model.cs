using System;
using System.Collections.Generic;
using System.Text;

namespace LS_Report.Models
{
    public class Stats_Model
    {
        public string _id { get; set; }
        public Agent agent { get; set; }
        public int completed_missions { get; set; }
        public int uncompleted_missions { get; set; }
        public int holiday { get; set; }
        public int sick { get; set; }
        public int tot { get; set; }
        public int worked { get; set; }
        public int visited { get; set; }
        public double visited_percentage { get; set; }
        public int not_visited { get; set; }
        public int doctors { get; set; }
        public int pharmacies { get; set; }
        public int @private { get; set; }
        public int @public { get; set; }
        public int gps_correct { get; set; }
        public int gps_incorrect { get; set; }
        public int CM { get; set; }
        public int SR { get; set; }
        public int HR { get; set; }
        public int A_frequency { get; set; }
        public int B_frequency { get; set; }
        public int C_frequency { get; set; }
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public int focus { get; set; }
        public int dual { get; set; }
        public List<Speciality> specialities { get; set; }
    }
    public class Agent
    {
        public string _id { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string network { get; set; }
        public string manager { get; set; }
    }

    public class Speciality
    {
        public string name { get; set; }
        public int visits { get; set; }
    }
}
