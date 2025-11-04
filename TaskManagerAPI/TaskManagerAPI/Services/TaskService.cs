// Services/TaskService.cs
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly List<TaskItem> _tasks = new();
        private int _nextId = 1;

        public TaskService()
        {
            // Seed with sample data
            SeedSampleData();
        }

        public async Task<IEnumerable<TaskItem>> GetTasksAsync(TaskStatus? status, TaskPriority? priority, int? projectId, string? assignedTo, int page, int pageSize)
        {
            await Task.Delay(10); // Simulate async operation

            var query = _tasks.AsQueryable();

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            if (projectId.HasValue)
                query = query.Where(t => t.ProjectId == projectId.Value);

            if (!string.IsNullOrEmpty(assignedTo))
                query = query.Where(t => t.AssignedTo != null && t.AssignedTo.Contains(assignedTo, StringComparison.OrdinalIgnoreCase));

            return query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            await Task.Delay(10);
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public async Task<TaskItem> CreateTaskAsync(CreateTaskRequest request)
        {
            await Task.Delay(10);

            var task = new TaskItem
            {
                Id = _nextId++,
                Title = request.Title,
                Description = request.Description,
                Priority = request.Priority,
                ProjectId = request.ProjectId,
                DueDate = request.DueDate,
                AssignedTo = request.AssignedTo,
                Tags = request.Tags ?? new List<string>(),
                CreatedAt = DateTime.UtcNow
            };

            _tasks.Add(task);
            return task;
        }

        public async Task<TaskItem?> UpdateTaskAsync(int id, UpdateTaskRequest request)
        {
            await Task.Delay(10);

            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return null;

            if (!string.IsNullOrEmpty(request.Title))
                task.Title = request.Title;

            if (request.Description != null)
                task.Description = request.Description;

            if (request.Status.HasValue)
            {
                task.Status = request.Status.Value;
                if (request.Status.Value == TaskStatus.Completed)
                    task.CompletedAt = DateTime.UtcNow;
            }

            if (request.Priority.HasValue)
                task.Priority = request.Priority.Value;

            if (request.DueDate.HasValue)
                task.DueDate = request.DueDate.Value;

            if (request.AssignedTo != null)
                task.AssignedTo = request.AssignedTo;

            if (request.Tags != null)
                task.Tags = request.Tags;

            return task;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            await Task.Delay(10);

            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return false;

            _tasks.Remove(task);
            return true;
        }

        public async Task<TaskItem?> CompleteTaskAsync(int id)
        {
            await Task.Delay(10);

            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return null;

            task.Status = TaskStatus.Completed;
            task.CompletedAt = DateTime.UtcNow;

            return task;
        }

        public async Task<object> GetTaskStatisticsAsync()
        {
            await Task.Delay(10);

            return new
            {
                Total = _tasks.Count,
                Completed = _tasks.Count(t => t.Status == TaskStatus.Completed),
                InProgress = _tasks.Count(t => t.Status == TaskStatus.InProgress),
                Todo = _tasks.Count(t => t.Status == TaskStatus.Todo),
                Overdue = _tasks.Count(t => t.DueDate.HasValue && t.DueDate < DateTime.UtcNow && t.Status != TaskStatus.Completed),
                ByPriority = new
                {
                    Critical = _tasks.Count(t => t.Priority == TaskPriority.Critical),
                    High = _tasks.Count(t => t.Priority == TaskPriority.High),
                    Medium = _tasks.Count(t => t.Priority == TaskPriority.Medium),
                    Low = _tasks.Count(t => t.Priority == TaskPriority.Low)
                }
            };
        }

        public async Task<IEnumerable<TaskItem>> SearchTasksAsync(string query)
        {
            await Task.Delay(10);

            return _tasks.Where(t =>
                t.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                t.Description.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                t.Tags.Any(tag => tag.Contains(query, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        private void SeedSampleData()
        {
            _tasks.AddRange(new[]
            {
                new TaskItem { Id = _nextId++, Title = "Setup CI/CD Pipeline", Description = "Configure automated deployment", Status = TaskStatus.InProgress, Priority = TaskPriority.High, ProjectId = 1, AssignedTo = "John Doe", Tags = new List<string> { "devops", "automation" } },
                new TaskItem { Id = _nextId++, Title = "Fix Login Bug", Description = "Users can't login with special characters", Status = TaskStatus.Todo, Priority = TaskPriority.Critical, ProjectId = 1, AssignedTo = "Jane Smith", Tags = new List<string> { "bug", "security" } },
                new TaskItem { Id = _nextId++, Title = "Write API Documentation", Description = "Document all REST endpoints", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, ProjectId = 2, AssignedTo = "Bob Johnson", CompletedAt = DateTime.UtcNow.AddDays(-2), Tags = new List<string> { "documentation", "api" } }
            });
        }
    }
}
