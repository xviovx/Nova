using NovaApp.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using System.ComponentModel; // Add this namespace for INotifyPropertyChanged
using NovaApp.ViewModels;
using System.Security.Cryptography.X509Certificates;

namespace NovaApp.Views
{
    public partial class ProjectsPage : INotifyPropertyChanged
    {
        private ProjectsViewModel ProjectsViewModel;
        public ProjectsPage()
        {
            InitializeComponent();
            ProjectsViewModel = new ProjectsViewModel(new Services.ProjectsRestService());
            BindingContext = ProjectsViewModel;
            LoadProjects();

        }

        public async void LoadProjects()
        {
            await ProjectsViewModel.FetchAllProjects();
        }
    }
}
