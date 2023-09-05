using Mopups.Services;
using NovaApp.Models;
using NovaApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NovaApp.ViewModels
{
    public class ProjectsViewModel : BaseViewModel
    {
        public ProjectsRestService _projectsRestService { get; set; }
        public RestService _restService { get; set; }

        public ObservableCollection<Project> ProjectsList { get; set; }

        public ObservableCollection<ClientOwner> ClientList { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DeadlineDate { get; set; }
        public ClientOwner ClientOwner { get; set; }
        public ICommand AddNewProjectCommand { get; }

        public string TitleError { get; private set; }
        public bool IsTitleInvalid { get; private set; }

  
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

        private ClientOwner _selectedClientOwner;
        public ClientOwner SelectedClientOwner
        {
            get { return _selectedClientOwner; }
            set
            {
                _selectedClientOwner = value;
                OnPropertyChanged(nameof(SelectedClientOwner));
            }
        }


        public ProjectsViewModel(ProjectsRestService projectsRestService)
        {
            _projectsRestService = projectsRestService;
            ProjectsList = new ObservableCollection<Project>();
            ClientList = new ObservableCollection<ClientOwner>();

            AddNewProjectCommand = new Command(async () => await AddProject());
        }

        public async Task FetchAllProjects()
        {
            var items = await _projectsRestService.RefreshProjectsListAsync();
            ProjectsList.Clear();
            foreach (var item in items)
            {
                DateTime dateToConvert = item.deadlineDate;
                var FormattedDate = dateToConvert.ToString("MMMM dd, yyyy");
                item.deadlineDateString = FormattedDate;
                ProjectsList.Add(item);
            }

            Debug.WriteLine($"Number of projects fetched: {ProjectsList.Count}");
        }

        private bool ValidateInput()
        {
            bool isValid = true;

            // Validate Title
            if (string.IsNullOrWhiteSpace(Title))
            {
                TitleError = "Project Title is required.";
                IsTitleInvalid = true;
                isValid = false;
            }
            else
            {
                TitleError = string.Empty;
                IsTitleInvalid = false;
            }

            // Add similar validation logic for Description, DeadlineDate, and ClientOwner here

            // Notify property changes for error messages and visibility flags
            OnPropertyChanged(nameof(TitleError));
            OnPropertyChanged(nameof(IsTitleInvalid));

          

            return isValid;
        }

        public async Task AddProject()
        {
            bool isValid = ValidateInput();

            if (!isValid)
            {
                // Validation failed, do not add the project
                Debug.WriteLine("Validation failed. Project not added.");
                return;
            }
            DateTime dateToConvert = DeadlineDate;
            var FormattedDate = dateToConvert.ToString("MMMM dd, yyyy");
           
            Debug.WriteLine("Client Owner" + FormattedDate);
            // Assuming SelectedClient is of type Client and SelectedClientOwner is of type ClientOwner

            // Create a new ClientOwner object and populate its properties
            var newClientOwner = new ClientOwner
            {
                id = SelectedClient.id,
                username = SelectedClient.username
            };

            Debug.WriteLine($"Selected Client viewmodel: {SelectedClient.username}, ID: {SelectedClient.id}, ...");
            var newProject = new Project
            {
                title = Title,
                //description = Description,
                deadlineDateString = FormattedDate,
                deadlineDate = DeadlineDate,
                clientOwner = newClientOwner,
                status = "In Progress",
                
            };

            try
            {
                // Attempt to save the project
                Debug.WriteLine("Attempting to save the project...");
                await _projectsRestService.SaveProjectAsync(newProject);
                Debug.WriteLine("Project saved successfully.");
                MopupService.Instance.PopAllAsync();

                // Clear input fields
                Title = string.Empty;
                Description = string.Empty;
                DeadlineDate = DateTime.Now; // Reset to the default date
                ClientOwner = null; // Clear the selected client owner
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or display an error message as needed
                Debug.WriteLine($"Error adding project: {ex.Message}");
                ValidateInput();
            }
        }




    }

}
