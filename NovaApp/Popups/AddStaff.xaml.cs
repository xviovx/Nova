namespace NovaApp.Popups;
using Mopups.Services;
using NovaApp.ViewModels;

public partial class AddStaff
{
    private StaffViewModel _viewModel;
    public AddStaff()
	{
		InitializeComponent();
        _viewModel = new StaffViewModel(new Services.RestService()); //init our service
        BindingContext = _viewModel; //the context of the xaml is this viewmodel
    }

    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        MopupService.Instance.PopAllAsync();
    }
}