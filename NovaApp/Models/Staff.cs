using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaApp.Models
{
    public class Staff
    {
        public string id { get; set; }
        public int profileImage { get; set; }
        public string username { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string staffType { get; set; } = string.Empty;
        public string position { get; set; } = string.Empty;
        public int payPerHour { get; set; }
        public string password { get; set; } = string.Empty;
        public int availableHours { get; set; }
        public bool? active { get; set; }
    }
}