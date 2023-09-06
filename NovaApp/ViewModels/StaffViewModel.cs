using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Documents;
using Mopups.Services;
using NovaApp.Models;
using NovaApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Networking;
using Windows.System;

namespace NovaApp.ViewModels
{
    public class StaffViewModel : BaseViewModel
    {
        private readonly RestService _restService;

        public ObservableCollection<Staff> StaffList { get; set; }

        // Adding Staff Properties
        public int ProfileImage { get; set; }
        public string SelectedImageSource { get; set; } // Changed from private to public
        private int _selectedImageIndex;
        public int SelectedImageIndex
        {
            get => _selectedImageIndex;
            set
            {
                if (_selectedImageIndex != value)
                {
                    _selectedImageIndex = value;
                    OnPropertyChanged(nameof(SelectedImageIndex));
                }
            }
        }

        public string Username { get; set; }
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
        public string salary { get; set; }

        public ICommand AddNewStaffCommand { get; }
        public ICommand DeactivateStaffCommand { get; }

        private string _staffId;
        public string StaffId
        {
            get => _staffId;
            set
            {
                _staffId = value;
                OnPropertyChanged(nameof(StaffId));
            }
        }

        public ICommand UpdateStaffCommand { get; private set; }

        private string _profileImageUrl;

        public string ProfileImageUrl
        {
            get { return _profileImageUrl; }
            set
            {
                if (_profileImageUrl != value)
                {
                    _profileImageUrl = value;
                    OnPropertyChanged(nameof(ProfileImageUrl)); // Implement INotifyPropertyChanged
                }
            }
        }
        private Staff _selectedStaff;
        public Staff SelectedStaff
        {
            get { return _selectedStaff; }
            set
            {
                _selectedStaff = value;
                OnPropertyChanged(nameof(SelectedStaff));
                OnPropertyChanged(nameof(SelectedStaff.payPerHour));
            }
        }


        private Staff _fetchedStaffData;

        public Staff FetchedStaffData
        {
            get { return _fetchedStaffData; }
            set
            {
                _fetchedStaffData = value;
                OnPropertyChanged(nameof(FetchedStaffData));
            }
        }

        private bool isAddStaffSuccessful;

        public bool IsAddStaffSuccessful
        {
            get { return isAddStaffSuccessful; }
            set
            {
                isAddStaffSuccessful = true;
                OnPropertyChanged(nameof(IsAddStaffSuccessful));
            }
        }


        // Error properties and visibility flags
        public string FirstNameError { get; private set; }
        public bool IsFirstNameInvalid { get; private set; }


        public string LastNameError { get; private set; }
        public bool IsLastNameInvalid { get; private set; }

        public string EmailError { get; private set; }
        public bool IsEmailInvalid { get; private set; }

        public string PasswordError { get; private set; }
        public bool IsPasswordInvalid { get; private set; }

        public string PositionError { get; private set; }
        public bool IsPositionInvalid { get; private set; }

        public string RoleGroupError { get; private set; }
        public bool IsRoleGroupInvalid { get; private set; }

        public string ProfileImageError { get; private set; }
        public bool IsProfileImageInvalid { get; private set; }


        // ObservableCollection to store position options for the Picker
        public ObservableCollection<string> PositionOptions { get; }

        public StaffViewModel(RestService restService)
        {
            _restService = restService;
            StaffList = new ObservableCollection<Staff>();
            NumberOfStaff = StaffList.Count;


            // Initialize PositionOptions
            PositionOptions = new ObservableCollection<string>
                {
                    "Senior Developer",
                    "Junior Developer"
                };

            // Set the default position to admin
            Position = "Admin";

            AddNewStaffCommand = new Command(async () => await AddStaff());
            UpdateStaffCommand = new Command<string>(async (staffId) =>
            {
                // Use the staffId parameter in your update logic
                Debug.WriteLine(staffId);
                await UpdateStaff(staffId);
            });
            DeactivateStaffCommand = new Command<string>(async (staffId) =>
            {
                // Use the staffId parameter in your update logic
                Debug.WriteLine(staffId);
                await DeactivateStaff(staffId);
            });
        }

        //staffcount
        public int _numberOfStaff;
        public int NumberOfStaff
        {
            get { return _numberOfStaff; }
            set
            {
                if (_numberOfStaff != value)
                {
                    _numberOfStaff = value;
                    OnPropertyChanged(nameof(NumberOfStaff));
                }
            }
        }

        private bool ValidateInput()
        {
            bool isValid = true;

            // Validate FirstName
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                FirstNameError = "First Name is required.";
                IsFirstNameInvalid = true;
                isValid = false;
            }
            else
            {
                FirstNameError = string.Empty;
                IsFirstNameInvalid = false;
            }

            // Validate LastName
            if (string.IsNullOrWhiteSpace(LastName))
            {
                LastNameError = "Last Name is required.";
                IsLastNameInvalid = true;
                isValid = false;
            }
            else
            {
                LastNameError = string.Empty;
                IsLastNameInvalid = false;
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

            // Validate Password
            if (IsAdminType && string.IsNullOrWhiteSpace(Password))
            {
                PasswordError = "Password is required for administrative staff.";
                Debug.WriteLine(PasswordError);
                IsPasswordInvalid = true;
                isValid = false;
            }
            else if (IsAdminType && !IsValidPassword(Password)) // Use IsValidPassword method for password validation
            {
                PasswordError = "Password must contain 8 characters.";
                IsPasswordInvalid = true;
                isValid = false;
            }
            else
            {
                PasswordError = string.Empty;
                IsPasswordInvalid = false;
            }

            // Validate Position
            if (IsEmployeeType && Position == null)
            {
                PositionError = "Position is required for employees.";
                IsPositionInvalid = true;
                isValid = false;
            }
            else
            {
                PositionError = string.Empty;
                IsPositionInvalid = false;
            }

            // Validate Role Type
            if (!IsEmployeeType && !IsAdminType)
            {
                RoleGroupError = "Role Type is required.";
                IsRoleGroupInvalid = true;
                isValid = false;
            }
            else
            {
                RoleGroupError = string.Empty;
                IsRoleGroupInvalid = false;
            }

            // Validate Profile Image
            if (SelectedImageIndex == 0)
            {
                ProfileImageError = "Profile Image is required.";
                IsProfileImageInvalid = true;
                isValid = false;
            }
            else
            {
                ProfileImageError = string.Empty;
                IsProfileImageInvalid = false;
            }

            // Notify property changes for error messages and visibility flags
            OnPropertyChanged(nameof(FirstNameError));
            OnPropertyChanged(nameof(IsFirstNameInvalid));
            OnPropertyChanged(nameof(LastNameError));
            OnPropertyChanged(nameof(IsLastNameInvalid));
            OnPropertyChanged(nameof(EmailError));
            OnPropertyChanged(nameof(IsEmailInvalid));
            OnPropertyChanged(nameof(RoleGroupError));
            OnPropertyChanged(nameof(IsRoleGroupInvalid));
            OnPropertyChanged(nameof(ProfileImageError));
            OnPropertyChanged(nameof(IsProfileImageInvalid));
            OnPropertyChanged(nameof(PositionError));
            OnPropertyChanged(nameof(IsPositionInvalid));
            OnPropertyChanged(nameof(PasswordError));
            OnPropertyChanged(nameof(IsPasswordInvalid));

            return isValid;
        }

        // Email validation using regular expression
        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
            return Regex.IsMatch(email, emailPattern);
        }

        // Password validation method
        private bool IsValidPassword(string password)
        {
            // Example: Password must be at least 8 characters and contain at least one uppercase letter, one lowercase letter, and one digit.
            string passwordPattern = @"^.{8}$";
            return Regex.IsMatch(password, passwordPattern);
        }




        public async Task AddStaff()
        {
            Debug.WriteLine("AddStaff method started...");
            int maxAvailableHours = 40; // Assuming 40 hours is the max for a week

            string staffType;
            int salary;

            // Validation logic
            bool isValid = ValidateInput();

            if (!isValid)
            {
                // Validation failed, display error messages
                return;
            }

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
            string password = IsAdminType ? Password : "Invalid User";

            // Concatenate FirstName and LastName for the Name property
            string fullName = $"{FirstName} {LastName}";

            // Ensure SelectedImageIndex is an integer
            int selectedImageInt = SelectedImageIndex;

            Debug.WriteLine($"Creating newStaff object with: profileImage={selectedImageInt}, Name={fullName}, Email={Email}, Position={Position}, StaffType={staffType}, Salary={salary}, Password={password}");

            var newStaff = new Staff
            {
                profileImage = selectedImageInt,
                username = fullName,
                email = Email,
                staffType = staffType,
                position = Position,
                payPerHour = salary,
                password = password,
                availableHours = maxAvailableHours,
                active = true
            };

            try
            {
                // Attempt to save the staff member
                await _restService.SaveStaffAsync(newStaff, true);
                MopupService.Instance.PopAllAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or display an error message as needed
                Debug.WriteLine($"Error adding staff: {ex.Message}");
                ValidateInput();

            }

            // Refresh the list of staff members after adding
            await FetchAllStaff();

            // Clear input fields, radio button selections, and other properties
            ProfileImage = 0;
            FirstName = string.Empty; // Clear the FirstName
            LastName = string.Empty;  // Clear the LastName
            Email = string.Empty;
            IsEmployeeType = false;
            IsAdminType = false;
            Position = string.Empty;
            Password = string.Empty;
            AvailableHours = 0;


            // Clear input fields, radio button selections, and other properties
            ProfileImage = 0;
            FirstName = string.Empty; // Clear the FirstName
            LastName = string.Empty;  // Clear the LastName
            Email = string.Empty;
            IsEmployeeType = false;
            IsAdminType = false;
            Position = string.Empty;
            Password = string.Empty;
            AvailableHours = 0;
        }

        public async Task UpdateStaff(string staffId)
        {
            Debug.WriteLine("UpdateStaff method started...", staffId);

            // Validation logic
            bool isValid = ValidateInput();

            if (!isValid)
            {
                // Validation failed, display error messages
                return;
            }

            if (_selectedStaff == null)
            {
                // Handle the case when no staff member is selected for update.
                Debug.WriteLine("No staff member selected for update.");
                return;
            }

            // Prepare the updated data
            Staff updatedStaff = new Staff
            {
                id = _selectedStaff.id, // Make sure to include the ID of the existing staff member
                profileImage = SelectedImageIndex,
                username = $"{FirstName} {LastName}",
                email = Email,
                staffType = IsEmployeeType ? "Employee" : "Administrative Staff",
                position = IsEmployeeType ? Position : "admin", // Only set position for employees
                payPerHour = IsEmployeeType ? (Position == "Junior Developer" ? 400 : 600) : 17000, // Set pay based on type and position
                password = IsAdminType ? Password : "Invalid User",
                availableHours = 40, // Assuming 40 hours is the max for a week
                active = true
                // Add other fields as needed
            };


            try
            {
                // Update the selected staff member with a PATCH request
                await _restService.UpdateStaffAsync(updatedStaff);
                MopupService.Instance.PopAllAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or display an error message as needed
                Debug.WriteLine($"Error updating staff: {ex.Message}");
                // Validation logic
                ValidateInput();

            }

            // Refresh the list of staff members after updating
            await FetchAllStaff();

            // Clear input fields, radio button selections, and other properties
            ProfileImage = 0;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            IsEmployeeType = false;
            IsAdminType = false;
            Position = string.Empty;
            Password = string.Empty;
            AvailableHours = 0;
        }


        public async Task DeactivateStaff(string staffId)
        {
            Debug.WriteLine("DeactivateStaff method started...", staffId);

            // Validation logic
            bool isValid = ValidateInput();

            if (!isValid)
            {
                // Validation failed, display error messages
                return;
            }

            if (_selectedStaff == null)
            {
                // Handle the case when no staff member is selected for update.
                Debug.WriteLine("No staff member selected for Deactivate.");
                return;
            }

            // Determine the new value for the 'active' property
            bool newActiveValue = _selectedStaff.active.HasValue ? !_selectedStaff.active.Value : true; // Toggle if not null, default to true otherwise
            // Prepare the updated data
            Staff updatedStaff = new Staff
            {
                id = _selectedStaff.id, // Make sure to include the ID of the existing staff member
                profileImage = SelectedImageIndex,
                username = $"{FirstName} {LastName}",
                email = Email,
                staffType = IsEmployeeType ? "Employee" : "Administrative Staff",
                position = IsEmployeeType ? Position : "admin", // Only set position for employees
                payPerHour = IsEmployeeType ? (Position == "Junior Developer" ? 400 : 600) : 17000, // Set pay based on type and position
                password = IsAdminType ? Password : "Invalid User",
                availableHours = 40, // Assuming 40 hours is the max for a week
                active = newActiveValue // Set the new value of the 'active' property
                                        // Add other fields as needed
            };

            try
            {
                // Update the selected staff member with a PATCH request
                await _restService.UpdateStaffAsync(updatedStaff);
                MopupService.Instance.PopAllAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or display an error message as needed
                Debug.WriteLine($"Error DeactivateStaff staff: {ex.Message}");
                // Validation logic
                ValidateInput();
            }

            // Refresh the list of staff members after updating
            await FetchAllStaff();

            // Clear input fields, radio button selections, and other properties
            ProfileImage = 0;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            IsEmployeeType = false;
            IsAdminType = false;
            Position = string.Empty;
            Password = string.Empty;
            AvailableHours = 0;
        }



        public async Task FetchAllStaff()
        {
            try
            {
                var staffMembers = await _restService.RefreshStaffAsync();
                Debug.WriteLine("Running fetch staff");

                // Log the count of staff members fetched
                Debug.WriteLine($"Fetched {staffMembers.Count} staff members");

                StaffList.Clear();

                foreach (var staff in staffMembers)
                {
                    StaffList.Add(staff);
                    Debug.WriteLine($"Fetched staff: {staff.username}, {staff.payPerHour}");

                    // Set the selected staff to the first staff member fetched
                    if (StaffList.Count == 1)
                    {
                        SelectedStaff = staff;
                    }
                    NumberOfStaff = StaffList.Count;
                    Debug.WriteLine($"Number of staff fetched: {StaffList.Count}");

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.ToString()); // Log any exceptions that occur
            }
        }

        public async Task FetchStaffById(string staffId)
        {
            try
            {
                var staffMember = await _restService.GetStaffByIdAsync(staffId);
                Debug.WriteLine("Running fetch staff by ID");

                if (staffMember != null)
                {
                    SelectedStaff = staffMember;
                }
                else
                {
                    // Handle the case when no staff member with the specified ID was found
                    Debug.WriteLine("Staff member not found.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.ToString()); // Log any exceptions that occur
            }
        }

        public async Task GetStaffById(string staffId)
        {
            try
            {
                var staff = await _restService.GetStaffByIdAsync(staffId);

                // var staf tasks 

                // get tasks

                Debug.WriteLine("Running fetch staff by ID");

                if (staff != null)
                {
                    FetchedStaffData = staff; // Store the fetched staff data here
                }
                else
                {
                    // Handle the case when no staff with the specified ID was found
                    Debug.WriteLine("Staff not found.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.ToString()); // Log any exceptions that occur
            }
        }





    }
}