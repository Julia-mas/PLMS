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
        public async Task<ActionResult<GetTaskModel>> GetById(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            GetTaskDto taskDto;
            try
            {
                taskDto = await _taskService.GetTaskByIdAsync(id, userId);
            }
            catch (UnauthorizedAccessException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status403Forbidden);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            if(!ModelState.IsValid)
            {
                var errorMessage = string.Join(", ", ModelState.Values.First().Errors.First().ErrorMessage);
                return ApiResponseHelper.CreateErrorResponse(errorMessage, StatusCodes.Status400BadRequest);
            }

            var taskModel = _mapper.Map<GetTaskModel>(taskDto);

            return Ok(taskModel);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(EditTaskModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(", ", ModelState.Values.First().Errors.First().ErrorMessage);
                return ApiResponseHelper.CreateErrorResponse(errorMessage, StatusCodes.Status400BadRequest);
            }
            var taskDto = _mapper.Map<EditTaskDto>(model);

            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _taskService.EditTaskAsync(taskDto, id, userId);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status500InternalServerError);
            }
            catch (UnauthorizedAccessException ex) 
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status403Forbidden);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            return ApiResponseHelper.CreateOkResponse<string>("Task was updated successfully");
        }

        [HttpPost]
        public async Task<ActionResult<TaskModelBase>> Add(TaskModelBase model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(", ", ModelState.Values.First().Errors.First().ErrorMessage);
                return ApiResponseHelper.CreateErrorResponse(errorMessage, StatusCodes.Status400BadRequest);
            }
            var taskDto = _mapper.Map<AddTaskDto>(model);
            int idTask = await _taskService.AddTaskAsync(taskDto);

            return ApiResponseHelper.CreateOkResponse("Task was added successfully", StatusCodes.Status200OK, idTask);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _taskService.DeleteTaskAsync(id, userId);
            }
            catch (UnauthorizedAccessException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status403Forbidden);
            }

            return ApiResponseHelper.CreateOkResponse<string>("Task was deleted successfully");
        }

        [HttpGet("GetFilteredShort")]
        public async Task<ActionResult<IEnumerable<TaskShortModel>>> GetFilteredShort([FromQuery] TaskFilter filters)
        {
            filters.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var taskDto = await _taskService.GetFilteredShortTasksAsync(filters);
            var taskModel = taskDto.Select(t => _mapper.Map<TaskShortModel>(t));

            return ApiResponseHelper.CreateOkResponse("Returned filtered tasks without details", StatusCodes.Status200OK, taskModel);
        }

        [HttpGet("GetFilteredShortWithComments")]
        public async Task<ActionResult<TaskShortWithCommentsModel>> GetFilteredShortWithComments([FromQuery] TaskFilter filters)
        {
            filters.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var taskDto = await _taskService.GetFilteredShortWithCommentsAsync(filters);
            var taskModel = taskDto.Select(t => _mapper.Map<TaskShortWithCommentsModel>(t));

            return ApiResponseHelper.CreateOkResponse("Returned filtered tasks with comments", StatusCodes.Status200OK, taskModel);
        }

        [HttpGet("GetFilteredFull")]
        public async Task<ActionResult<IEnumerable<TaskFullDetailsModel>>> GetFilteredFull([FromQuery] TaskFilter filters)
        {
            filters.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var taskDto = await _taskService.GetFilteredFullAsync(filters);
            var taskModel = taskDto.Select(t => _mapper.Map<TaskFullDetailsModel>(t));

            return ApiResponseHelper.CreateOkResponse("Returned filtered tasks with full details", StatusCodes.Status200OK, taskModel);
        }
    }
}
