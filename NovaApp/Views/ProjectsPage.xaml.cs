using System.ComponentModel; 
using NovaApp.ViewModels;
using Mopups.Services;
using NovaApp.Popups.Projects;
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
            Debug.WriteLine("Loading Projects");
            await ProjectsViewModel.FetchAllProjects();
        }

        private void OnAddNewProjectClicked(object sender, EventArgs e)
        {
            MopupService.Instance.PushAsync(new AddProject());
        }
    }
}
