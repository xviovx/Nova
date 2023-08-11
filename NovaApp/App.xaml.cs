using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Nova;

namespace NovaApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Create a new NavigationPage and customize the navigation bar
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Colors.Transparent, // Hide the background color of the navigation bar
                BarTextColor = Colors.Black, // Set the text color of the navigation bar items

            };
        }

        protected override void OnStart()
        {
            base.OnStart();
        }
    }
}