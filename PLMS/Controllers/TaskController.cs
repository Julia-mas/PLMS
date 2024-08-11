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
                return ApiResponseHelper.CreateErrorResponse(ex.Message, 403);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, 404);
            }

            if(!ModelState.IsValid)
            {
                var errorMessage = string.Join(", ", ModelState.Values.First().Errors.First().ErrorMessage);
                return ApiResponseHelper.CreateErrorResponse(errorMessage, 400);
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
                return ApiResponseHelper.CreateErrorResponse(errorMessage, 400);
            }
            var taskDto = _mapper.Map<EditTaskDto>(model);

            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _taskService.EditTaskAsync(taskDto, id, userId);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, 500);
            }
            catch (UnauthorizedAccessException ex) 
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, 403);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, 404);
            }

            return ApiResponseHelper.CreateResponse<string>("Task was updated successfully");
        }

        [HttpPost]
        public async Task<ActionResult<AddTaskModel>> Add(AddTaskModel model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(", ", ModelState.Values.First().Errors.First().ErrorMessage);
                return ApiResponseHelper.CreateErrorResponse(errorMessage, 400);
            }
            var taskDto = _mapper.Map<AddTaskDto>(model);
            await _taskService.AddTaskAsync(taskDto, userId);

            return ApiResponseHelper.CreateResponse<string>("Task was added successfully");
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
                return ApiResponseHelper.CreateErrorResponse(ex.Message, 403);
            }

            return ApiResponseHelper.CreateResponse<string>("Task was deleted successfully");
        }

        [HttpGet("GetFilteredShort")]
        public async Task<ActionResult<IEnumerable<TaskShortModel>>> GetFilteredShort([FromQuery] TaskFilter filters)
        {
            filters.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var taskDto = await _taskService.GetFilteredShortTasksAsync(filters);
            var taskModel = taskDto.Select(t => _mapper.Map<TaskShortModel>(t));

            return Ok(taskModel);
        }

        [HttpGet("GetFilteredShortWithComments")]
        public async Task<ActionResult<TaskShortWithCommentsModel>> GetFilteredShortWithComments([FromQuery] TaskFilter filters)
        {
            filters.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var taskDto = await _taskService.GetFilteredShortWithCommentsAsync(filters);
            var taskModel = taskDto.Select(t => _mapper.Map<TaskShortWithCommentsModel>(t));

            return Ok(taskModel);
        }

        [HttpGet("GetFilteredFull")]
        public async Task<ActionResult<IEnumerable<TaskFullDetailsModel>>> GetFilteredFull([FromQuery] TaskFilter filters)
        {
            filters.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var taskDto = await _taskService.GetFilteredFullAsync(filters);
            var taskModel = taskDto.Select(t => _mapper.Map<TaskFullDetailsModel>(t));

            return Ok(taskModel);
        }
    }
}
