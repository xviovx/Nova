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

    private void OnImageTapped(object sender, EventArgs e)
    {
        try
        {
            if (sender is Image tappedImage)
            {
                // Reset opacity for all images within the named StackLayout
                foreach (var child in ImageStackLayout.Children)
                {
                    if (child is Image image)
                    {
                        image.Opacity = 0.3;
                    }
                }

                // Set tapped image's opacity to 1 and update SelectedImageSource
                tappedImage.Opacity = 1;
                _viewModel.SelectedImageSource = tappedImage.Source.ToString(); // Store the selected image source
            }
        }
        catch (Exception ex)
        {
            // Handle the exception here
            Debug.WriteLine($"An exception occurred: {ex.Message}");
        }
    }






}