using Mopups.Services;
using NovaApp.Models;
using NovaApp.Popups;
using NovaApp.ViewModels;
using System.Diagnostics;

namespace NovaApp.Views
{
    public partial class ClientPage : ContentView
    {
        private ClientViewModel _viewModel;

        public ClientPage()
        {
            InitializeComponent();
            _viewModel = new ClientViewModel(new Services.RestService());
            BindingContext = _viewModel;

            LoadClients(); // Call the LoadClients method in the constructor
        }

        public async void LoadClients()
        {
            await _viewModel.FetchAllClients();
        }

        private void OnAddNewClicked(object sender, EventArgs e)
        {
            MopupService.Instance.PushAsync(new AddClient());
        }

        private void OnEditTapped(object sender, EventArgs e)
        {
            var clientId = _viewModel.SelectedClient?.id; // Assuming 'id' is the property holding the client's ID
            if (clientId != null)
            {
                MopupService.Instance.PushAsync(new UpdateClient(clientId));
            }
        }

        private async void OnCardTapped(object sender, EventArgs e)
        {
            Debug.WriteLine("executing card tap");
            var clientId = ((sender as Image)?.BindingContext as Client)?.id; // Adjust property name accordingly
            if (clientId != null)
            {
                await _viewModel.FetchClientById(clientId);
            }
        }
    }
}