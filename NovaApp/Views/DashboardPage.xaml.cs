using Microcharts;
using NovaApp.Models;
using NovaApp.ViewModels;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace NovaApp.Views

{

    public partial class DashboardPage : ContentView //Client Total
    {

        private DashboardViewModel viewModel;

        private ProjectsViewModel ProjectsViewModel;

        //Project total count
        public object NavigationManager { get; private set; }

        private ChartEntry[] projectZeroCompletion;
        private ChartEntry[] projectOneCompletion;
        private ChartEntry[] projectTwoCompletion;
        private ChartEntry[] projectThreeCompletion;

        public DashboardPage()
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

        private async void OnImageButtonClicked(object sender, EventArgs e)
        {
            // Navigate to another page when the image button is clicked
            await Navigation.PushAsync(new StaffPageWrapper());
        }




    }
}
