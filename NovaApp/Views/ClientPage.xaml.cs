using Mopups.Services;
using NovaApp.Popups;
using NovaApp.ViewModels;

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
            MopupService.Instance.PushAsync(new UpdateClient());
        }
    }
}
