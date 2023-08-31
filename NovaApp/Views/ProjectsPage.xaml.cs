using NovaApp.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace NovaApp.Views;

public partial class ProjectsPage : ContentView
{

    public ObservableCollection<Project> ProjectsList { get; set; } = new ObservableCollection<Project>();
    static readonly string BaseUrl = "http://localhost:3000";

    public ProjectsPage()
    {
        InitializeComponent();
         _ = LoadItems(); 
        BindingContext = this; 

    }


    public async Task LoadItems()
    {
        var items = await GetProjectsAsync();
        foreach (var item in items)
        {
            ProjectsList.Add(item);
        }
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
}