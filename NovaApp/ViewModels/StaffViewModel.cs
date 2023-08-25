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
                                                // Boolean properties for radio button selection
        public bool IsEmployeeType { get; set; }
        public bool IsAdminType { get; set; }

        public ICommand AddNewStaffCommand { get; }

        public StaffViewModel(RestService restService)
        {
            _restService = restService;
            StaffList = new ObservableCollection<Staff>();

            AddNewStaffCommand = new Command(async () => await AddStaff());
        }

        public async Task AddStaff()
        {
            int maxAvailableHours = 40; // Assuming 40 hours is the max for a week

            string staffType;
            string position;
            int salary;

            if (IsEmployeeType)
            {
                staffType = "Employee";
                position = Position; // Use the inputted position for employees

                if (position == "Junior Developer")
                {
                    salary = 400; // R400 per hour for Junior Developer
                }
                else if (position == "Senior Developer")
                {
                    salary = 600; // R600 per hour for Senior Developer
                }
                else
                {
                    // Handle other positions if needed
                    salary = 0;
                }
            }
            else if (IsAdminType)
            {
                staffType = "Administrative Staff";
                position = "Admin"; // Automatically set position to "Admin" for admin staff
                salary = 17000; // R17000 per month for Administrative Staff
            }
            else
            {
                // Handle case when neither radio button is selected
                staffType = string.Empty;
                position = string.Empty;
                salary = 0;
            }

            // Set password based on staff type
            string password = IsAdminType ? Password : string.Empty;

            var newStaff = new Staff
            {
                ProfileImage = ProfileImage,
                Name = Name,
                Email = Email,
                StaffType = staffType,
                Position = position, // Set position here
                Salary = salary, // Set salary here
                Password = password, // Set password here
                AvailableHours = maxAvailableHours
            };

            await _restService.SaveStaffAsync(newStaff, true);

            // Refresh the list of staff members after adding
            await FetchAllStaff();

            // Clear input fields, radio button selections, and other properties
            ProfileImage = string.Empty;
            Name = string.Empty;
            Email = string.Empty;
            IsEmployeeType = false;
            IsAdminType = false;
            Position = string.Empty;
            Password = string.Empty;
            AvailableHours = 0;
        }


        public async Task FetchAllStaff()
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
