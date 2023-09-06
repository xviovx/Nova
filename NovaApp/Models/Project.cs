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
        public bool status { get; set; } //DASHBOARD
        public DateTime deadlineDate { get; set; }
        public string deadlineDateString { get; set; }
        public string description { get; set; }
        public List<TaskDisplay> jobs { get; set; }
        public ClientOwner clientOwner { get; set; }
        public int progress { get; set; }
        public List<FundItem> funds { get; set; }
        public int totalExpenses { get; set; }
        public int totalIncome { get; set; }

        public int baseCost { get; set; }


    }

    public class FundItem
    {
        public string id { get; set; }
        public int expenses { get; set; }
        public int income { get; set; }
    }

    public class createProjectDto
    {
        public string title { get; set; }
        public string description { get; set; }
        public DateTime deadlineDate { get; set; }
        public string clientOwner { get; set; }
        public int baseCost { get; set; }
    }

    public class ClientOwner
    {
        public string id { get; set; }
        public string username { get; set; }
    }
}
