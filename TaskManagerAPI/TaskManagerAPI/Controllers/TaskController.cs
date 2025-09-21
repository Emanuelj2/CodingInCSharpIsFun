using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers
{
    [ApiController] // Indicates that this class is an API controller
    [Route("api/[controller]")] // Route: api/task
    [Produces("application/json")] // Specifies that the controller returns JSON responses
    public class TaskController : ControllerBase 
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }


        


    }
}
