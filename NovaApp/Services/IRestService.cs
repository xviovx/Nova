using NovaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaApp.Services
{
    public interface IRestService
    {

        //Define all of our REST method

        //Clients
        Task<List<Client>> RefreshClientsAsync(); //GET all clients
        Task SaveClientAsync(Client item, bool isNewItem = false);//POST/PUT clients
        Task<Client> GetClientByIdAsync(string clientId); //GET client by Id

        //Staff
        Task<List<Staff>> RefreshStaffAsync(); //GET all staff
        Task SaveStaffAsync(Staff item, bool isNewItem = false);//POST/PUT staff
        Task<Staff> GetStaffByIdAsync(string staffId); //GET staff by Id
        Task UpdateStaffAsync(Staff item);

    }
}