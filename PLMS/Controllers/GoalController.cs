using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLMS.API.ApiHelper;
using PLMS.API.Models.ModelsGoals;
using PLMS.BLL.DTO.GoalsDto;
using PLMS.BLL.Filters;
using PLMS.BLL.ServicesInterfaces;
using PLMS.Common.Exceptions;
using System.Security.Claims;

namespace PLMS.API.Controllers
{
    [Authorize]
    [Route("plms/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly IGoalService _goalService;
        private readonly IMapper _mapper;

        public GoalController(IGoalService goalService, IMapper mapper)
        {
            _goalService = goalService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<GoalBaseModel>> Add(GoalBaseModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(", ", ModelState.Values.First().Errors.First().ErrorMessage);
                return ApiResponseHelper.CreateErrorResponse(errorMessage, StatusCodes.Status400BadRequest);
            }
            var goalDto = _mapper.Map<AddGoalDto>(model);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            goalDto.UserId = userId;
            int goalId;

            try
            {
                goalId = await _goalService.AddGoalAsync(goalDto);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            return ApiResponseHelper.CreateOkResponseWithMessage("Goal was added successfully", goalId);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(GoalBaseModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.CreateValidationErrorResponse(ModelState);
            }
            var goalDto = _mapper.Map<EditGoalDto>(model);
            goalDto.Id = id;

            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _goalService.EditGoalAsync(goalDto, userId);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status500InternalServerError);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            return ApiResponseHelper.CreateOkResponseWithMessage<string>("Goal was updated successfully");
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _goalService.DeleteGoalAsync(id, userId);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            return ApiResponseHelper.CreateOkResponseWithMessage<string>("Goal was deleted successfully");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetGoalViewModel>> GetById(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            GetGoalDto goalDto;
            try
            {
                goalDto = await _goalService.GetGoalByIdAsync(id, userId);
            }
            catch (NotFoundException ex)
            {
                return ApiResponseHelper.CreateErrorResponse(ex.Message, StatusCodes.Status404NotFound);
            }

            if (!ModelState.IsValid)
            {
                ApiResponseHelper.CreateValidationErrorResponse(ModelState);
            }

            var goalModel = _mapper.Map<GetGoalViewModel>(goalDto);

            return ApiResponseHelper.CreateOkResponseWithoutMessage(goalModel);
        }

        [HttpGet("GetFilteredGoals")]
        public async Task<ActionResult<IEnumerable<GetGoalViewModel>>> GetFilteredFull([FromQuery] GoalFilter filters)
        {
            filters.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(filters.UserId))
            {
                return ApiResponseHelper.CreateErrorResponse("Goals were not found!", StatusCodes.Status404NotFound);
            }

            var goalDto = await _goalService.GetFilteredGoalsAsync(filters);
            var goalModel = goalDto.Select(g => _mapper.Map<GetGoalViewModel>(g));

            return ApiResponseHelper.CreateOkResponseWithoutMessage(goalModel);
        }

        [HttpGet("GetTaskCompletionPercentage")]
        public async Task<ActionResult<GoalCompletionInfoDto>> GetGoalCompletionInfo()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResponseHelper.CreateErrorResponse("Goals were not found!", StatusCodes.Status404NotFound);
            }

            var goalInfoDto = await _goalService.GetTaskCompletionPercentageAsync(userId);
            if (goalInfoDto == null) 
            {
                return ApiResponseHelper.CreateErrorResponse("Goals were not found!", StatusCodes.Status404NotFound);
            }

            var goalInfoModel = _mapper.Map<GoalCompletionInfoModel>(goalInfoDto);

            return ApiResponseHelper.CreateOkResponseWithoutMessage(goalInfoModel);
        }
    }
}
