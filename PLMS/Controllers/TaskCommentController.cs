using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLMS.API.ApiHelper;
using PLMS.API.Models.ModelsTaskComments;
using PLMS.BLL.DTO.TaskCommentsDto;
using PLMS.BLL.ServicesInterfaces;
using PLMS.Common.Exceptions;
using System.Security.Claims;

namespace PLMS.API.Controllers
{
    [Authorize]
    [Route("plms/[controller]")]
    [ApiController]
    public class TaskCommentController : ControllerBase
    {
        private readonly ITaskCommentService _taskCommentService;
        private readonly IMapper _mapper;

        public TaskCommentController(ITaskCommentService taskCommentService, IMapper mapper)
        {
            _taskCommentService = taskCommentService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<TaskCommentModel>> Add(TaskCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(", ", ModelState.Values.First().Errors.First().ErrorMessage);
                return ApiResponseHelper.CreateErrorResponse(errorMessage, StatusCodes.Status400BadRequest);
            }

            var taskCommentDto = _mapper.Map<AddTaskCommentDto>(model);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int commentId;

            try
            {
                commentId = await _taskCommentService.AddTaskComment(taskCommentDto, userId);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            return ApiResponseHelper.CreateOkResponseWithMessage("Task comment was added successfully", commentId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(TaskCommentModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.CreateValidationErrorResponse(ModelState);
            }

            var commentDto = _mapper.Map<EditTaskCommentDto>(model);
            commentDto.Id = id;

            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _taskCommentService.EditTaskComment(commentDto, userId);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status500InternalServerError);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            return ApiResponseHelper.CreateOkResponseWithMessage<string>("Task comment was updated successfully");
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _taskCommentService.DeleteTaskComment(id, userId);

            return ApiResponseHelper.CreateOkResponseWithMessage<string>("Task comment was deleted successfully");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetTaskCommentViewModel>> GetById(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            GetTaskCommentDto commentDto;

            try
            {
                commentDto = await _taskCommentService.GetTaskCommentByIdAsync(id, userId);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            var commentModel = _mapper.Map<GetTaskCommentViewModel>(commentDto);

            return ApiResponseHelper.CreateOkResponseWithoutMessage(commentModel);
        }
    }
}
