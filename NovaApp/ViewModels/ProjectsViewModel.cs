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

        private Project _selectedProject;
        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                if (_selectedProject != value)
                {
                    _selectedProject = value;
                    OnPropertyChanged(nameof(SelectedProject));
                }
            }
        }

        public ProjectsViewModel(ProjectsRestService projectsRestService)
        {
            _projectsRestService = projectsRestService;
            ProjectsList = new ObservableCollection<Project>();
            SelectedProject = new Project();
        }

        public async Task FetchAllProjects()
        {
            var items = await _projectsRestService.RefreshProjectsListAsync();
            ProjectsList.Clear();
            foreach (var item in items)
            {
                DateTime dateToConvert = item.deadlineDate;
                var FormattedDate = dateToConvert.ToString("MMMM dd, yyyy");
                item.deadlineDateString = FormattedDate;
                ProjectsList.Add(item);
            }

            Debug.WriteLine($"Number of projects fetched: {ProjectsList.Count}");
        }

        public async Task FetchProject(string ProjectId)
        {
            var Project = await _projectsRestService.GetProject(ProjectId);

            DateTime dateToConvert = Project.deadlineDate;
            var FormattedDate = dateToConvert.ToString("MMMM dd, yyyy");
            Project.deadlineDateString = FormattedDate;
            SelectedProject = Project;
            Debug.WriteLine("project title: ", SelectedProject.title);

        }




    }

}
