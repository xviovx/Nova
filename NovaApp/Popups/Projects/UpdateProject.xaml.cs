using Mopups.Services;
using NovaApp.Models;
using NovaApp.Services;
using NovaApp.ViewModels;
using System.Diagnostics;

namespace NovaApp.Popups.Projects
{
    public partial class UpdateProject
    {
        private ClientViewModel _clientViewModel;
        private ProjectsViewModel _projectsViewModel;

        public UpdateProject(string projectId)
        {
            InitializeComponent();

            // Initialize the ClientViewModel
            _clientViewModel = new ClientViewModel(new RestService());

            // Initialize the ProjectsViewModel
            _projectsViewModel = new ProjectsViewModel(new ProjectsRestService());

            // Fetch the selected project asynchronously
            Debug.WriteLine(projectId);
            InitializeAsync(projectId);

            // Set the BindingContext for the entire page
            BindingContext = _projectsViewModel;

            // Load clients (you can call this method from either view model)
            LoadClients();

            // Set the BindingContext for specific elements to the ClientViewModel
            PickerClientOwner.BindingContext = _clientViewModel;


            // Set default values based on fetched client data
            SetDefaultValues();

        }

        private async Task InitializeAsync(string projectId)
        {
            Debug.WriteLine("initializing");

            // Adjust how you obtain the client ID here, e.g., from a parameter
            // var clientId = ((sender as Image)?.BindingContext as Client)?.id;

            if (projectId != null)
            {
                try
                {
                    await _projectsViewModel.GetProjectById(projectId);
                    if (_projectsViewModel.FetchedProjectData != null)
                    {
                        // Set the FetchedStaffData property with the fetched data
                        _projectsViewModel.FetchedProjectData = _projectsViewModel.FetchedProjectData;

                        // Set the selected staff member
                        _projectsViewModel.SelectedProject = _projectsViewModel.FetchedProjectData; // Add this line

                        Debug.WriteLine($"Fetched project: {_projectsViewModel.FetchedProjectData.title}");

                        SetDefaultValues();
                    }
                    else
                    {
                        // Handle the case when fetched data is null
                        Debug.WriteLine("Fetched project data is null.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions that may occur during data fetching
                    Debug.WriteLine($"Error fetching project data: {ex.Message}");
                }
            }
        }




        private void SetDefaultValues()
        {
            // Set default values based on the fetched project data
            if (_projectsViewModel.FetchedProjectData != null)
            {
                Title.Text = _projectsViewModel.FetchedProjectData.title;
                Description.Text = _projectsViewModel.FetchedProjectData.description;
                //BaseCost.Text = _projectsViewModel.FetchedProjectData.baseCost;

                // Set the default deadline date
                 DeadlineDate.Date = _projectsViewModel.FetchedProjectData.deadlineDate;
                





                // Set the default selected client owner
                if (_projectsViewModel.FetchedProjectData.clientOwner != null)
                {
                    var selectedClient = _projectsViewModel.ClientList.FirstOrDefault(c => c.id == _projectsViewModel.FetchedProjectData.clientOwner.id);
                    if (selectedClient != null)
                    {
                        PickerClientOwner.SelectedItem = selectedClient;
                    }
                }
            }
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