using System;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using NovaApp.Models;
using System.Text.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace NovaApp.Views;

public partial class ProjectsPage : ContentView
{
    

    private ProjectsViewModel viewModel;
    public List<Project> ProjectsList = null;
    public ProjectsPage()
    {

        InitializeComponent();
        viewModel = new ProjectsViewModel();
        BindingContext = viewModel;
        LoadProjects();
    }

    private async void LoadProjects()
    {
        await viewModel.LoadProjectsAsync();
    }


}

public class ProjectsViewModel : BindableObject
{
    static string BaseUrl = "http://localhost:3000";
    static HttpClient client = new HttpClient();
    private ObservableCollection<Project> projects = new ObservableCollection<Project>();
    public ObservableCollection<Project> Projects
    {
        get => projects;
        set
        {
            projects = value;
            OnPropertyChanged();
        }
    }

    public async Task LoadProjectsAsync()
    {
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetStringAsync(new Uri(new Uri(BaseUrl), "/projects")); // Replace with your API URL
            var projectsList = JsonSerializer.Deserialize<List<Project>>(response);
            Projects = new ObservableCollection<Project>(projectsList);
        }
    }
}