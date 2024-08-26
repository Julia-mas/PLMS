using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLMS.API.ApiHelper;
using PLMS.API.Models.ModelsGoalComments;
using PLMS.BLL.DTO.GoalCommentsDto;
using PLMS.BLL.ServicesInterfaces;
using PLMS.Common.Exceptions;
using System.Security.Claims;

namespace PLMS.API.Controllers
{
    [Authorize]
    [Route("plms/[controller]")]
    [ApiController]
    public class GoalCommentController : ControllerBase
    {
        private readonly IGoalCommentService _goalCommentService;
        private readonly IMapper _mapper;
        public GoalCommentController(IGoalCommentService goalCommentService, IMapper mapper)
        {
            _goalCommentService = goalCommentService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<GoalCommentModel>> Add(GoalCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(", ", ModelState.Values.First().Errors.First().ErrorMessage);
                return ApiResponseHelper.CreateErrorResponse(errorMessage, StatusCodes.Status400BadRequest);
            }
            var goalCommentDto = _mapper.Map<AddGoalCommentDto>(model);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int commentId;

            try
            {
                commentId = await _goalCommentService.AddGoalComment(goalCommentDto, userId);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            return ApiResponseHelper.CreateOkResponseWithMessage("Goal comment was added successfully", commentId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(GoalCommentModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.CreateValidationErrorResponse(ModelState);
            }
            var commentDto = _mapper.Map<EditGoalCommentDto>(model);
            commentDto.Id = id;

            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _goalCommentService.EditGoalComment(commentDto, userId);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status500InternalServerError);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            return ApiResponseHelper.CreateOkResponseWithMessage<string>("Goal comment was updated successfully");
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _goalCommentService.DeleteGoalComment(id, userId);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            return ApiResponseHelper.CreateOkResponseWithMessage<string>("Goal comment was deleted successfully");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetGoalCommentViewModel>> GetById(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            GetGoalCommentDto commentDto;
            try
            {
                commentDto = await _goalCommentService.GetGoalCommentByIdAsync(id, userId);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            if (!ModelState.IsValid)
            {
                ApiResponseHelper.CreateValidationErrorResponse(ModelState);
            }

            var commentModel = _mapper.Map<GetGoalCommentViewModel>(commentDto);

            return ApiResponseHelper.CreateOkResponseWithoutMessage(commentModel);
        }
    }
}
