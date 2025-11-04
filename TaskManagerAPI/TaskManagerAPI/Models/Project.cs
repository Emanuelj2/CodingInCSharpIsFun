using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public ProjectStatus Status { get; set; } = ProjectStatus.Active;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DueDate { get; set; }

        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
        public string? Owner { get; set; }

        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }

    public class CreateProjectRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
        public string? Owner { get; set; }
    }

    public enum ProjectStatus
    {
        Active,
        OnHold,
        Completed,
        Cancelled
    }
}