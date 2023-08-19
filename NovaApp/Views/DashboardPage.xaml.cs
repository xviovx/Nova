using Microcharts;
using SkiaSharp;

namespace NovaApp.Views
{
    public partial class DashboardPage : ContentView
    {
        private ChartEntry[] receivedEntries;

        public DashboardPage()
        {
            InitializeComponent();

            // dummy data for received entries
            receivedEntries = new[]
            {
                new ChartEntry(200) { Label = "Mon", ValueLabel = "200", Color = SKColor.Parse("#5BDA8C") },
                new ChartEntry(250) { Label = "Tue", ValueLabel = "250", Color = SKColor.Parse("#5BDA8C") },
            };

            var chart = new DonutChart
            {
                Entries = receivedEntries,
                LabelTextSize = 12f
            };

            // assign chart to view
            chartView.Chart = chart;
        }
    }
}
