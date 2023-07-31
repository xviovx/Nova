using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Nova;

namespace NovaApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        MainPage = new MainPage();
    }
}
