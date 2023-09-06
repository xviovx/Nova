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

    public partial class DashboardPage : ContentView 
    {

        private ChartEntry[] receivedEntries;
        private DashboardViewModel viewModel;
        private StaffViewModel StaffViewModel;
        private ProjectsViewModel ProjectsViewModel;
        //private ClientViewModel ClientViewModel;


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
            StaffViewModel = new StaffViewModel(new Services.RestService());
            //ClientViewModel = new ClientViewModel(new Services.RestService());

            //BindingContext = ProjectsViewModel; //bind NumberOfProjects
            //this is the problem, when there is 2 binding
            BindingContext = StaffViewModel; //bind NumberOfStaff
            //BindingContext = ClientViewModel;

            LoadProjects();

            receivedEntries = new[]
            {
                new ChartEntry(200) { Label = "Jan", ValueLabel = "200", Color = SKColor.Parse("#FF7070") },
                new ChartEntry(250) { Label = "Feb", ValueLabel = "250", Color = SKColor.Parse("#FF7070") },
                new ChartEntry(400) { Label = "Mar", ValueLabel = "400", Color = SKColor.Parse("#FF7070") },
                new ChartEntry(300) { Label = "Apr", ValueLabel = "300", Color = SKColor.Parse("#FF7070") },
                new ChartEntry(190) { Label = "May", ValueLabel = "190", Color = SKColor.Parse("#FF7070") },
                new ChartEntry(600) { Label = "Jun", ValueLabel = "600", Color = SKColor.Parse("#FF7070") },
                new ChartEntry(110) { Label = "Jul", ValueLabel = "110", Color = SKColor.Parse("#FF7070") },
                new ChartEntry(120) { Label = "Aug", ValueLabel = "120", Color = SKColor.Parse("#FF7070") },
                new ChartEntry(130) { Label = "Sep", ValueLabel = "130", Color = SKColor.Parse("#FF7070") },
                new ChartEntry(140) { Label = "Oct", ValueLabel = "140", Color = SKColor.Parse("#FF7070") },
                new ChartEntry(150) { Label = "Nov", ValueLabel = "150", Color = SKColor.Parse("#FF7070") },
                new ChartEntry(160) { Label = "Dec", ValueLabel = "160", Color = SKColor.Parse("#FF7070") }
            };

            var chart = new LineChart
            {
                Entries = receivedEntries,
                LineMode = LineMode.Straight,
                LabelOrientation = Orientation.Horizontal
            };

            // Change appearance (optional)
            chart.LabelTextSize = 12f;

            // Assign chart to ChartView
            chartView.Chart = chart;
        }

        public async void LoadProjects()
        {
            await ProjectsViewModel.FetchAllProjects();
            await StaffViewModel.FetchAllStaff();
            //await ClientViewModel.FetchAllClients();

        }

        private async void OnImageButtonClicked(object sender, EventArgs e)
        {
            // Navigate to another page when the image button is clicked
            await Navigation.PushAsync(new StaffPageWrapper());
        }




    }
}
