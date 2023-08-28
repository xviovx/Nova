using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaApp.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string ProfileImage { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string StaffType { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int Salary { get; set; }
        public string Password { get; set; } = string.Empty;
        public int AvailableHours { get; set; }
        public bool Active { get; set; }
    }
}