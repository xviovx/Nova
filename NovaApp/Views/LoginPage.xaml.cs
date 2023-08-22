using Nova;

namespace NovaApp.Views;

public partial class LoginPage : ContentPage
{
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
        if (username == "user" && password == "pass")
        {
            // Simulate getting a JWT token from a server
            string jwtToken = "your_jwt_token_here";

            // Store the JWT token securely
            await SecureStorage.SetAsync("JWT", jwtToken);

            return true; // Login successful
        }

        return false; // Login failed
    }
}