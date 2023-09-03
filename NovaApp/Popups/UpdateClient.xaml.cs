using NovaApp.Components;
using NovaApp.Models;
using Mopups.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NovaApp.ViewModels;
using Windows.Networking;

namespace NovaApp.Popups
{
    public partial class UpdateClient
    {
        private ClientViewModel _viewModel;

        public UpdateClient(string clientId)
        {
            InitializeComponent();
            _viewModel = new ClientViewModel(new Services.RestService()); // Initialize our service

            // Fetch the selected client asynchronously
            Debug.WriteLine(clientId);
            InitializeAsync(clientId);

            // Set the BindingContext
            BindingContext = _viewModel;

            // Set default values based on fetched client data
            SetDefaultValues();
        }

        private async Task InitializeAsync(string clientId)
        {
            Debug.WriteLine("initializing");

            // Adjust how you obtain the client ID here, e.g., from a parameter
            // var clientId = ((sender as Image)?.BindingContext as Client)?.id;

            if (clientId != null)
            {
                try
                {
                    await _viewModel.GetClientById(clientId);
                    if (_viewModel.FetchedClientData != null)
                    {
                        // Set the FetchedStaffData property with the fetched data
                        _viewModel.FetchedClientData = _viewModel.FetchedClientData;

                        // Set the selected staff member
                        _viewModel.SelectedClient = _viewModel.FetchedClientData; // Add this line

                        Debug.WriteLine($"Fetched username: {_viewModel.FetchedClientData.username}");
                        Debug.WriteLine($"Fetched email: {_viewModel.FetchedClientData.email}");

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
                    Debug.WriteLine($"Error fetching client data: {ex.Message}");
                }
            }
        }




        private void SetDefaultValues()
        {
            // Set default values based on the fetched client data
            if (_viewModel.FetchedClientData != null)
            {
                Username.Text = _viewModel.FetchedClientData.username;
                Email.Text = _viewModel.FetchedClientData.email;
                // Set the button text based on the value of SelectedStaff.active
                DeactivateButton.Text = (_viewModel.FetchedClientData.active ?? false) ? "Deactivate" : "Activate";

                // Set radio button values based on the fetched client data
                if (_viewModel.FetchedClientData.clientsType == "Standard")
                {
                    StandardRadioButton.IsChecked = true;
                }
                else if (_viewModel.FetchedClientData.clientsType == "Priority")
                {
                    PriorityRadioButton.IsChecked = true;
                }
            }
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

        private void OnStandardRadioButtonClicked(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                DescriptionLabel.Text = "Allocated 15 hours a week";
            }
        }

        private void OnPriorityRadioButtonClicked(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                DescriptionLabel.Text = "Allocated 30 hours per week";
            }
        }

        private bool ValidateInput()
        {
            // Implement your validation logic here
            bool isValid = true;

            // Example validation: Check if the username is not empty
            if (string.IsNullOrWhiteSpace(Username.Text))
            {
                isValid = false;
                // Display an error message or set an error flag
                Debug.WriteLine("Username is required.");
            }

            return isValid;
        }
    }
}