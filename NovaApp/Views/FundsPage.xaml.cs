using Microcharts;
using NovaApp.Models;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace NovaApp.Views
{
    public partial class FundsPage : ContentView
    {
        private ChartEntry[] receivedEntries;
        private ChartEntry[] spentEntries;

        public FundsPage()
        {
            InitializeComponent();
            filterCards.SelectedIndex = 0;

            // dummy data for received entries
            receivedEntries = new[]
            {
                new ChartEntry(200) { Label = "Mon", ValueLabel = "200", Color = SKColor.Parse("#5BDA8C") },
                new ChartEntry(250) { Label = "Tue", ValueLabel = "250", Color = SKColor.Parse("#5BDA8C") },
                new ChartEntry(400) { Label = "Wed", ValueLabel = "400", Color = SKColor.Parse("#5BDA8C") },
                new ChartEntry(300) { Label = "Thu", ValueLabel = "300", Color = SKColor.Parse("#5BDA8C") },
                new ChartEntry(190) { Label = "Fri", ValueLabel = "190", Color = SKColor.Parse("#5BDA8C") },
                new ChartEntry(600) { Label = "Sat", ValueLabel = "600", Color = SKColor.Parse("#5BDA8C") },
                new ChartEntry(110) { Label = "Sun", ValueLabel = "110", Color = SKColor.Parse("#5BDA8C") }
            };

            // dummy data for spent entries
            spentEntries = new[]
            {
                new ChartEntry(110) { Label = "Mon", ValueLabel = "110", Color = SKColor.Parse("#EE6B8D") },
                new ChartEntry(600) { Label = "Tue", ValueLabel = "600", Color = SKColor.Parse("#EE6B8D") },
                new ChartEntry(190) { Label = "Wed", ValueLabel = "190", Color = SKColor.Parse("#EE6B8D") },
                new ChartEntry(300) { Label = "Thu", ValueLabel = "300", Color = SKColor.Parse("#EE6B8D") },
                new ChartEntry(400) { Label = "Fri", ValueLabel = "400", Color = SKColor.Parse("#EE6B8D") },
                new ChartEntry(250) { Label = "Sat", ValueLabel = "250", Color = SKColor.Parse("#EE6B8D") },
                new ChartEntry(200) { Label = "Sun", ValueLabel = "200", Color = SKColor.Parse("#EE6B8D") }
            };

            dataPicker.SelectedIndex = 0;

            // create chart
            var chart = new LineChart
            {
                Entries = receivedEntries,
                LineMode = LineMode.Straight,
                LabelOrientation = Orientation.Horizontal
            };

            // change appearance
            chart.LabelTextSize = 12f;

            // assign chart to view
            chartView.Chart = chart;
        }

        private void OnDataPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            // handle selection change of picker
            if (dataPicker.SelectedIndex == 0) // received
            {
                var chart = new LineChart
                {
                    Entries = receivedEntries,
                    LineMode = LineMode.Straight,
                    LabelOrientation = Orientation.Horizontal
                };
                chart.LabelTextSize = 12f;
                chartView.Chart = chart;
            }
            else if (dataPicker.SelectedIndex == 1) // spent
            {
                var chart = new LineChart
                {
                    Entries = spentEntries,
                    LineMode = LineMode.Straight,
                    LabelOrientation = Orientation.Horizontal
                };
                chart.LabelTextSize = 12f;
                chartView.Chart = chart;
            }
        }

        public ObservableCollection<Fund> FundsList { get; set; } = new ObservableCollection<Fund>();
        static readonly string BaseUrl = "http://localhost:3000";
        private HttpClient httpClient = new HttpClient();

        public async Task LoadItems()
        {
            var items = await GetFundsAsync();
            foreach (var item in items)
            {
                FundsList.Add(item);
            }
        }

        public async Task<List<Fund>> GetFundsAsync()
        {
            try
            {
                var response = await httpClient.GetAsync(new Uri(new Uri(BaseUrl), "/funds"));
                response.EnsureSuccessStatusCode(); // Ensure the HTTP request was successful

                var json = await response.Content.ReadAsStringAsync();
                var fundsList = JsonSerializer.Deserialize<List<Fund>>(json);
                return fundsList;
            }
            catch (Exception ex)
            {
                // Handle the exception gracefully (show an error message, log, etc.)
                Console.WriteLine($"Error fetching funds: {ex.Message}");
                return new List<Fund>(); // Return an empty list or handle appropriately
            }
        }
    }
}
