using NovaApp.Models;
using NovaApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NovaApp.ViewModels
{
    public class ClientViewModel : BaseViewModel
    {
        private readonly RestService _restService;

        public ObservableCollection<Client> ClientList { get; set; }

        // Adding Client Properties
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string ClientType { get; set; }
        public int AvailableHours { get; set; }
        public ICommand AddNewClientCommand { get; }

        public ClientViewModel(RestService restService)
        {
            _restService = restService;
            ClientList = new ObservableCollection<Client>();

            AddNewClientCommand = new Command(async () => await AddClient());
        }

        private async Task AddClient()
        {
            int availableHours;

            if (ClientType == "Priority")
            {
                availableHours = 30;
            }
            else if (ClientType == "Standard")
            {
                availableHours = 15;
            }
            else
            {
                // Set a default value or handle the case where ClientType is neither "Priority" nor "Standard"
                availableHours = 0;
            }

            var newClient = new Client
            {
                CompanyName = CompanyName,
                Email = Email,
                ClientType = ClientType,
                AvailableHours = availableHours
            };

            await _restService.SaveClientAsync(newClient, true);

            // Refresh the list of clients after adding
            await FetchAllClients();

            // Clear input fields
            CompanyName = string.Empty;
            Email = string.Empty;
            ClientType = string.Empty;
            AvailableHours = 0;
        }

        private async Task FetchAllClients()
        {
            var clients = await _restService.RefreshClientsAsync();
            ClientList.Clear();
            foreach (var client in clients)
            {
                ClientList.Add(client);
                Debug.WriteLine(client.CompanyName);
            }
        }
    }
}
