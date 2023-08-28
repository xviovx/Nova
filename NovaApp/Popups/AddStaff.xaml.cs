namespace NovaApp.Popups;
using Mopups.Services;

public partial class AddStaff
{
	public AddStaff()
	{
		InitializeComponent();
	}

    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        MopupService.Instance.PopAllAsync();
    }
}