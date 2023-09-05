using Mopups.Services;

namespace NovaApp.Popups.Projects;

public partial class UpdateProject
{
	public UpdateProject(string ProjectId)
	{
		InitializeComponent();
	}

    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        MopupService.Instance.PopAllAsync();
    }
}