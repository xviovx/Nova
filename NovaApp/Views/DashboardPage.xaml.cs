using Microcharts;
using NovaApp.Models;
using NovaApp.Services;
using NovaApp.ViewModels;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace NovaApp.Views

{

    public partial class DashboardPage : ContentView, INotifyPropertyChanged //Client Total
    {

        private DashboardViewModel viewModel;

        //Project total count
        private ProjectsPage projectsPage;
        private int _totalProjectsCount;
        public int TotalProjectsCount
        {
            get { return _totalProjectsCount; }
            set
            {
                if (_totalProjectsCount != value)
                {
                    _totalProjectsCount = value;
                    OnPropertyChanged();
                }
            }
        }

        //Client total count
        private RestService _restService;
        private string _totalClientsText;
        //Staff total count
        private string _totalStaffText;

        public string TotalClientsText
        {
            get { return _totalClientsText; }
            set
            {
                if (_totalClientsText != value)
                {
                    _totalClientsText = value;
                    OnPropertyChanged();
                }
            }
        }

        //staff total count
        public string TotalStaffText
        {
            get { return _totalStaffText; }
            set
            {
                if (_totalStaffText != value)
                {
                    _totalStaffText = value;
                    OnPropertyChanged();
                }
            }
        }

        public object NavigationManager { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ChartEntry[] projectZeroCompletion;
        private ChartEntry[] projectOneCompletion;
        private ChartEntry[] projectTwoCompletion;
        private ChartEntry[] projectThreeCompletion;

        public DashboardPage()
        {

            InitializeComponent();

            viewModel = new DashboardViewModel(new ProjectsViewModel(new ProjectsRestService()));
            BindingContext = viewModel;


            viewModel.FetchBusyProjects();

            _restService = new RestService();
            BindingContext = this;

            projectsPage = new ProjectsPage();
            projectsPage.LoadItems();

            TotalProjectsCount = projectsPage.TotalProjectsCount;

            UpdateTotalClientsCount();
            UpdateTotalStaffCount();

            //BindingContext = projectsPage;

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


        }
        private async void UpdateTotalClientsCount()
        {
            try
            {
                var clients = await _restService.RefreshClientsAsync();
                if (clients != null)
                {
                    TotalClientsText = $"{clients.Count}";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                Debug.WriteLine($"Error loading clients: {ex.Message}");
            }
        }

        private async void UpdateTotalStaffCount()
        {
            try
            {
                var staff = await _restService.RefreshStaffAsync();
                if (staff != null)
                {
                    TotalStaffText = $"{staff.Count}";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                Debug.WriteLine($"Error loading staff: {ex.Message}");
            }
        }


        //navigate folder1.png to staff page
        //private async void OnImageTapped(object sender, EventArgs e)
        //{
        //var staffPageWrapper = new StaffPageWrapper();
        //await Navigation.PushAsync(staffPageWrapper);
        //}

        private async void OnImageButtonClicked(object sender, EventArgs e)
        {
            // Navigate to another page when the image button is clicked
            await Navigation.PushAsync(new StaffPageWrapper());
        }




    }
}
