using Mopups.Services;
using NovaApp.Popups;

namespace NovaApp.Views;

public partial class ClientPage : ContentView
{
	public ClientPage()
	{
		InitializeComponent();
	}

    private void OnAddNewClicked(object sender, EventArgs e)
    {
        MopupService.Instance.PushAsync(new AddClient());
    }
}