﻿using NovaApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaApp.ViewModels
{
    public class DashboardViewModel

    {

        private ProjectsViewModel _projectsViewModel; //NEW
        public ObservableCollection<Project> BusyProjects => _projectsViewModel.ProjectsList; //NEW
        public ObservableCollection<Project> Projects { get; set; }

        //adding PROJECT properties
        public long Id { get; set; }
        public string title { get; set; }

        public string Description { get; set; } = string.Empty;

        public User ClientOwner { get; set; }

        public List<int> Jobs { get; set; } = new List<int>();

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? DeadlineDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public List<Note> Notes { get; set; } = new List<Note>();

        public List<Fund> Funds { get; set; } = new List<Fund>();

        public int? Profile { get; set; }


        public DashboardViewModel(ProjectsViewModel projectsViewModel)
        {
            Projects = new ObservableCollection<Project>();
            _projectsViewModel = projectsViewModel;
        }

        public void FetchBusyProjects()
        {
            _projectsViewModel.FetchBusyProjects();
        }


        //public static implicit operator DashboardViewModel(ProjectsViewModel v)
        //{
          //  throw new NotImplementedException();
        //}
    }
}