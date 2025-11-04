// Controllers/TasksController.cs
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TaskManagementAPI.Models;
using TaskManagerAPI.Services;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Get all tasks with optional filtering
        /// </summary>
        /// <param name="status">Filter by task status</param>
        /// <param name="priority">Filter by priority</param>
        /// <param name="projectId">Filter by project ID</param>
        /// <param name="assignedTo">Filter by assignee</param>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10, max: 100)</param>
        /// <returns>List of tasks</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks(
            [FromQuery] TaskStatus? status = null,
            [FromQuery] TaskPriority? priority = null,
            [FromQuery] int? projectId = null,
            [FromQuery] string? assignedTo = null,
            [FromQuery] int page = 1,
            [FromQuery][Range(1, 100)] int pageSize = 10)
        {
            var tasks = await _taskService.GetTasksAsync(status, priority, projectId, assignedTo, page, pageSize);
            return Ok(tasks);
        }

        /// <summary>
        /// Get a specific task by ID
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>Task details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }
            return Ok(task);
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        /// <param name="request">Task creation data</param>
        /// <returns>Created task</returns>
        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask([FromBody] CreateTaskRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _taskService.CreateTaskAsync(request);
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        /// <summary>
        /// Update an existing task
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <param name="request">Task update data</param>
        /// <returns>Updated task</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<TaskItem>> UpdateTask(int id, [FromBody] UpdateTaskRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedTask = await _taskService.UpdateTaskAsync(id, request);
            if (updatedTask == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            return Ok(updatedTask);
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id);
            if (!result)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            return NoContent();
        }

        /// <summary>
        /// Mark a task as completed
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>Updated task</returns>
        [HttpPatch("{id}/complete")]
        public async Task<ActionResult<TaskItem>> CompleteTask(int id)
        {
            var task = await _taskService.CompleteTaskAsync(id);
            if (task == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            return Ok(task);
        }

        /// <summary>
        /// Get task statistics
        /// </summary>
        /// <returns>Task statistics</returns>
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetTaskStatistics()
        {
            var stats = await _taskService.GetTaskStatisticsAsync();
            return Ok(stats);
        }

        /// <summary>
        /// Search tasks by title or description
        /// </summary>
        /// <param name="query">Search query</param>
        /// <returns>Matching tasks</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> SearchTasks([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            var tasks = await _taskService.SearchTasksAsync(query);
            return Ok(tasks);
        }
    }
}
