using NovaApp.Models;

namespace NovaApp.Services.Tasks
{
    public interface IRestTaskService
    {
        Task SaveTaskAsync(CreateTaskDto task, string projectid);
    }
}
