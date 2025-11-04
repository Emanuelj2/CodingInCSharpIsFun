using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetTasksAsync(TaskStatus? status, TaskPriority? priority, int? projectId, string? assignedTo, int page, int pageSize);
        Task<TaskItem?> GetTaskByIdAsync(int id);
        Task<TaskItem> CreateTaskAsync(CreateTaskRequest request);
        Task<TaskItem?> UpdateTaskAsync(int id, UpdateTaskRequest request);
        Task<bool> DeleteTaskAsync(int id);
        Task<TaskItem?> CompleteTaskAsync(int id);
        Task<object> GetTaskStatisticsAsync();
        Task<IEnumerable<TaskItem>> SearchTasksAsync(string query);
    }
}
