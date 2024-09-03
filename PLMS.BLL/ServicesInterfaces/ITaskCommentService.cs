using PLMS.BLL.DTO.TaskCommentsDto;

namespace PLMS.BLL.ServicesInterfaces
{
    public interface ITaskCommentService
    {
        Task<int> AddTaskComment(AddTaskCommentDto commentDto, string userId);

        Task EditTaskComment(EditTaskCommentDto commentDto, string userId);

        Task DeleteTaskComment(int id, string userId);


        Task<GetTaskCommentDto> GetTaskCommentByIdAsync(int id, string userId);
    }
}
