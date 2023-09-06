using Nova;
using NovaApp.Models;
using NovaApp.ViewModels;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace NovaApp.Views;


public partial class LoginPage : ContentPage
{
    static string BaseUrl = "http://localhost:3000";
    static HttpClient client = new HttpClient();

    private MainPageViewModel MainPageViewModel;
    public LoginPage()
    {
        InitializeComponent();
        NavigationPage.SetHasBackButton(this, false);
        NavigationPage.SetHasNavigationBar(this, false);
        MainPageViewModel = new MainPageViewModel(new Services.Auth.AuthRestService());
        BindingContext = MainPageViewModel;
    }

    public async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        var email = EmailEntry.Text;
        var password = PasswordEntry.Text;
        await MainPageViewModel.onLogin(email, password);
        CheckForLoggedInUser();
    }

    private async void CheckForLoggedInUser()
    {
        var localDataCheck = await MainPageViewModel.GetLocalData();
        if (localDataCheck)
        {
            await Navigation.PushAsync(new MainPage());
            return;
        }
        await Navigation.PushAsync(new LoginPage());
        return;

    }


}