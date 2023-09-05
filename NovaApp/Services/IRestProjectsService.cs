using NovaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaApp.Services
{
    public interface IRestProjectsService
    {
        Task<List<Project>> RefreshProjectsListAsync();
        Task<Project> GetProject(string ProjectId);
    }
}
