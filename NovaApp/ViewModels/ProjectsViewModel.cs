using NovaApp.Models;
using NovaApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaApp.ViewModels
{
    public class ProjectsViewModel : BaseViewModel
    {

        public ProjectsRestService _projectsRestService { get; set; }

        public ObservableCollection<Project> ProjectsList { get; set; }

        public ProjectsViewModel(ProjectsRestService projectsRestService)
        {
            _projectsRestService = projectsRestService;
            ProjectsList = new ObservableCollection<Project>();
        }

        public async Task FetchBusyProjects() //DASHBOARD
        {
            var items = await _projectsRestService.RefreshProjectsListAsync();
            ProjectsList.Clear();

            // Filter projects based on the Status property
            var busyProjects = items.Where(project => project.Status == "Busy").ToList();

            foreach (var item in busyProjects)
            {
                ProjectsList.Add(item);
            }

            foreach (var project in busyProjects)
            {
                Debug.WriteLine($"Busy Project ID: {project.id}, Title: {project.title}, Status: {project.Status}");
            }
        }


    }
}
