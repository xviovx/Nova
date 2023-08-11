using System;
using Microsoft.Maui.Controls;
using NovaApp.Views;
using Microsoft.Maui.Graphics;

namespace Nova
{
    public partial class MainPage : ContentPage
    {
        private void SetActiveButton(Button activeButton)
        {
            buttonView1.TextColor = Colors.Black;
            buttonView1.FontAttributes = FontAttributes.None; // Set inactive button text normal
            buttonView2.TextColor = Colors.Black;
            buttonView2.FontAttributes = FontAttributes.None; // Set inactive button text normal
            buttonView3.TextColor = Colors.Black;
            buttonView3.FontAttributes = FontAttributes.None; // Set inactive button text normal
            buttonView4.TextColor = Colors.Black;
            buttonView4.FontAttributes = FontAttributes.None; // Set inactive button text normal
            buttonView5.TextColor = Colors.Black;
            buttonView5.FontAttributes = FontAttributes.None; // Set inactive button text normal
            buttonView7.TextColor = Colors.Black;
            buttonView7.FontAttributes = FontAttributes.None; // Set inactive button text normal

            //buttonView6.TextColor = Colors.Black;
            //buttonView6.FontAttributes = FontAttributes.None; // Set inactive button text normal

            activeButton.TextColor = Colors.White; // Set active button color
            activeButton.FontAttributes = FontAttributes.Bold; // Set active button text bold

            tab.Margin = new Thickness(0, activeButton.Height + 5, 0, 0); // Position tab below the active button
            tab.Opacity = 1; // Show the tab
        }

        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            contentFrame.Content = new DashboardPage(); // Set the initial content to MyView1
            SetActiveButton(buttonView1);
        }

        private void OnView1Clicked(object sender, EventArgs e)
        {
            // Set the content to MyView1 when the "View 1" button is clicked
            contentFrame.Content = new DashboardPage();
            SetActiveButton(buttonView1);
        }

        private void OnClientClicked(object sender, EventArgs e)
        {
            // Set the content to MyView2 when the "View 2" button is clicked
            contentFrame.Content = new ClientPage();
            SetActiveButton(buttonView2);
        }

        private void OnStaffClicked(object sender, EventArgs e)
        {
            // Set the content to MyView2 when the "View 2" button is clicked
            contentFrame.Content = new StaffPage();
            SetActiveButton(buttonView3);
        }

        private void OnFundsClicked(object sender, EventArgs e)
        {
            // Set the content to MyView2 when the "View 2" button is clicked
            contentFrame.Content = new FundsPage();
            SetActiveButton(buttonView5);
        }

        private void OnProjectClicked(object sender, EventArgs e)
        {
            // Set the content to MyView2 when the "View 2" button is clicked
            contentFrame.Content = new ProjectsPage();
            SetActiveButton(buttonView4);
        }

        //private void OnLogInClicked(object sender, EventArgs e)
        //{
        //    // Set the content to MyView2 when the "View 2" button is clicked
        //    contentFrame.Content = new SignInPage();
        //    SetActiveButton(buttonView7);
        //}
        private async void OnLogInClicked(object sender, EventArgs e)
        {
            // Navigate to the LoginPage
            await Navigation.PushAsync(new LoginPage());
        }

        //private void OnLogInClicked(object sender, EventArgs e)
        //{
        // Set the content to MyView2 when the "View 6" button is clicked
        //contentFrame.Content = new LoginPage();
        //SetActiveButton(buttonView6);
        //}

    }
}
