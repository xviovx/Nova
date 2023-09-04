﻿using NovaApp.Models;
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
    internal class ProjectsViewModel: BaseViewModel
    {
        public ProjectsRestService _projectsRestService { get; set; }

        public ObservableCollection<Project> ProjectsList { get; set; }

        public ProjectsViewModel(ProjectsRestService projectsRestService)
        {
            _projectsRestService = projectsRestService;
            ProjectsList = new ObservableCollection<Project>();
        }

        public async Task FetchAllProjects()
        {
            var items = await _projectsRestService.RefreshProjectsListAsync();
            ProjectsList.Clear();
            foreach (var item in items)
            {
                ProjectsList.Add(item);
            }
        }

    }
}