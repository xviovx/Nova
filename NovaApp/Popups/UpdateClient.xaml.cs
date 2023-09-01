using NovaApp.Components;
using NovaApp.Models;
using Mopups.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NovaApp.ViewModels;

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
        }

        private async Task InitializeAsync(string clientId)
        {
            Debug.WriteLine("initializing");

            // Adjust how you obtain the client ID here, e.g., from a parameter
            // var clientId = ((sender as Image)?.BindingContext as Client)?.id;

            if (clientId != null)
            {
                await _viewModel.FetchClientById(clientId);
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
