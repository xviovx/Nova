using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaApp.Models
{
    public class Client
    {
        public string id { get; set; }
        public string username { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string clientType { get; set; } = string.Empty;
        public int availableHours { get; set; }
        public bool? active { get; set; }

    }
}