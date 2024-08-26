using PLMS.BLL.DTO.GoalsDto;
using PLMS.BLL.Filters;

namespace PLMS.BLL.ServicesInterfaces
{
    public interface IGoalService
    {
        Task<int> AddGoalAsync(AddGoalDto taskDto);

        Task EditGoalAsync(EditGoalDto taskDto, string userId);

        Task DeleteGoalAsync(int id, string userId);

        
        Task<GetGoalDto> GetGoalByIdAsync(int id, string userId);

        Task<GoalCompletionInfoDto> GetTaskCompletionPercentageAsync(string userId);

        Task<List<GetGoalDto>> GetFilteredGoalsAsync(GoalFilter filters);

    }
}
