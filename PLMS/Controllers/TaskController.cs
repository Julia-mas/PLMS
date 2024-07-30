using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLMS.BLL.Filters;
using PLMS.BLL.ServicesInterfaces;
using PLMS.BLL.DTO;

namespace PLMS.API.Controllers
{
    [Route("plms/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetByIdAsync(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditAsync(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);

            if (task.Id != id)
            {
                return BadRequest();
            }

            try
            {
                await _taskService.EditTaskAsync(task);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> AddAsync(TaskDto task)
        {
            await _taskService.AddTaskAsync(task);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = task.Id }, task);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            await _taskService.DeleteTaskAsync(id);

            return NoContent();
        }

        [HttpGet("GetFilteredShort")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetFilteredShortAsync([FromQuery] MyItemFilter filters,
            [FromQuery] string sortField, [FromQuery] string includeColumns)
        {
            var tasks = await _taskService.GetFilteredShortTasksAsync(filters, sortField, includeColumns);
            
            return Ok(tasks);
        }

        [HttpGet("GetFilteredShortWithComments")]
        public async Task<ActionResult<TaskDto>> GetFilteredShortWithCommentsAsync([FromQuery] MyItemFilter filters)
        {
            var tasks = await _taskService.GetFilteredShortWithCommentsAsync(filters);
            
            return Ok(tasks);
        }

        [HttpGet("GetFilteredFull")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetFilteredFullAsync([FromQuery] MyItemFilter filters)
        {
            var tasks = await _taskService.GetFilteredFullAsync(filters);

            return Ok(tasks);
        }
    }
}
