using NovaApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;

namespace NovaApp.Services
{
    public class RestService : IRestService
    {
        //defining our httpclient
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;


        //base api url
        internal string baseUrl = "http://localhost:3000/users/";

        //list of Clients & Staff
        public List<Client> Clients { get; private set; }
        public List<Staff> StaffItem { get; private set; }

        //constructor - creating our httpclient
        public RestService()
        {
            _client = new HttpClient();

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        // Get all clients
        public async Task<List<Client>> RefreshClientsAsync()
        {
            Clients = new List<Client>();

            Uri uri = new Uri(string.Format($"{baseUrl}clients", string.Empty));
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);

                // Log request details
                Debug.WriteLine("Request URI: " + uri.ToString());
                Debug.WriteLine("Request Type: GET");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Clients = JsonSerializer.Deserialize<List<Client>>(content, _serializerOptions);

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

            return Clients;
        }


        // Add or update a Client
        public async Task SaveClientAsync(Client item, bool isNewItem = false)
        {
            Uri uri = new Uri(string.Format($"{baseUrl}clients", string.Empty));

            try
            {
                string json = JsonSerializer.Serialize<Client>(item, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewItem)
                {
                    response = await _client.PostAsync(uri, content);
                }
                else
                {
                    response = await _client.PutAsync(uri, content);
                }

                // Log JSON data
                Debug.WriteLine("Serialized JSON data: " + json);

                // Log request details
                Debug.WriteLine("Request URI: " + uri.ToString());
                Debug.WriteLine("Request Type: " + (isNewItem ? "POST" : "PUT"));

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Client successfully saved.");
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

        //update client
        public async Task UpdateClientAsync(Client item)
        {

            Debug.WriteLine($"Client: {item}");
            try
            {
                Uri uri = new Uri($"{baseUrl}{item.id}"); // Construct the URI with the item's ID

                Debug.WriteLine("URI: " + uri); // Debug: Log the constructed URI

                string json = JsonSerializer.Serialize<Client>(item, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                Debug.WriteLine("Request JSON: " + json); // Debug: Log the request JSON data

                // Create a new HttpRequestMessage with the HTTP PATCH method
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, uri);
                request.Content = content; // Set the content of the request

                Debug.WriteLine("PATCH Request Content Set"); // Debug: Log content setting

                HttpResponseMessage response = await _client.SendAsync(request); // Send the PATCH request

                Debug.WriteLine("PATCH Request Sent to: " + uri); // Debug: Log PATCH request

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Client successfully updated.");
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



        //Get all staff
        public async Task<List<Staff>> RefreshStaffAsync()
        {
            var staffList = new List<Staff>();

            Uri uri = new Uri($"{baseUrl}staff"); // Simplified URI construction

            try
            {
                Debug.WriteLine("Fetching staff data from: " + uri); // Debug: Log the URL being requested

                HttpResponseMessage response = await _client.GetAsync(uri);

                Debug.WriteLine("HTTP Status Code: " + response.StatusCode); // Debug: Log the HTTP status code

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    staffList = JsonSerializer.Deserialize<List<Staff>>(content, _serializerOptions);

                    Debug.WriteLine($"Fetched {staffList.Count} staff members"); // Debug: Log the count of staff members fetched
                }
                else
                {
                    Debug.WriteLine("Failed to fetch staff data. HTTP Status Code: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.ToString()); // Debug: Log any exceptions that occur
            }

            return staffList;
        }



        //add or update a Staff 
        public async Task SaveStaffAsync(Staff item, bool isNewItem = false)
        {
            Uri uri = new Uri($"{baseUrl}staff"); // Simplified URI construction

            try
            {
                string json = JsonSerializer.Serialize<Staff>(item, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewItem)
                {
                    response = await _client.PostAsync(uri, content);
                    Debug.WriteLine("POST Request Sent to: " + uri); // Debug: Log POST request
                }
                else
                {
                    response = await _client.PatchAsync(uri, content);
                    Debug.WriteLine("Patch Request Sent to: " + uri); // Debug: Log Patch request
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Staff successfully saved.");
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
        public async Task UpdateStaffAsync(Staff item)
        {
            try
            {
                Uri uri = new Uri($"{baseUrl}{item.id}"); // Construct the URI with the item's ID

                string json = JsonSerializer.Serialize<Staff>(item, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Create a new HttpRequestMessage with the HTTP PATCH method
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, uri);
                request.Content = content; // Set the content of the request

                HttpResponseMessage response = await _client.SendAsync(request); // Send the PUT request

                Debug.WriteLine("PATCH Request Sent to: " + uri); // Debug: Log PATCH request

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Staff successfully updated.");
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




        public async Task<Staff> GetStaffByIdAsync(string staffId)
        {
            Staff staff = null;

            try
            {
                Uri uri = new Uri($"{baseUrl}{staffId}"); // Construct the URL with the staff ID
                Debug.WriteLine("Fetching staff data by ID from: " + uri);

                HttpResponseMessage response = await _client.GetAsync(uri);

                Debug.WriteLine("HTTP Status Code: " + response.StatusCode);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    staff = JsonSerializer.Deserialize<Staff>(content, _serializerOptions);

                    Debug.WriteLine("Fetched staff data by ID: " + content);
                }
                else
                {
                    Debug.WriteLine("Failed to fetch staff data by ID. HTTP Status Code: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.ToString());
            }

            return staff;
        }



        public async Task<Client> GetClientByIdAsync(string clientId)
        {
            Client client = null;

            try
            {
                Uri uri = new Uri($"{baseUrl}{clientId}"); // Construct the URL with the staff ID
                Debug.WriteLine("Fetching client data by ID from: " + uri);

                HttpResponseMessage response = await _client.GetAsync(uri);

                Debug.WriteLine("HTTP Status Code: " + response.StatusCode);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    client = JsonSerializer.Deserialize<Client>(content, _serializerOptions);

                    Debug.WriteLine("Fetched client data by ID: " + content);
                }
                else
                {
                    Debug.WriteLine("Failed to fetch client data by ID. HTTP Status Code: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.ToString());
            }

            return client;
        }


    }
}