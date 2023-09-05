using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaApp.Models
{
    public class Project
    {
        public string id { get; set; }
        public string title { get; set; }
        public string Status { get; set; } //DASHBOARD
        public DateTime? CompletedDate { get; set; } //DASHBAORD

        public DateTime deadlineDate { get; set; }
        public string deadlineDateString { get; set; }

        public ClientOwner clientOwner { get; set; }
    }

    public class ClientOwner
    {
        public string id { get; set; }
        public string username { get; set; }
    }
}
