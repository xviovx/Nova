using Mopups.Services;
using NovaApp.Models;
using NovaApp.Popups;
using NovaApp.ViewModels;
using System.Diagnostics;
using NovaApp.Services;


namespace NovaApp.Views
{
    public partial class ClientPage : ContentView
    {
        private ClientViewModel _viewModel;
        private ProjectsViewModel ProjectsViewModel;

        public ClientPage()
        {
            InitializeComponent();
            ProjectsViewModel = new ProjectsViewModel(new Services.ProjectsRestService());

            ProjectList.BindingContext = ProjectsViewModel;

            _viewModel = new ClientViewModel(new Services.RestService());
            BindingContext = _viewModel;

            LoadClients(); // Call the LoadClients method in the constructor

            // Check if there are clients loaded
           
        }

        public async void LoadClients()
        {
            await _viewModel.FetchAllClients();
            if (_viewModel.ClientList.Count > 0)
            {
                // Get the first client's ID
                var firstClient = _viewModel.ClientList[0];
                var firstClientId = firstClient?.id;

                // Fetch projects and filter by the first client
                await ProjectsViewModel.FilterProjectsByClient(firstClientId);
            }
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
                _viewModel.SelectedClientID = clientId; // Set the selected client's ID
                ProjectsViewModel.FilterProjectsByClient(clientId); // Filter projects by client
            }
            else
            {
                // Clear the selection, show all projects
                _viewModel.SelectedClientID = null;
                ProjectsViewModel.FilterProjectsByClient(null); // Show all projects
            }
        }


        void OnTextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
        {
            var filteredList = new List<Client>(_viewModel.ClientList);

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                // Reset the CollectionView's ItemsSource to the original list
                MyCollectionViews.ItemsSource = _viewModel.ClientList;
            }
            else
            {
                // Filter the items based on the search text
                filteredList = filteredList
                    .Where(client => client.username.ToLower().Contains(e.NewTextValue.ToLower()))
                    .ToList();

                // If there's only one in the filtered list, add a blank card
                if (filteredList.Count == 1)
                {
                    var blankCard = new Client(); // Create a new instance with default or empty values
                    filteredList.Add(blankCard);
                }

                MyCollectionViews.ItemsSource = filteredList;
            }
        }

        public async Task LoadAllProjects()
        {
            await ProjectsViewModel.FetchAllProjects(); // Call the FetchAllProjects method from your ProjectsViewModel
        }



    }

}