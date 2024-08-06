using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLMS.API.ApiHelper;
using PLMS.API.Models.ModelsTasks;
using PLMS.BLL.DTO;
using PLMS.BLL.Filters;
using PLMS.BLL.ServicesInterfaces;
using PLMS.Common.Exceptions;
using System.Security.Claims;

namespace PLMS.API.Controllers
{
    [Authorize]
    [Route("plms/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;

        public TaskController(ITaskService taskService, IMapper mapper)
        {
            _taskService = taskService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetTaskModel>> GetByIdAsync(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            GetTaskDto taskDto;
            try
            {
                taskDto = await _taskService.GetTaskByIdAsync(id, userId);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }

            if (taskDto == null)
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return ApiResponseHelper.CreateErrorResponse(ModelState);
            }

            var taskModel = _mapper.Map<GetTaskModel>(taskDto);

            return Ok(taskModel);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditAsync(EditTaskModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.CreateErrorResponse(ModelState);
            }
            var taskDto = _mapper.Map<EditTaskDto>(model);

            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _taskService.EditTaskAsync(taskDto, id, userId);
            }
            catch (DbUpdateConcurrencyException)
            {
                return ApiResponseHelper.CreateResponse(false, "DbUpdateConcurrencyException", 500);
            }
            catch (UnauthorizedAccessException ex) 
            {
                return Unauthorized(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return ApiResponseHelper.CreateResponse(true, "Task was updated successfully");
        }

        [HttpPost]
        public async Task<ActionResult<AddTaskModel>> AddAsync(AddTaskModel model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.CreateErrorResponse(ModelState);
            }
            var taskDto = _mapper.Map<AddTaskDto>(model);
            await _taskService.AddTaskAsync(taskDto, userId);
            var createdTaskModel = _mapper.Map<AddTaskModel>(taskDto);

            return ApiResponseHelper.CreateResponse(true, "Task was added successfully");
            // CreatedAtAction(nameof(GetByIdAsync), new { id = taskDto.Id }, createdTaskModel);  -- Не получается вернуть маршрут на новое айди, не могу понять в чем ошибка (System.InvalidOperationException: No route matches the supplied values.
         //   at Microsoft.AspNetCore.Mvc.CreatedAtActionResult.OnFormatting(ActionContext context)
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            GetTaskDto taskDto;
            try
            {
                taskDto = await _taskService.GetTaskByIdAsync(id, userId);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }

            if (taskDto == null)
            {
                return ApiResponseHelper.CreateResponse(true, "Task was deleted successfully");
            }

            await _taskService.DeleteTaskAsync(id);

            return ApiResponseHelper.CreateResponse(true, "Task was deleted successfully");
        }

        [HttpGet("GetFilteredShort")]
        public async Task<ActionResult<IEnumerable<TaskShortModel>>> GetFilteredShortAsync([FromQuery] TaskItemFilter filters)
        {
            filters.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var taskDto = await _taskService.GetFilteredShortTasksAsync(filters);
            var taskModel = taskDto.Select(t => _mapper.Map<TaskShortModel>(t));

            return Ok(taskModel);
        }

        [HttpGet("GetFilteredShortWithComments")]
        public async Task<ActionResult<TaskShortWithCommentsModel>> GetFilteredShortWithCommentsAsync([FromQuery] TaskItemFilter filters)
        {
            filters.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var taskDto = await _taskService.GetFilteredShortWithCommentsAsync(filters);
            var taskModel = taskDto.Select(t => _mapper.Map<TaskShortWithCommentsModel>(t));

            return Ok(taskModel);
        }

        [HttpGet("GetFilteredFull")]
        public async Task<ActionResult<IEnumerable<TaskFullDetailsModel>>> GetFilteredFullAsync([FromQuery] TaskItemFilter filters)
        {
            filters.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var taskDto = await _taskService.GetFilteredFullAsync(filters);
            var taskModel = taskDto.Select(t => _mapper.Map<TaskFullDetailsModel>(t));

            return Ok(taskModel);
        }
    }
}
