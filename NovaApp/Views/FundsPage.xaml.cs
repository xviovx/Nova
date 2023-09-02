using Microcharts;
using NovaApp.Models;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NovaApp.Views
{
    public partial class FundsPage : ContentView, INotifyPropertyChanged
    {
        private string _totalReceivedLabel;
        public string TotalReceivedLabel
        {
            get { return _totalReceivedLabel; }
            set
            {
                if (_totalReceivedLabel != value)
                {
                    _totalReceivedLabel = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _totalSpentLabel;
        public string TotalSpentLabel
        {
            get { return _totalSpentLabel; }
            set
            {
                if (_totalSpentLabel != value)
                {
                    _totalSpentLabel = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _totalFundsLabel;
        public string TotalFundsLabel
        {
            get { return _totalFundsLabel; }
            set
            {
                if (_totalFundsLabel != value)
                {
                    _totalFundsLabel = value;
                    OnPropertyChanged();
                }
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private HttpClient httpClient = new HttpClient();
        private ChartEntry[] receivedEntries;
        private ChartEntry[] spentEntries;
        public ObservableCollection<Fund> FundsList { get; set; } = new ObservableCollection<Fund>();
        public ObservableCollection<ObservableCollection<Fund>> GroupedFundsList { get; set; } = new ObservableCollection<ObservableCollection<Fund>>();
        static readonly string BaseUrl = "http://localhost:3000";

        public FundsPage()
        {
            InitializeComponent();
            _ = LoadItems();
            BindingContext = this;
            filterCards.SelectedIndex = 0;
            filterCards.SelectedIndexChanged += OnFilterCardsSelectedIndexChanged;

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

        public async Task LoadItems()
        {
            try
            {
                var items = await GetFundsAsync();

                if (items != null)
                {
                    FundsList.Clear();
                    foreach (var item in items)
                    {
                        FundsList.Add(item);
                    }
                }

                GroupProjects();
                CalculateTotals();
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Unhandled exception: {e.Message}", "OK");
            }
        }

        public async Task<List<Fund>> GetFundsAsync()
        {
            try
            {
                var response = await httpClient.GetAsync($"{BaseUrl}/funds");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var fundsList = JsonSerializer.Deserialize<List<Fund>>(json);
                    return fundsList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Debug", $"Exception: {e.Message}", "OK");
                return null;
            }
        }

        public void GroupProjects()
        {
            ObservableCollection<Fund> tempGroup = null;

            for (int i = 0; i < FundsList.Count; i++)
            {
                if (i % 2 == 0)
                {
                    tempGroup = new ObservableCollection<Fund>();
                    GroupedFundsList.Add(tempGroup);
                }
                tempGroup.Add(FundsList[i]);
            }
        }

        private void OnFilterCardsSelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = filterCards.Items[filterCards.SelectedIndex];
            SortFunds(selectedValue);
        }

        private void SortFunds(string selectedValue)
        {
            switch (selectedValue)
            {
                case "Spent Asc.":
                    FundsList = new ObservableCollection<Fund>(FundsList.OrderBy(f => f.expenses));
                    break;
                case "Received Asc.":
                    FundsList = new ObservableCollection<Fund>(FundsList.OrderBy(f => f.income));
                    break;
                case "Spent Desc.":
                    FundsList = new ObservableCollection<Fund>(FundsList.OrderByDescending(f => f.expenses));
                    break;
                case "Received Desc.":
                    FundsList = new ObservableCollection<Fund>(FundsList.OrderByDescending(f => f.income));
                    break;
            }
            GroupedFundsList.Clear();
            GroupProjects();
            OnPropertyChanged(nameof(GroupedFundsList));
        }

        public void CalculateTotals()
        {
            decimal totalReceived = FundsList.Sum(f => f.income);
            decimal totalSpent = FundsList.Sum(f => f.expenses);
            decimal totalFunds = totalReceived - totalSpent;

            TotalReceivedLabel = $"R{totalReceived:0,0.00}";
            TotalSpentLabel = $"R{totalSpent:0,0.00}";
            TotalFundsLabel = $"R{totalFunds:0,0.00}";
        }

    }
}
