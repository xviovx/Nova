using NovaApp.Models;
using NovaApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Networking;

namespace NovaApp.ViewModels
{
    public class StaffViewModel : BaseViewModel
    {
        private readonly RestService _restService;

        public ObservableCollection<Staff> StaffList { get; set; }

        // Adding Staff Properties
        public string ProfileImage { get; set; }
        public string SelectedImageSource { get; set; } // Changed from private to public

        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string StaffType { get; set; }

        private string _position;
        public string Position
        {
            get { return _position; }
            set { SetProperty(ref _position, value); }
        }


        public string Password { get; set; }
        public int AvailableHours { get; set; } // Max hours an employee can work

        // Boolean properties for radio button selection
        public bool IsEmployeeType { get; set; }
        public bool IsAdminType { get; set; }

        public ICommand AddNewStaffCommand { get; }

        // ObservableCollection to store position options for the Picker
        public ObservableCollection<string> PositionOptions { get; }

        public StaffViewModel(RestService restService)
        {
            _restService = restService;
            StaffList = new ObservableCollection<Staff>();

            // Initialize PositionOptions
            PositionOptions = new ObservableCollection<string>
                {
                    "Senior Developer",
                    "Junior Developer"
                };

            // Set the default position to admin
            Position = "Admin";

            AddNewStaffCommand = new Command(async () => await AddStaff());
        }

        public async Task AddStaff()
        {
            Debug.WriteLine("AddStaff method started...");
            int maxAvailableHours = 40; // Assuming 40 hours is the max for a week

            string staffType;
            int salary;

            if (IsEmployeeType)
            {
                staffType = "Employee";
                if (string.IsNullOrEmpty(Position))
                {
                    Position = PositionOptions[0]; // Set default position if not selected
                }

                if (Position == "Junior Developer")
                {
                    salary = 400; // R400 per hour for Junior Developer
                }
                else if (Position == "Senior Developer")
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
                salary = 17000; // R17000 per month for Administrative Staff
       
            }
            else
            {
                // Handle case when neither radio button is selected
                staffType = string.Empty;
                Position = string.Empty;
                salary = 0;
            }

            // Set password based on staff type
            string password = IsAdminType ? Password : string.Empty;

            // Concatenate FirstName and LastName for the Name property
            string fullName = $"{FirstName} {LastName}";

            Debug.WriteLine($"Creating newStaff object with: ProfileImage={SelectedImageSource}, Name={fullName}, Email={Email}, Position={Position}, StaffType={staffType}, Salary={salary}, Password={password}");

            var newStaff = new Staff
            {
                ProfileImage = SelectedImageSource, // Use the SelectedImageSource directly
                Name = fullName,
                Email = Email,
                StaffType = staffType,
                Position = Position,
                Salary = salary,
                Password = password,
                AvailableHours = maxAvailableHours,
                Active = true
            };

            await _restService.SaveStaffAsync(newStaff, true);

            // Refresh the list of staff members after adding
            await FetchAllStaff();

            // Clear input fields, radio button selections, and other properties
            ProfileImage = string.Empty;
            FirstName = string.Empty; // Clear the FirstName
            LastName = string.Empty;  // Clear the LastName
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
