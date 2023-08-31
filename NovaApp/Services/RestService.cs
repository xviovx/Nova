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

        //Get all clients
        public async Task<List<Client>> RefreshClientsAsync()
        {
            Clients = new List<Client>();

            Uri uri = new(string.Format($"{baseUrl}clients", string.Empty));
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Clients = JsonSerializer.Deserialize<List<Client>>(content, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return Clients;
        }

        //add or update a Client 
        public async Task SaveClientAsync(Client item, bool isNewItem = false)
        {
            Uri uri = new(string.Format($"{baseUrl}clients", string.Empty));

            try
            {
                string json = JsonSerializer.Serialize<Client>(item, _serializerOptions);
                StringContent content = new(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewItem)
                    response = await _client.PostAsync(uri, content);
                else
                    response = await _client.PutAsync(uri, content);

                if (response.IsSuccessStatusCode)
                    Debug.WriteLine("Client successfully saved.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        //Get all staff
        public async Task<List<Staff>> RefreshStaffAsync()
        {
            var staffList = new List<Staff>();

            Uri uri = new(string.Format($"{baseUrl}staff", string.Empty));
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    staffList = JsonSerializer.Deserialize<List<Staff>>(content, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return staffList;
        }


        //add or update a Staff 
        public async Task SaveStaffAsync(Staff item, bool isNewItem = false)
        {
            Uri uri = new(string.Format($"{baseUrl}staff", string.Empty));

            try
            {
                string json = JsonSerializer.Serialize<Staff>(item, _serializerOptions);
                StringContent content = new(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewItem)
                    response = await _client.PostAsync(uri, content);
                else
                    response = await _client.PutAsync(uri, content);

                if (response.IsSuccessStatusCode)
                    Debug.WriteLine("Staff successfully saved.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR {0}", ex.Message);
            }
        }

    }
}