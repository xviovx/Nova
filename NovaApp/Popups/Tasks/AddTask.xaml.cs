using Mopups.Services;

namespace NovaApp.Popups.Tasks;

public partial class AddTask
{
	public AddTask(string ProjectId)
	{
		InitializeComponent();
	}

    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        MopupService.Instance.PopAllAsync();
    }
}