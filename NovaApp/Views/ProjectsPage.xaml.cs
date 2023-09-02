using NovaApp.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using System.ComponentModel; // Add this namespace for INotifyPropertyChanged

namespace NovaApp.Views
{
    public partial class ProjectsPage : ContentView, INotifyPropertyChanged
    {
        private static readonly string BaseUrl = "http://localhost:3000";
        public ProjectsPage()
        {
            InitializeComponent();
            _ = LoadItems();
            BindingContext = this;
        }

        // ObservableCollection for your projects list
        public ObservableCollection<Project> ProjectsList { get; set; } = new ObservableCollection<Project>();

        // DASHBOARD: Private field for TotalProjectsCount
        private int totalProjectsCount = 0;

        // DASHBOARD: Public property for TotalProjectsCount
        public int TotalProjectsCount
        {
            get { return totalProjectsCount; }
            set
            {
                if (totalProjectsCount != value)
                {
                    totalProjectsCount = value;
                    OnPropertyChanged(nameof(TotalProjectsCount)); // Notify property change
                    Debug.WriteLine($"TotalProjectsCount changed to {value}");
                }
            }
        }



        // Load items and update TotalProjectsCount
        public async Task LoadItems()
        {
            var items = await GetProjectsAsync();
            foreach (var item in items)
            {
                ProjectsList.Add(item);
            }

            // DASHBOARD: Update TotalProjectsCount
            TotalProjectsCount = items.Count;


            Debug.WriteLine($"TotalProjectsCount: {TotalProjectsCount}");
        }

        // DASHBOARD Implement
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        // https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/rest
        public static async Task<List<Project>> GetProjectsAsync()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(new Uri(new Uri(BaseUrl), "/projects")); // Replace with your API URL
            var json = await response.Content.ReadAsStringAsync();
            var projectsList = JsonSerializer.Deserialize<List<Project>>(json);
            return projectsList;
        }


        //DASHBOARD
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
