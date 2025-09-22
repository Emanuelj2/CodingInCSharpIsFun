// Controllers/ProjectsController.cs
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Models;
using TaskManagerAPI.Services;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Get all projects
        /// </summary>
        /// <param name="status">Filter by project status</param>
        /// <returns>List of projects</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects([FromQuery] ProjectStatus? status = null)
        {
            var projects = await _projectService.GetProjectsAsync(status);
            return Ok(projects);
        }

        /// <summary>
        /// Get a specific project by ID
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <param name="includeTasks">Include associated tasks</param>
        /// <returns>Project details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id, [FromQuery] bool includeTasks = false)
        {
            var project = await _projectService.GetProjectByIdAsync(id, includeTasks);
            if (project == null)
            {
                return NotFound($"Project with ID {id} not found.");
            }
            return Ok(project);
        }

        /// <summary>
        /// Create a new project
        /// </summary>
        /// <param name="request">Project creation data</param>
        /// <returns>Created project</returns>
        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject([FromBody] CreateProjectRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await _projectService.CreateProjectAsync(request);
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        /// <summary>
        /// Update a project
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <param name="request">Project update data</param>
        /// <returns>Updated project</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<Project>> UpdateProject(int id, [FromBody] CreateProjectRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await _projectService.UpdateProjectAsync(id, request);
            if (project == null)
            {
                return NotFound($"Project with ID {id} not found.");
            }

            return Ok(project);
        }

        /// <summary>
        /// Delete a project
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProject(int id)
        {
            var result = await _projectService.DeleteProjectAsync(id);
            if (!result)
            {
                return NotFound($"Project with ID {id} not found.");
            }

            return NoContent();
        }

        /// <summary>
        /// Get tasks for a specific project
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>List of tasks</returns>
        [HttpGet("{id}/tasks")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetProjectTasks(int id)
        {
            var tasks = await _projectService.GetProjectTasksAsync(id);
            if (tasks == null)
            {
                return NotFound($"Project with ID {id} not found.");
            }
            return Ok(tasks);
        }
    }
}