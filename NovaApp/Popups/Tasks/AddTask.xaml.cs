using Microsoft.UI.Xaml.Media;
using Mopups.Services;
using NovaApp.Models;
using NovaApp.Services;
using NovaApp.Services.Tasks;
using NovaApp.ViewModels;
using System.Diagnostics;
using Windows.System;

namespace NovaApp.Popups.Tasks;

public partial class AddTask
{
    private StaffViewModel StaffViewModel;

    public string assignedUser; 

    public string projectId;

    private TaskRestService taskService;

    public AddTask(string ProjectId)
	{
        InitializeComponent();
        taskService = new TaskRestService();
        StaffViewModel = new StaffViewModel(new RestService());
        BindingContext = StaffViewModel;
        projectId = ProjectId;
        LoadStaff();

    }

    public async void LoadStaff()
    {
        await StaffViewModel.FetchAllStaff();
    }

    public async void OnAddTaskClicked(object sender, EventArgs e)
    {
        CreateTaskDto createTaskDto = new CreateTaskDto();

        createTaskDto.title = TitleEntry.Text;
        createTaskDto.description = DescriptionEntry.Text;
        createTaskDto.assignedUserId = assignedUser;
        createTaskDto.workHours = int.Parse(WorkHoursEntry.Text);

        await taskService.SaveTaskAsync(createTaskDto, projectId);
        // await MopupService.Instance.PopAllAsync();
    }

    

    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        MopupService.Instance.PopAllAsync();
    }

    private void OnUserPickerSelectedUser(object sender, EventArgs e)
    {
        Debug.WriteLine("SelectedIndexChanged event fired.");

        var picker = (Picker)sender;
        var selectedUser = (Staff)picker.SelectedItem;
        if (selectedUser != null)
        {
            Debug.WriteLine($"Selected User ID: {selectedUser.id}");
            assignedUser = selectedUser.id;
        }
    }

    
};