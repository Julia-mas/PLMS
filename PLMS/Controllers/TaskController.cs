using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLMS.API.ApiHelper;
using PLMS.API.Models.ModelsTasks;
using PLMS.BLL.DTO.TasksDto;
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
        public async Task<ActionResult<GetTaskViewModel>> GetById(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            GetTaskDto taskDto;
            try
            {
                taskDto = await _taskService.GetTaskByIdAsync(id, userId);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            if(!ModelState.IsValid)
            {
                ApiResponseHelper.CreateValidationErrorResponse(ModelState);
            }

            var taskModel = _mapper.Map<GetTaskViewModel>(taskDto);

            return ApiResponseHelper.CreateOkResponseWithoutMessage(taskModel);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(TaskBaseModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.CreateValidationErrorResponse(ModelState);
            }
            var taskDto = _mapper.Map<EditTaskDto>(model);
            taskDto.Id = id;

            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _taskService.EditTaskAsync(taskDto, userId);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status500InternalServerError);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            return ApiResponseHelper.CreateOkResponseWithMessage<string>("Task was updated successfully");
        }

        [HttpPost]
        public async Task<ActionResult<TaskBaseModel>> Add(TaskBaseModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(", ", ModelState.Values.First().Errors.First().ErrorMessage);
                return ApiResponseHelper.CreateErrorResponse(errorMessage, StatusCodes.Status400BadRequest);
            }
            var taskDto = _mapper.Map<AddTaskDto>(model);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            taskDto.UserId = userId;
            int idTask;

            try
            {
                idTask = await _taskService.AddTaskAsync(taskDto);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            return ApiResponseHelper.CreateOkResponseWithMessage("Task was added successfully", idTask);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _taskService.DeleteTaskAsync(id, userId);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            return ApiResponseHelper.CreateOkResponseWithMessage<string>("Task was deleted successfully");
        }

        [HttpGet("GetFilteredShort")]
        public async Task<ActionResult<IEnumerable<TaskShortViewModel>>> GetFilteredShort([FromQuery] TaskFilter filters)
        {
            filters.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(filters.UserId))
            {
                return ApiResponseHelper.CreateErrorResponse("Tasks were not found!", StatusCodes.Status404NotFound);
            }

            var taskDto = await _taskService.GetFilteredShortTasksAsync(filters);
            var taskModel = taskDto.Select(t => _mapper.Map<TaskShortViewModel>(t));

            return ApiResponseHelper.CreateOkResponseWithoutMessage(taskModel);
        }

        [HttpGet("GetFilteredShortWithComments")]
        public async Task<ActionResult<TaskShortWithCommentsViewModel>> GetFilteredShortWithComments([FromQuery] TaskFilter filters)
        {
            filters.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(filters.UserId)) 
            {
                return ApiResponseHelper.CreateErrorResponse("Tasks were not found!", StatusCodes.Status404NotFound);
            }

            var taskDto = await _taskService.GetFilteredShortWithCommentsAsync(filters);
            var taskModel = taskDto.Select(t => _mapper.Map<TaskShortWithCommentsViewModel>(t));

            return ApiResponseHelper.CreateOkResponseWithoutMessage(taskModel);
        }

        [HttpGet("GetFilteredFull")]
        public async Task<ActionResult<IEnumerable<TaskFullDetailsViewModel>>> GetFilteredFull([FromQuery] TaskFilter filters)
        {
            filters.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(filters.UserId))
            {
                return ApiResponseHelper.CreateErrorResponse("Tasks were not found!", StatusCodes.Status404NotFound);
            }

            var taskDto = await _taskService.GetFilteredFullAsync(filters);
            var taskModel = taskDto.Select(t => _mapper.Map<TaskFullDetailsViewModel>(t));

            return ApiResponseHelper.CreateOkResponseWithoutMessage(taskModel);
        }
    }
}
