using System.ComponentModel; 
using NovaApp.ViewModels;
using Mopups.Services;
using NovaApp.Popups.Projects;
using NovaApp.Models;
using System.Diagnostics;

namespace NovaApp.Views
{
    public partial class ProjectsPage : ContentView
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
            await ProjectsViewModel.FetchProject(ProjectsViewModel.ProjectsList[0].id);
        }

        private void OnAddNewProjectClicked(object sender, EventArgs e)
        {
            MopupService.Instance.PushAsync(new AddProject());
        }

        private async void OnProjectClicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var clickedProjectId = (string)button.CommandParameter;
            Debug.WriteLine($"ImageButton clicked for projectId: {clickedProjectId}");

            await ProjectsViewModel.FetchProject(clickedProjectId);
        }
    }
}
