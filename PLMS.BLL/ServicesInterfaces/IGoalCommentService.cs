using PLMS.BLL.DTO.GoalCommentsDto;

namespace PLMS.BLL.ServicesInterfaces
{
    public interface IGoalCommentService
    {
        Task<int> AddGoalComment(AddGoalCommentDto commentDto, string userId);

        Task EditGoalComment(EditGoalCommentDto commentDto, string userId);

        Task DeleteGoalComment(int id, string userId);


        Task<GetGoalCommentDto> GetGoalCommentByIdAsync(int id, string userId);
    }
}
