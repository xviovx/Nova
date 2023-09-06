using Mopups.Services;
using NovaApp.Models;
using NovaApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
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
        public ICommand UpdateClientCommand { get; }
        public ICommand DeactivateClientCommand { get; }

        public string CompanyNameError { get; private set; }
        public bool IsCompanyNameInvalid { get; private set; }

        public string EmailError { get; private set; }
        public bool IsEmailInvalid { get; private set; }

        public string ClientTypeError { get; private set; }
        public bool IsClientTypeInvalid { get; private set; }

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

        //Clientcount
            public int _numberOfClients;
            public int NumberOfClients
        {
            get { return _numberOfClients; }
            set
            {
                if (_numberOfClients != value)
                {
                    _numberOfClients = value;
                    OnPropertyChanged(nameof(NumberOfClients));
                }
            }
        }

        public ClientViewModel(RestService restService)
        {
            _restService = restService;
            ClientList = new ObservableCollection<Client>();
            NumberOfClients = ClientList.Count;


            AddNewClientCommand = new Command(async () => await AddClient());
            UpdateClientCommand = new Command<string>(async (clientId) =>
            {
                // Use the staffId parameter in your update logic
                Debug.WriteLine(clientId);
                await UpdateClient(clientId);
            });
            DeactivateClientCommand = new Command<string>(async (clientId) =>
            {
                // Use the staffId parameter in your update logic
                Debug.WriteLine(clientId);
                await DeactivateClient(clientId);
            });
        }

        private bool ValidateInput()
        {
            bool isValid = true;

            // Validate Company Name
            if (string.IsNullOrWhiteSpace(CompanyName))
            {
                CompanyNameError = "Company Name is required.";
                IsCompanyNameInvalid = true;
                isValid = false;
            }
            else
            {
                CompanyNameError = string.Empty;
                IsCompanyNameInvalid = false;
            }

            // Validate Email
            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailError = "Email is required.";
                IsEmailInvalid = true;
                isValid = false;
            }
            else if (!IsValidEmail(Email)) // Use IsValidEmail method for email validation
            {
                EmailError = "Invalid email format.";
                IsEmailInvalid = true;
                isValid = false;
            }
            else
            {
                EmailError = string.Empty;
                IsEmailInvalid = false;
            }

            // Validate Client Type
            if (!IsStandardType && !IsPriorityType)
            {
                ClientTypeError = "Client Type is required.";
                Debug.WriteLine(ClientTypeError);
                IsClientTypeInvalid = true;
                isValid = false;
            }
            else
            {
                ClientTypeError = string.Empty;
                IsClientTypeInvalid = false;
            }

            // Notify property changes for error messages and visibility flags
            OnPropertyChanged(nameof(CompanyNameError));
            OnPropertyChanged(nameof(IsCompanyNameInvalid));
            OnPropertyChanged(nameof(EmailError));
            OnPropertyChanged(nameof(IsEmailInvalid));
            OnPropertyChanged(nameof(ClientTypeError));
            OnPropertyChanged(nameof(IsClientTypeInvalid));

            return isValid;
        }
        // Email validation using regular expression
        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
            return Regex.IsMatch(email, emailPattern);
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
                clientsType = clientType,
                availableHours = availableHours,
                active = true
            };


            try
            {
                // Attempt to save the client
                await _restService.SaveClientAsync(newClient, true);
                MopupService.Instance.PopAllAsync();


                // Add the new client to the ObservableCollection immediately
                ClientList.Add(newClient);

                // Clear input fields and radio button selections
                CompanyName = string.Empty;
                Email = string.Empty;
                IsStandardType = false;
                IsPriorityType = false;
                AvailableHours = 0;
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or display an error message as needed
                Debug.WriteLine($"Error adding staff: {ex.Message}");
                ValidateInput();

            }


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
                NumberOfClients = ClientList.Count;
                Debug.WriteLine($"Number of staff fetched: {ClientList.Count}");

            }
        }

        public async Task UpdateClient(string clientId)
        {
            Debug.WriteLine("UpdateClient method started...", clientId);

            // Validation logic
            bool isValid = ValidateInput();

            if (SelectedClient == null)
            {
                // Handle the case when no client is selected for update.
                Debug.WriteLine("No client selected for update.");
                return;
            }

            // Continue with the update only if the input is valid
            if (isValid)
            {
                Debug.WriteLine($"Selected client ID: {SelectedClient.id}");

                // Prepare the updated data
                Client updatedClient = new Client
                {
                    id = SelectedClient.id, // Make sure to include the ID of the existing client
                    username = CompanyName,
                    email = Email,
                    clientsType = IsStandardType ? "Standard" : "Priority",
                    availableHours = IsStandardType ? 15 : 30, // Set available hours based on type
                    active = true
                    // Add other fields as needed
                };

                Debug.WriteLine("Updated client data prepared.");

                try
                {
                    // Update the selected client with a PATCH request
                    Debug.WriteLine("Updating client...");
                    await _restService.UpdateClientAsync(updatedClient);
                    Debug.WriteLine("Client updated successfully.");
                    MopupService.Instance.PopAllAsync();
                }
                catch (Exception ex)
                {
                    // Handle the exception, log it, or display an error message as needed
                    Debug.WriteLine($"Error updating client: {ex.Message}");
                }

                // Refresh the list of clients after updating
                Debug.WriteLine("Fetching all clients...");
                await FetchAllClients();
                Debug.WriteLine("Clients fetched successfully.");

                // Clear input fields, radio button selections, and other properties
                CompanyName = string.Empty;
                Email = string.Empty;
                IsStandardType = false;
                IsPriorityType = false;
                AvailableHours = 0;
            }
            else
            {
                // Validation failed, do not proceed with the update
                Debug.WriteLine("Validation failed. Client not updated.");
            }

            Debug.WriteLine("UpdateClient method completed.");
        }



        public async Task DeactivateClient(string clientId)
        {
            Debug.WriteLine("DeactivateClient method started...", clientId);

            // Validation logic
            bool isValid = ValidateInput();

            if (!isValid)
            {
                // Validation failed, display error messages
                return;
            }

            if (SelectedClient == null)
            {
                // Handle the case when no client is selected for deactivation.
                Debug.WriteLine("No client selected for deactivation.");
                return;
            }

            // Determine the new value for the 'active' property
            bool newActiveValue = SelectedClient.active.HasValue ? !SelectedClient.active.Value : true; // Toggle if not null, default to true otherwise
                                                                                                        // Prepare the updated data
            Client updatedClient = new Client
            {
                id = SelectedClient.id, // Make sure to include the ID of the existing client
                username = CompanyName,
                email = Email,
                clientsType = IsStandardType ? "Standard" : "Priority",
                availableHours = IsStandardType ? 15 : 30, // Set available hours based on type
                active = newActiveValue // Set the new value of the 'active' property
                                        // Add other fields as needed
            };

            try
            {
                // Update the selected client with a PATCH request
                await _restService.UpdateClientAsync(updatedClient);
                MopupService.Instance.PopAllAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or display an error message as needed
                Debug.WriteLine($"Error deactivating client: {ex.Message}");
                // Validation logic
                ValidateInput();
            }

            // Refresh the list of clients after updating
            await FetchAllClients();

            // Clear input fields, radio button selections, and other properties
            CompanyName = string.Empty;
            Email = string.Empty;
            IsStandardType = false;
            IsPriorityType = false;
            AvailableHours = 0;
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