using Mopups.Services;
using NovaApp.ViewModels;
using System.Diagnostics;

namespace NovaApp.Popups;

public partial class AddProject
{
    private ProjectsViewModel _viewModel;
    public AddProject()
	{
        InitializeComponent();
        //_viewModel = new ProjectsViewModel(new Services.RestService()); //init our service
        //_viewModel = new ProjectsViewModel(new Services.RestService()); //init our service
        BindingContext = _viewModel; //the context of the xaml is this viewmodel

    }

    public void OnCloseButtonClicked(object sender, EventArgs e)
    {
        try
        {
            MopupService.Instance.PopAllAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"An error occurred while popping: {ex.Message}");

        }
    }
}