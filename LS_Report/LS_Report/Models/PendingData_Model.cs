using System;

namespace LS_Report.Models
{
    public class PendingData_Model
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Error { get; set; }
        public string Type { get; set; }
        public int Id { get; set; }
        public DateTime? Last_Sync { get; set; } = null;
    }
}