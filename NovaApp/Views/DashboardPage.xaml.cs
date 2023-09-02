using Microcharts;
using NovaApp.Models;
using NovaApp.Services;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;

namespace NovaApp.Views
{

    public partial class DashboardPage : ContentView
    {
        //Client total count
        private RestService _restService;
        public string TotalClientsText { get; set; }

        private ProjectsPage projectsPage;

        private ChartEntry[] projectZeroCompletion;
        private ChartEntry[] projectOneCompletion;
        private ChartEntry[] projectTwoCompletion;
        private ChartEntry[] projectThreeCompletion;

        public DashboardPage()
        {
            InitializeComponent();

            _restService = new RestService();
            BindingContext = this;


            //NEW
            projectsPage = new ProjectsPage();
            BindingContext = projectsPage;

            int totalProjectsCount = projectsPage.TotalProjectsCount;



            // dummy data for charts
            //chart zero data
            projectZeroCompletion = new[]
            {
                new ChartEntry(95)
                {
                    Label = "Complete",
                    ValueLabel = "95%",
                    Color = SKColor.Parse("#343434"),
                    ValueLabelColor = SKColors.Black,
                },
                new ChartEntry(5)
                {
                    Label = "Incomplete",
                    ValueLabel = "5%",
                    Color = SKColor.Parse("#EEEEEE"),
                    ValueLabelColor = SKColors.Gray, // Change the color as needed
                }
            };

            //chart one data
            projectOneCompletion = new[]
            {
                new ChartEntry(80)
                {
                    Label = "Complete",
                    ValueLabel = "80%",
                    Color = SKColor.Parse("#343434"),
                    ValueLabelColor = SKColors.Black,
                },
                new ChartEntry(20)
                {
                    Label = "Incomplete",
                    ValueLabel = "20%",
                    Color = SKColor.Parse("#EEEEEE"),
                    ValueLabelColor = SKColors.Gray, // Change the color as needed
                }
            };

            //chart two data
            projectTwoCompletion = new[]
            {
                new ChartEntry(55)
                {
                    Label = "Complete",
                    ValueLabel = "55%",
                    Color = SKColor.Parse("#343434"),
                    ValueLabelColor = SKColors.Black,
                },
                new ChartEntry(45)
                {
                    Label = "Incomplete",
                    ValueLabel = "45%",
                    Color = SKColor.Parse("#EEEEEE"),
                    ValueLabelColor = SKColors.Gray, // Change the color as needed
                }
            };

            //chart three data
            projectThreeCompletion = new[]
            {
                new ChartEntry(20)
                {
                    Label = "Complete",
                    ValueLabel = "20%",
                    Color = SKColor.Parse("#343434"),
                    ValueLabelColor = SKColors.Black,
                },
                new ChartEntry(80)
                {
                    Label = "Incomplete",
                    ValueLabel = "80%",
                    Color = SKColor.Parse("#EEEEEE"),
                    ValueLabelColor = SKColors.Gray, // Change the color as needed
                }
            };

            var chartZero = new DonutChart
            {
                Entries = projectZeroCompletion,
                LabelTextSize = 12f,
                LabelMode = LabelMode.LeftAndRight
            };

            var chartOne = new DonutChart
            {
                Entries = projectOneCompletion,
                LabelTextSize = 12f,
                LabelMode = LabelMode.LeftAndRight
            };

            var chartTwo = new DonutChart
            {
                Entries = projectTwoCompletion,
                LabelTextSize = 12f,
                LabelMode = LabelMode.LeftAndRight
            };

            var chartThree = new DonutChart
            {
                Entries = projectThreeCompletion,
                LabelTextSize = 12f,
                LabelMode = LabelMode.LeftAndRight
            };

            // assign chart to view
            chartViewZero.Chart = chartZero;
            chartViewOne.Chart = chartOne;
            chartViewTwo.Chart = chartTwo;
            chartViewThree.Chart = chartThree;

            UpdateTotalClientsCount();

        }
        private async void UpdateTotalClientsCount()
        {
            try
            {
                var clients = await _restService.RefreshClientsAsync();
                if (clients != null)
                {
                    TotalClientsText = $"Total Clients: {clients.Count}";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                Debug.WriteLine($"Error loading clients: {ex.Message}");
            }
        }




    }
}
