using NovaApp.Models;
using NovaApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NovaApp.ViewModels
{
    public class StaffViewModel : BaseViewModel
    {
        private readonly RestService _restService;

        public ObservableCollection<Staff> StaffList { get; set; }

        // Adding Staff Properties
        public string ProfileImage { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string StaffType { get; set; }
        public string Position { get; set; }
        public string Password { get; set; }
        public int AvailableHours { get; set; } // Max hours an employee can work

        public ICommand AddNewStaffCommand { get; }

        public StaffViewModel(RestService restService)
        {
            _restService = restService;
            StaffList = new ObservableCollection<Staff>();

            AddNewStaffCommand = new Command(async () => await AddStaff());
        }

        private async Task AddStaff()
        {
            int maxAvailableHours = 40; // Assuming 40 hours is the max for a week

            if (StaffType == "Admin")
            {
                Position = "Admin"; // Set title to "Admin" for admin employees
    
            }
            else
            {
                // For non-admin employees, Titles is already set based on user input
                Password = null; // Non-admin employees do not receive a password
            }

            var newStaff = new Staff
            {
                ProfileImage = ProfileImage,
                Name = Name,
                Email = Email,
                StaffType = StaffType,
                Position = Position,
                Password = Password,
                AvailableHours = maxAvailableHours
            };

            await _restService.SaveStaffAsync(newStaff, true);

            // Refresh the list of staff members after adding
            await FetchAllStaff();

            // Clear input fields
            ProfileImage = string.Empty;
            Name = string.Empty;
            Email = string.Empty;
            StaffType = string.Empty;
            Position = string.Empty;
            Password = string.Empty;
            AvailableHours = 0;
        }

        private async Task FetchAllStaff()
        {
            var staffMembers = await _restService.RefreshStaffAsync();
            StaffList.Clear();
            foreach (var staff in staffMembers)
            {
                StaffList.Add(staff);
                Debug.WriteLine(staff.Name);
            }
        }
    }
}
