using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaApp.Models
{
    public class CreateTaskDto
    {
        public string title { get; set; }
        public string description { get; set; }
        public string assignedUserId { get; set; }
        public string workHours { get; set; }
    }

    public class TaskDisplay
    {
        public string title { get; set; }
        public string description { get; set; }
        public string assignedUserId { get; set; }
        public int workHours { get; set; }
        public string profileImage { get; set; }
    }
}
