using Mopups.Services;
using NovaApp.Models;
using NovaApp.Services;
using NovaApp.ViewModels;
using System.Diagnostics;

namespace NovaApp.Popups.Projects
{
    public partial class AddProject
    {
        private ClientViewModel _clientViewModel;
        private ProjectsViewModel _projectsViewModel;

        public AddProject()
        {
            InitializeComponent();

            // Initialize the ClientViewModel
            _clientViewModel = new ClientViewModel(new RestService());

            // Initialize the ProjectsViewModel
            _projectsViewModel = new ProjectsViewModel(new ProjectsRestService());

            // Set the BindingContext for the entire page
            BindingContext = _projectsViewModel;

            // Load clients (you can call this method from either view model)
            LoadClients();

            // Set the BindingContext for specific elements to the ClientViewModel
            PickerClientOwner.BindingContext = _clientViewModel;


        }

        public async void LoadClients()
        {
            // Call the FetchAllClients method from the ClientViewModel
            await _clientViewModel.FetchAllClients();
        }

        private void OnCloseButtonClicked(object sender, EventArgs e)
        {
            MopupService.Instance.PopAllAsync();
        }

        private void OnPickerClientOwnerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is Picker picker)
            {
                if (picker.SelectedItem is Client selectedClient)
                {
                    // Store the selected client in your ProjectsViewModel
                    _projectsViewModel.SelectedClient = selectedClient;

                    // Print the selected client properties to the debug output
                    Debug.WriteLine($"Selected Client: {selectedClient.username}, ID: {selectedClient.id}, ...");
                }
            }
        }
    }

}