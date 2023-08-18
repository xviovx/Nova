using Microcharts;
using SkiaSharp;

namespace NovaApp.Views
{
    public partial class FundsPage : ContentView
    {
        public FundsPage()
        {
            InitializeComponent();

            // Create chart entries for funds received
            ChartEntry[] receivedEntries = new[]
            {
                new ChartEntry(200) { Label = "Monday", ValueLabel = "200", Color = SKColor.Parse("#00FF00") }, // Green
                new ChartEntry(250) { Label = "Tuesday", ValueLabel = "250", Color = SKColor.Parse("#00FF00") }, // Green
                // Add more entries for other days...
            };

            

            // Create the line chart with combined entries
            var chart = new LineChart
            {
                Entries = receivedEntries,
                LineMode = LineMode.Straight,
                LabelOrientation = Orientation.Horizontal // Set label orientation to horizontal
            };

            // Customize chart appearance
            chart.LabelTextSize = 25f; // Adjust font size

            // Assign the chart to your ChartView
            chartView.Chart = chart;
        }
    }
}
