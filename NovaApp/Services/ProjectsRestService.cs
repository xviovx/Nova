using NovaApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NovaApp.Services
{
    public class ProjectsRestService : IRestProjectsService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;

        internal string BaseUrl = "http://localhost:3000/";

        public List<Project> Projects { get; set; }

        public ProjectsRestService()
        {
            _client = new HttpClient();

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<List<Project>> RefreshProjectsListAsync()
        {
            Projects = new List<Project>();

            Uri uri = new Uri(string.Format(BaseUrl + "projects"));
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);

                Debug.WriteLine("Request URI: " + uri.ToString());
                Debug.WriteLine("Request Type: GET");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Projects = JsonSerializer.Deserialize<List<Project>>(content, _serializerOptions);

                    // Log the successful response content
                    Debug.WriteLine("Successful Response Content: " + content);
                }
                else
                {
                    // Log error response content
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Error Response Content: " + errorContent);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
            }

            return Projects;

        }
    }
}
