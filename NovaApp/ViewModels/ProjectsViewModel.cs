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

        public ObservableCollection<TaskDisplay> TaskList { get; set; }

        public ObservableCollection<ClientOwner> ClientList { get; set; }

        private Project _selectedProject;
        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                if (_selectedProject != value)
                {
                    _selectedProject = value;
                    OnPropertyChanged(nameof(SelectedProject));
                }
            }
        }

        public int _numberOfProjects;
        public int NumberOfProjects
        {
            get { return _numberOfProjects; }
            set
            {
                if (_numberOfProjects != value)
                {
                    _numberOfProjects = value;
                    OnPropertyChanged(nameof(NumberOfProjects));
                }
            }
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string BaseCost { get; set; }
        public DateTime DeadlineDate { get; set; }
        public ClientOwner ClientOwner { get; set; }
        public ICommand AddNewProjectCommand { get; }
        public ICommand UpdateProjectCommand { get; }

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


        public ProjectsViewModel(ProjectsRestService projectsRestService)
        {
            _projectsRestService = projectsRestService;

            ProjectsList = new ObservableCollection<Project>();

            TaskList = new ObservableCollection<TaskDisplay>();

            SelectedProject = new Project();

            NumberOfProjects = ProjectsList.Count;

            AddNewProjectCommand = new Command(async () => await AddProject());
            UpdateProjectCommand = new Command<Project>(async (project) =>
            {
                Debug.WriteLine(project.id);
                await UpdateProject(project.id);
            });
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
                int totalJobs = item.jobs.Count;
                int completedJobs = item.jobs.Count(job => job.status);
                int progress = totalJobs == 0 ? 0 : completedJobs * 100 / totalJobs;
                item.progress = progress;
                ProjectsList.Add(item);
            }
            NumberOfProjects = ProjectsList.Count;

            Debug.WriteLine($"Number of projects fetched: {ProjectsList.Count}");
        }

        public async Task FetchProject(string ProjectId)
        {
            var Project = await _projectsRestService.GetProject(ProjectId);

            DateTime dateToConvert = Project.deadlineDate;
            var FormattedDate = dateToConvert.ToString("MMMM dd, yyyy");
            Project.deadlineDateString = FormattedDate;

            int totalJobs = Project.jobs.Count;
            int completedJobs = Project.jobs.Count(job => job.status);
            int progress = totalJobs == 0 ? 0 : completedJobs * 100 / totalJobs;

            Project.progress = progress;

            // funds set up
            var totalExpense = Project.funds.Sum(fund => fund.expenses);
            var totalIincome = Project.funds.Sum(fund => fund.income);


            Project.totalExpenses = totalExpense;
            Project.totalIncome = totalIincome;

            SelectedProject = Project;


            Debug.WriteLine("project title: ", SelectedProject.title);



            // setup task list

            var items = await _projectsRestService.GetTaskList(ProjectId);
            TaskList.Clear();
            foreach (var item in items)
            {
                TaskList.Add(item);
            }


        }

        private Project _fetchedProjectData;

        public Project FetchedProjectData
        {
            get { return _fetchedProjectData; }
            set
            {
                _fetchedProjectData = value;
                OnPropertyChanged(nameof(FetchedProjectData));
            }
        }

        public async Task FetchProjectById(string projectId)
        {
            try
            {
                var project = await _projectsRestService.GetProject(projectId);

                if (project != null)
                {
                    DateTime dateToConvert = project.deadlineDate;
                    var formattedDate = dateToConvert.ToString("MMMM dd, yyyy");
                    project.deadlineDateString = formattedDate;

                    int totalJobs = project.jobs.Count;
                    int completedJobs = project.jobs.Count(job => job.status);
                    int progress = totalJobs == 0 ? 0 : completedJobs * 100 / totalJobs;

                    project.progress = progress;
                    SelectedProject = project;

                    Debug.WriteLine("Project title: " + SelectedProject.title);

                    // setup task list
                    var items = await _projectsRestService.GetTaskList(projectId);
                    TaskList.Clear();
                    foreach (var item in items)
                    {
                        TaskList.Add(item);
                    }
                }
                else
                {
                    // Handle the case when no project with the specified ID was found
                    Debug.WriteLine("Project not found.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.ToString()); // Log any exceptions that occur
            }
        }


        public async Task GetProjectById(string projectId)
        {
            try
            {
                var project = await _projectsRestService.GetProject(projectId);
                Debug.WriteLine("Running fetch project by ID");

                if (project != null)
                {
                    // Store the fetched project data here
                    FetchedProjectData = project;
                }
                else
                {
                    // Handle the case when no project with the specified ID was found
                    Debug.WriteLine("Project not found.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.ToString()); // Log any exceptions that occur
            }
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
            Debug.WriteLine("Running");

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

            int baseCostValue;
            if (int.TryParse(BaseCost, out baseCostValue))
            {
                Debug.WriteLine(baseCostValue);
                Debug.WriteLine($"Selected Client viewmodel: {SelectedClient.username}, ID: {SelectedClient.id}, ...");
                var newProject = new createProjectDto
                {
                    title = Title,
                    description = Description,
                    deadlineDate = DeadlineDate,
                    clientOwner = SelectedClient.id,
                    baseCost = baseCostValue // Assign the parsed integer value
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
                    BaseCost = string.Empty; // Reset BaseCost to an empty string
                }
                catch (Exception ex)
                {
                    // Handle the exception, log it, or display an error message as needed
                    Debug.WriteLine($"Error adding project: {ex.Message}");
                    ValidateInput();
                }
            }
            else
            {
                // Handle the case where BaseCost cannot be parsed as an integer
                Debug.WriteLine("Invalid BaseCost format. Project not added.");
            }
        }

        public async Task ChangeTaskStatus(string taskId)
        {
            await _projectsRestService.ChangeTaskStatus(taskId);
        }


        public async Task UpdateProject(string projectId)
        {
            Debug.WriteLine("Running UpdateProject");

            bool isValid = ValidateInput(); // You may want to create a separate ValidateInput method for update validation.

            if (!isValid)
            {
                // Validation failed, do not update the project
                Debug.WriteLine("Validation failed. Project not updated.");
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

            int baseCostValue;
            if (int.TryParse(BaseCost, out baseCostValue))
            {
                Debug.WriteLine(baseCostValue);
                Debug.WriteLine($"Selected Client viewmodel: {SelectedClient.username}, ID: {SelectedClient.id}, ...");

                // Create a new createProjectDto with the updated data
                var updatedProject = new createProjectDto
                {
                    title = Title,
                    description = Description,
                    deadlineDate = DeadlineDate, // Assuming DeadlineDate is a DateTime
                    clientOwner = SelectedClient.id,
                    baseCost = baseCostValue
                };

                try
                {
                    // Update the project identified by projectId
                    Debug.WriteLine("Attempting to update the project..." + projectId);
                    await _projectsRestService.UpdateProjectAsync(projectId, updatedProject);
                    Debug.WriteLine("Project updated successfully.");
                    MopupService.Instance.PopAllAsync();

                    // Clear input fields or reset as needed
                    Title = string.Empty;
                    Description = string.Empty;
                    DeadlineDate = DateTime.Now; // Reset to the default date
                    SelectedClient = null; // Clear the selected client
                    BaseCost = string.Empty; // Reset BaseCost to an empty string
                }
                catch (Exception ex)
                {
                    // Handle the exception, log it, or display an error message as needed
                    Debug.WriteLine($"Error updating project: {ex.Message}");
                    ValidateInput();
                }
            }
            else
            {
                // Handle the case where BaseCost cannot be parsed as an integer
                Debug.WriteLine("Invalid BaseCost format. Project not updated.");
            }
        }
    }

}
