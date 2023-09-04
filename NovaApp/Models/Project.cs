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

        public ClientOwner clientOwner { get; set; }
    }

    public class ClientOwner
    {
        public string id { get; set; }
        public string username { get; set; }
    }
}
