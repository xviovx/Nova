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
        public int workHours { get; set; }
    }

    public class TaskDisplay
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string assignedUserId { get; set; }
        public int workHours { get; set; }
        public string profileImage { get; set; }
        public bool status { get; set; }
        public AssignedUser assignedUser { get; set; }
    }

    public class AssignedUser
    {
        public string userId { get; set; }
        public int profileImage { get; set; }
        public string profileImageUrl { get; set; }
    }
}
