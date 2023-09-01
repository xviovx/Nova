using NovaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaApp.Class
{
    public class StaffWrapper
    {
        public Staff OriginalStaff { get; set; } // Reference to the original Staff object
        public string ProfileImageUrl { get; set; }
        public string Salary { get; set; }
    }

}
