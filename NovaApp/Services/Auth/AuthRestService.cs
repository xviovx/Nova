using NovaApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NovaApp.Services.Auth
{
    public class AuthRestService : IAuthRestService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;

        internal string BaseUrl = "http://localhost:3000/";

        public AuthRestService()
        {
            _client = new HttpClient();

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public UserData UserData { get; set; }

        public async Task OnLogin(UserData loginData)
        {
            Uri uri = new Uri(string.Format(BaseUrl + "users/signin"));
            try
            {
                // Create a JSON object with email and password

                // Serialize the login data to JSON
                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(uri, content);

                Debug.WriteLine("Request URI: " + uri.ToString());
                Debug.WriteLine("Request Type: POST");

                if (response.IsSuccessStatusCode)
                {

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Successful Response Content: " + responseContent);

                    UserData = JsonSerializer.Deserialize<UserData>(responseContent, _serializerOptions);

                    // Place data in local storage
                    await SecureStorage.SetAsync("UserProfile", UserData.profileImage.ToString());
                    await SecureStorage.SetAsync("Username", UserData.username);
                    await SecureStorage.SetAsync("UserEmail", UserData.email);

                    // Log the successful response content
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