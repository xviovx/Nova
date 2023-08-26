﻿using NovaApp.Models;
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

        public ClientViewModel(RestService restService)
        {
            _restService = restService;
            ClientList = new ObservableCollection<Client>();

            AddNewClientCommand = new Command(async () => await AddClient());
        }

        public async Task AddClient()
        {
            int availableHours =0;
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

            var newClient = new Client
            {
                CompanyName = CompanyName,
                Email = Email,
                ClientType = clientType,
                AvailableHours = availableHours,
                Active = true
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
                Debug.WriteLine(client.CompanyName);
            }
        }
    }
}