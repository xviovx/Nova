using Nova;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace NovaApp.Views;


public partial class LoginPage : ContentPage
{
    static string BaseUrl = "http://localhost:3000";
    static HttpClient client = new HttpClient();
    public LoginPage()
    {
        InitializeComponent();
        NavigationPage.SetHasBackButton(this, false);
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;

        bool loginResult = await OnLogoutHandler(username, password);

        if (loginResult)
        {
            await Navigation.PushAsync(new MainPage());
            // Navigate to the dashboard or main page
        }
        else
        {
            await DisplayAlert("Login Failed", "Invalid credentials. Please try again.", "OK");
        }
    }




    public async Task<bool> OnLogoutHandler(string username, string password)
    {
        if (username != null && password != null)
        {
            string loginUrl = $"{BaseUrl}/users/signin";

            var postData = new { email = username, password = password };
            string jsonData = JsonSerializer.Serialize(postData);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(loginUrl, content);

            string responseBody = await response.Content.ReadAsStringAsync();

            Debug.WriteLine(responseBody);

            string jwtToken = "your_jwt_token_here";

            await SecureStorage.SetAsync("JWT", jwtToken);

            return true; //Login Successful
        }
        return false;
    }
}