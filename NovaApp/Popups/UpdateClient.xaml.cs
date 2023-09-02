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
                await _viewModel.GetClientById(clientId);
                Debug.WriteLine($"Fetched username: {_viewModel.FetchedClientData.username}");
                Debug.WriteLine($"Fetched email: {_viewModel.FetchedClientData.email}");
                SetDefaultValues();
            }
        }

        private void SetDefaultValues()
        {
            // Set default values based on the fetched client data
            if (_viewModel.FetchedClientData != null)
            {
                Username.Text = _viewModel.FetchedClientData.username;
                Email.Text = _viewModel.FetchedClientData.email;

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
    }
}
