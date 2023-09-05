using Mopups.Services;

namespace NovaApp.Popups.Projects;

public partial class AddProject
{
    public AddProject()
	{
        InitializeComponent();
	}

    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        MopupService.Instance.PopAllAsync();
    }
}