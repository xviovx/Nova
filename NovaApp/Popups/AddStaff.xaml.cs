namespace NovaApp.Popups;
using Mopups.Services;
using NovaApp.ViewModels;
using System.Diagnostics;

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

    private void IsEmployeeType_Checked(object sender, CheckedChangedEventArgs e)
    {
        bool isAdmin = !e.Value; // If Employee is checked, Admin is not checked
        bool isEmployee = e.Value;

        PositionPicker.IsVisible = isEmployee;
        PasswordLayout.IsVisible = isAdmin;
    }

    private void IsAdminType_Checked(object sender, CheckedChangedEventArgs e)
    {
        bool isAdmin = e.Value;
        bool isEmployee = !e.Value; // If Admin is checked, Employee is not checked

        PositionPicker.IsVisible = isEmployee;
        PasswordLayout.IsVisible = isAdmin;
    }


}