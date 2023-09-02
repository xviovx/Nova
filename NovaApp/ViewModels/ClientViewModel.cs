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
        public bool IsStandardType { get; set; }
        public bool IsPriorityType { get; set; }
        public int AvailableHours { get; set; }
        public bool Active { get; set; }
        public ICommand AddNewClientCommand { get; }

        public string CompanyNameError { get; private set; }
        public bool IsCompanyNameInvalid { get; private set; }

        public string EmailError { get; private set; }
        public bool IsEmailInvalid { get; private set; }

        private Client _selectedClient;
        public Client SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                OnPropertyChanged(nameof(SelectedClient));
            }
        }

        private Client _fetchedClientData;

        public Client FetchedClientData
        {
            get { return _fetchedClientData; }
            set
            {
                _fetchedClientData = value;
                OnPropertyChanged(nameof(FetchedClientData));
            }
        }

        public ClientViewModel(RestService restService)
        {
            _restService = restService;
            ClientList = new ObservableCollection<Client>();

            AddNewClientCommand = new Command(async () => await AddClient());
        }

        private bool ValidateInput()
        {
            bool isValid = true;

            // Validate CompanyName
            CompanyNameError = ValidateField(CompanyName, "Company Name is required.");
            IsCompanyNameInvalid = !string.IsNullOrEmpty(CompanyNameError);

            // Validate Email
            EmailError = ValidateField(Email, "Email is required.");
            IsEmailInvalid = !string.IsNullOrEmpty(EmailError);

            isValid = isValid && !IsCompanyNameInvalid && !IsEmailInvalid;

            return isValid;
        }

        private string ValidateField(string fieldValue, string fieldName)
        {
            return string.IsNullOrWhiteSpace(fieldValue) ? $"{fieldName} is required." : string.Empty;
        }

        public async Task AddClient()
        {
            int availableHours = 0;
            string clientType;

            if (IsStandardType)
            {
                clientType = "Standard";
                availableHours = 15;
            }
            else if (IsPriorityType)
            {
                clientType = "Priority";
                availableHours = 30;
            }
            else
            {
                // Handle case when neither radio button is selected
                clientType = string.Empty;
            }

            Debug.WriteLine($"Adding new client with: CompanyName={CompanyName}, Email={Email}, ClientType={clientType}, AvailableHours={availableHours}");

            bool isValid = ValidateInput();

            if (!isValid)
            {
                // Validation failed, do not add the client
                return;
            }

            var newClient = new Client
            {
                username = CompanyName,
                email = Email,
                clientType = clientType,
                availableHours = availableHours,
                active = true
            };

            await _restService.SaveClientAsync(newClient, true);

            // Refresh the list of clients after adding
            await FetchAllClients();

            // Clear input fields and radio button selections
            CompanyName = string.Empty;
            Email = string.Empty;
            IsStandardType = false;
            IsPriorityType = false;
            AvailableHours = 0;
        }

        public async Task FetchAllClients()
        {
            var clients = await _restService.RefreshClientsAsync();
            ClientList.Clear();
            foreach (var client in clients)
            {
                ClientList.Add(client);
                Debug.WriteLine(client.username);


                // Set the selected staff to the first staff member fetched
                if (ClientList.Count == 1)
                {
                    SelectedClient = client;
                }
            }
        }


        public async Task FetchClientById(string clientId)
        {
            try
            {
                var client = await _restService.GetClientByIdAsync(clientId);
                Debug.WriteLine("Running fetch staff by ID");

                if (client != null)
                {
                    SelectedClient = client;
                }
                else
                {
                    // Handle the case when no staff member with the specified ID was found
                    Debug.WriteLine("Client not found.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.ToString()); // Log any exceptions that occur
            }
        }



        public async Task GetClientById(string clientId)
        {
            try
            {
                var client = await _restService.GetClientByIdAsync(clientId);
                Debug.WriteLine("Running fetch client by ID");

                if (client != null)
                {
                    FetchedClientData = client; // Store the fetched client data here
                }
                else
                {
                    // Handle the case when no client with the specified ID was found
                    Debug.WriteLine("Client not found.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.ToString()); // Log any exceptions that occur
            }
        }




    }
}
