using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly List<TaskItem> _tasks = new();
        private int _nextId = 1;

        public TaskService() { }

        
    }
}
