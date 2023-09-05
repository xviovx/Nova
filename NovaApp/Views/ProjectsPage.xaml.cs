using System.ComponentModel; 
using NovaApp.ViewModels;
using Mopups.Services;
using NovaApp.Popups.Projects;

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

        private void OnAddNewProjectClicked(object sender, EventArgs e)
        {
            MopupService.Instance.PushAsync(new AddProject());
        }
    }
}
