using Mopups.Services;

namespace NovaApp.Popups;

public partial class AddClient
{
	public AddClient()
	{
		InitializeComponent();
	}

    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        MopupService.Instance.PopAllAsync();
    }
}