using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    public class TaskItem
    {
        int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public TaskStatus Status { get; set; } = TaskStatus.Todo;

        [Required]
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        
        public int ProjectId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DueDate { get; set; }

        public DateTime? CompletedAt { get; set; }


        [StringLength(100, ErrorMessage = "Assignee name cannot exceed 100 characters")]
        public string? AssignedTo { get; set; }

        public List<string> Tags { get; set; } = new List<string>();


    }

    public class CreateTaskRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public int ProjectId { get; set; }
        public DateTime? DueDate { get; set; }

        [StringLength(100, ErrorMessage = "Assignee name cannot exceed 100 characters")]
        public string? AssignedTo { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }

    public class UpdateTaskRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;
        public TaskStatus Status { get; set; } = TaskStatus.Todo;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public DateTime? DueDate { get; set; }
        [StringLength(100, ErrorMessage = "Assignee name cannot exceed 100 characters")]
        public string? AssignedTo { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }


    /// Enumeration for task priority levels and statuses.
    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum TaskStatus
    {
        Todo,
        InProgress,
        Review,
        Completed,
        Cancelled
    }
}
