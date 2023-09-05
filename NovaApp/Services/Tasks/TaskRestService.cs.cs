using NovaApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;

namespace NovaApp.Services.Tasks
{
    public class TaskRestService : IRestTaskService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;

        internal string BaseUrl = "http://localhost:3000/";

        public TaskRestService()
        {
            _client = new HttpClient();

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task SaveTaskAsync(CreateTaskDto task, string projectId)
        {
            Uri uri = new Uri(string.Format(BaseUrl + "projects/" + projectId + "/jobs"));
            try
            {
                string jsonData = JsonSerializer.Serialize(task);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(uri, content);

                Debug.WriteLine("Request URI: " + uri.ToString());
                Debug.WriteLine("Request Type: POST");
                if (response.IsSuccessStatusCode)
                {
                    string contentResponse = await response.Content.ReadAsStringAsync();

                    // Log the successful response content
                    Debug.WriteLine("Successful Response Content: " + contentResponse);
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
        }
    }
}
