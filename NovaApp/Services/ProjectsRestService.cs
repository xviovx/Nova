﻿using NovaApp.Models;
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
        public List<TaskDisplay> TaskList { get; set; }

        public Project SelectedProject { get; set; }

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

        public async Task<Project> GetProject(string ProjectId)
        {
            SelectedProject = new Project();
            Uri uri = new Uri(string.Format(BaseUrl + "projects/" + ProjectId));

            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);

                Debug.WriteLine("Request URI: " + uri.ToString());
                Debug.WriteLine("Request Type: GET");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    SelectedProject = JsonSerializer.Deserialize<Project>(content, _serializerOptions);

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
            return SelectedProject;
        }


        // Add or update a Project
        public async Task SaveProjectAsync(createProjectDto project, bool isNewItem = false)
        {
            string itemType = "projects"; // Specify that we're working with projects
            Uri uri = new Uri(string.Format($"{BaseUrl}{itemType}", string.Empty));

            try
            {
                string json = JsonSerializer.Serialize(project, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewItem)
                {
                    response = await _client.PostAsync(uri, content);
                }
                else
                {
                    response = await _client.PostAsync(uri, content);
                }

                // Log JSON data
                Debug.WriteLine("Serialized JSON data: " + json);

                // Log request details
                Debug.WriteLine("Request URI: " + uri.ToString());
                Debug.WriteLine("Request Type: " + (isNewItem ? "POST" : "PUT"));

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Project successfully saved.");
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

        public async Task ChangeTaskStatus(string TaskId)
        {
            Uri uri = new Uri(string.Format(BaseUrl + "jobs/change-status/" + TaskId));
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);

                Debug.WriteLine("Request URI: " + uri.ToString());
                Debug.WriteLine("Request Type: GET");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

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
        }

        public async Task<List<TaskDisplay>> GetTaskList(string ProjectId)
        {
            TaskList = new List<TaskDisplay>();

            Uri uri = new Uri(string.Format(BaseUrl + "projects/" + ProjectId + "/jobs"));
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);

                Debug.WriteLine("Request URI: " + uri.ToString());
                Debug.WriteLine("Request Type: GET");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    TaskList = JsonSerializer.Deserialize<List<TaskDisplay>>(content, _serializerOptions);

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

            return TaskList;
        }

        public async Task UpdateProjectAsync(string projectId, createProjectDto updatedProject)
        {
            try
            {
                Uri uri = new Uri($"{BaseUrl}projects/{projectId}"); // Construct the URI with the project's ID

                string json = JsonSerializer.Serialize(updatedProject, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Create a new HttpRequestMessage with the HTTP Put method
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, uri);
                request.Content = content; // Set the content of the request

                HttpResponseMessage response = await _client.SendAsync(request); // Send the PUT request

                Debug.WriteLine("PUT Request Sent to: " + uri); // Debug: Log PUT request

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Project successfully updated.");
                }
                else
                {
                    Debug.WriteLine("HTTP Status Code: " + response.StatusCode); // Debug: Log HTTP status code
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Response Content: " + responseContent); // Debug: Log response content
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.ToString()); // Debug: Log the exception details
            }
        }

    }
}
