using PLMS.BLL.DTO.CommentsDto;

namespace PLMS.BLL.DTO.TasksDto
{
    public class TaskShortWithCommentsDto: TaskShortDto
    {
        public string GoalTitle { get; set; } = null!;
        public IEnumerable<TaskCommentDto> TaskComments { get; set; } = new List<TaskCommentDto>();
    }
}
