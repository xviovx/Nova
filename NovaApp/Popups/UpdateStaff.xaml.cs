using Mopups.Services;
using NovaApp.Models;
using NovaApp.ViewModels;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Windows.Foundation.Collections;

namespace NovaApp.Popups;

public partial class UpdateStaff
{
    private StaffViewModel _viewModel;
    public UpdateStaff(string staffId)
    {
        InitializeComponent();
        _viewModel = new StaffViewModel(new Services.RestService()); // Initialize our service

        // Set the StaffId property with the provided staffId
        _viewModel.StaffId = staffId;

        // Fetch the selected staff asynchronously
        InitializeAsync(staffId);

        // Set the BindingContext
        BindingContext = _viewModel;

        // Set default values based on fetched staff data
        SetDefaultValues();
    }

    private void SetInitialSelectedImage(int imageIndex)
    {
        // Reset opacity for all images within the named StackLayout
        foreach (var child in ImageStackLayout.Children)
        {
            if (child is Image image)
            {
                image.Opacity = 0.3;
            }
        }

        // Find the image with the specified index and set its opacity to 1
        if (ImageStackLayout.Children.ElementAtOrDefault(imageIndex) is Image initialImage)
        {
            _viewModel.SelectedImageIndex = imageIndex +1;
            Debug.WriteLine($"Initial image: {imageIndex}");
            initialImage.Opacity = 1;
        }
    }

    private async Task InitializeAsync(string staffId)
    {
        Debug.WriteLine("Initializing");

        if (staffId != null)
        {
            try
            {
                // Fetch staff data asynchronously
                await _viewModel.GetStaffById(staffId);

                if (_viewModel.FetchedStaffData != null)
                {
                    // Set the FetchedStaffData property with the fetched data
                    _viewModel.FetchedStaffData = _viewModel.FetchedStaffData;

                    // Set the selected staff member
                    _viewModel.SelectedStaff = _viewModel.FetchedStaffData; // Add this line

                    Debug.WriteLine($"Fetched username: {_viewModel.FetchedStaffData.username}");
                    Debug.WriteLine($"Fetched email: {_viewModel.FetchedStaffData.email}");
                    Debug.WriteLine($"Fetched profileImage: {_viewModel.FetchedStaffData.profileImage}");

                    // Set the initial staff type based on the fetched data
                    _viewModel.IsEmployeeType = _viewModel.FetchedStaffData.staffType == "Employee";
                    _viewModel.IsAdminType = _viewModel.FetchedStaffData.staffType == "Administrative Staff";

                    // Set the selected profile image immediately after fetching data
                    SetInitialSelectedImage(_viewModel.FetchedStaffData.profileImage - 1);

                    // Set default values based on fetched staff data
                    SetDefaultValues();
                }
                else
                {
                    // Handle the case when fetched data is null
                    Debug.WriteLine("Fetched staff data is null.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions that may occur during data fetching
                Debug.WriteLine($"Error fetching staff data: {ex.Message}");
            }
        }
    }



    private void SetDefaultValues()
    {
        // Set default values based on the fetched client data
        if (_viewModel.FetchedStaffData != null)
        {
            if (_viewModel.FetchedStaffData != null)
            {

                string username = _viewModel.FetchedStaffData.username;
                Email.Text = _viewModel.FetchedStaffData.email;
                PasswordEntry.Text = _viewModel.FetchedStaffData.password;

                // Split the username by space
                string[] nameParts = username.Split(' ');

                if (nameParts.Length >= 2)
                {
                    // Set FirstName and LastName based on the split parts
                    FirstName.Text = nameParts[0];
                    LastName.Text = nameParts[1];
                }
                else if (nameParts.Length == 1)
                {
                    // If there is only one part, set it as FirstName
                    FirstName.Text = nameParts[0];
                    LastName.Text = string.Empty; // Set LastName as empty
                }
            }

            IsEmployeeType.IsChecked = _viewModel.FetchedStaffData.staffType == "Employee";
            IsAdminType.IsChecked = _viewModel.FetchedStaffData.staffType == "Administrative Staff";

            // Set default values based on the fetched staff data
            if (_viewModel.FetchedStaffData != null)
            {
                // Set the selected position from the fetched data
                _viewModel.Position = _viewModel.FetchedStaffData.position;

                // Set the button text based on the value of SelectedStaff.active
                DeactivateButton.Text = (_viewModel.FetchedStaffData.active ?? false) ? "Deactivate" : "Activate";
            }
        }
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
        Debug.WriteLine("Is Admin");
        bool isAdmin = e.Value;
        bool isEmployee = !e.Value; // If Admin is checked, Employee is not checked

        PasswordLayout.IsVisible = isAdmin; // Set password layout visibility
        PositionPicker.IsVisible = isEmployee; // Set position picker visibility
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

                // Extract the numeric part of the image name (assuming the name is like "X.png")
                Debug.WriteLine(tappedImage);
                string imageName = Path.GetFileNameWithoutExtension(tappedImage.Source.ToString());
                if (int.TryParse(imageName, out int imageIndex))
                {
                    Debug.WriteLine(imageName);
                    _viewModel.SelectedImageIndex = imageIndex; // Store the selected image index
                }
                else
                {
                    Debug.WriteLine("Failed to parse image index.");
                }
            }
        }
        catch (Exception ex)
        {
            // Handle the exception here
            Debug.WriteLine($"An exception occurred: {ex.Message}");
        }

    }
}