using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetProjectsAsync(ProjectStatus? status);
        Task<Project?> GetProjectByIdAsync(int id, bool includeTasks);
        Task<Project> CreateProjectAsync(CreateProjectRequest request);
        Task<Project?> UpdateProjectAsync(int id, CreateProjectRequest request);
        Task<bool> DeleteProjectAsync(int id);
        Task<IEnumerable<TaskItem>?> GetProjectTasksAsync(int id);
    }
}
