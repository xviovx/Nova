using NovaApp.Models;
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
        private ProjectsViewModel _projectsViewModel;
        public ObservableCollection<Project> Projects
        {
            get { return _projectsViewModel.ProjectsList; }
        }

        public DashboardViewModel(ProjectsViewModel projectsViewModel)
        {
            _projectsViewModel = projectsViewModel;
        }

        public void PrintProjectTitles()
        {
            foreach (var project in Projects)
            {
                Console.WriteLine(project.title); // Print project title to console
            }
        }


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

    }
}
