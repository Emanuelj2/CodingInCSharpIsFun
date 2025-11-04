// Services/ProjectService.cs
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services
{
    public class ProjectService : IProjectService
    {
        private readonly List<Project> _projects = new();
        private readonly ITaskService _taskService;
        private int _nextId = 1;

        public ProjectService(ITaskService taskService)
        {
            _taskService = taskService;
            SeedSampleData();
        }

        public async Task<IEnumerable<Project>> GetProjectsAsync(ProjectStatus? status)
        {
            await Task.Delay(10);

            var query = _projects.AsQueryable();

            if (status.HasValue)
                query = query.Where(p => p.Status == status.Value);

            return query.OrderByDescending(p => p.CreatedAt).ToList();
        }

        public async Task<Project?> GetProjectByIdAsync(int id, bool includeTasks)
        {
            await Task.Delay(10);

            var project = _projects.FirstOrDefault(p => p.Id == id);
            if (project == null) return null;

            if (includeTasks)
            {
                var allTasks = await _taskService.GetTasksAsync(null, null, id, null, 1, 1000);
                project.Tasks = allTasks.ToList();
            }

            return project;
        }

        public async Task<Project> CreateProjectAsync(CreateProjectRequest request)
        {
            await Task.Delay(10);

            var project = new Project
            {
                Id = _nextId++,
                Name = request.Name,
                Description = request.Description,
                DueDate = request.DueDate,
                Owner = request.Owner,
                CreatedAt = DateTime.UtcNow
            };

            _projects.Add(project);
            return project;
        }

        public async Task<Project?> UpdateProjectAsync(int id, CreateProjectRequest request)
        {
            await Task.Delay(10);

            var project = _projects.FirstOrDefault(p => p.Id == id);
            if (project == null) return null;

            project.Name = request.Name;
            project.Description = request.Description;
            project.DueDate = request.DueDate;
            project.Owner = request.Owner;

            return project;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            await Task.Delay(10);

            var project = _projects.FirstOrDefault(p => p.Id == id);
            if (project == null) return false;

            _projects.Remove(project);
            return true;
        }

        public async Task<IEnumerable<TaskItem>?> GetProjectTasksAsync(int id)
        {
            var project = _projects.FirstOrDefault(p => p.Id == id);
            if (project == null) return null;

            return await _taskService.GetTasksAsync(null, null, id, null, 1, 1000);
        }

        private void SeedSampleData()
        {
            _projects.AddRange(new[]
            {
                new Project { Id = _nextId++, Name = "E-commerce Platform", Description = "Build a modern e-commerce solution", Status = ProjectStatus.Active, Owner = "Alice Wilson", DueDate = DateTime.UtcNow.AddMonths(3) },
                new Project { Id = _nextId++, Name = "Mobile App Redesign", Description = "Redesign mobile app UI/UX", Status = ProjectStatus.Active, Owner = "Mike Brown", DueDate = DateTime.UtcNow.AddMonths(2) }
            });
        }
    }
}