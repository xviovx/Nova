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


        //Add Project
        Task SaveProjectAsync(Project project, bool isNewItem = false);
    }
}
