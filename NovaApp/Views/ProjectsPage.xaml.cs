using System;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace NovaApp.Views;

public partial class ProjectsPage : ContentView
{
    static string BaseUrl = "http://localhost:3000";
    static HttpClient client = new HttpClient();
    public ProjectsPage()
    {

        InitializeComponent();
        Debug.WriteLine("Projects lanched");
        Task.Run(async () => await GetProjects());

    }

    public static async Task GetProjects()
    {
        HttpResponseMessage response = await client.GetAsync(new Uri(new Uri(BaseUrl), "/projects"));
        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            // Process responseBody here
            Debug.WriteLine(responseBody);
        }
        else
        {
            Debug.WriteLine("Request failed");
        }
    }
}