using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLMS.BLL.Filters;
using PLMS.BLL.ServicesInterfaces;
using PLMS.BLL.DTO;
using Microsoft.AspNetCore.Authorization;

namespace PLMS.API.Controllers
{
    //ToDo: Once tokens added in the UserController add [Authorize] attribute
    //so you can access UserName of logged in user in the Actions
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
        public async Task<ActionResult> EditAsync(int id)//ToDo: implement edit - create EditTaskModel, EditTaskDto;
                                                         //accept EditTaskModel as input param, then map it to Dto.
                                                         //don't forget about validation & constraints: you can use FluentValidator & ModelState.IsValid (like in UserControler)
                                                         //should check things like Description max length, ids of required entities not null, etc.
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
                return BadRequest();//ToDo: This is a server error, not the client, therefore return 500 Internal Server Error Response
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> AddAsync(TaskDto task)//ToDo: should not accept/return DTO, should accept/return Model/ViewModel instead
        {
            //ToDo: validation for write operations is a must, so implement it, for example, with fluent validator
            await _taskService.AddTaskAsync(task);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = task.Id }, task);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);

            if (task == null)
            {
                return NotFound();//ToDo: should be success, see idempotent requests
            }

            await _taskService.DeleteTaskAsync(id);

            return NoContent();//ToDo: better to just return 200
        }

        [HttpGet("GetFilteredShort")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetFilteredShortAsync([FromQuery] MyItemFilter filters,
            [FromQuery] string sortField, [FromQuery] string includeColumns)
            //ToDo: remove sortField - it's already present in the filters object;
            //ToDo: remove includeColumns - they must not be exposed to the outside!
        {
            //filters.UserName = base.User.Identity.Name; //ToDo: include user in the filters param;
            //it should be always setup from the backend

            var tasks = await _taskService.GetFilteredShortTasksAsync(filters, sortField, includeColumns);

            return Ok(tasks);//ToDo: should not return DTO, should return ViewModel instead
        }

        [HttpGet("GetFilteredShortWithComments")]
        public async Task<ActionResult<TaskDto>> GetFilteredShortWithCommentsAsync([FromQuery] MyItemFilter filters)
        {
            var tasks = await _taskService.GetFilteredShortWithCommentsAsync(filters);

            return Ok(tasks);//ToDo: should not return DTO, should return ViewModel instead
        }

        [HttpGet("GetFilteredFull")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetFilteredFullAsync([FromQuery] MyItemFilter filters)
        {
            var tasks = await _taskService.GetFilteredFullAsync(filters);

            return Ok(tasks);//ToDo: should not return DTO, should return ViewModel instead
        }
    }
}
