using PLMS.BLL.DTO.TaskCommentsDto;

namespace PLMS.BLL.DTO.TasksDto
{
    public class TaskShortWithCommentsDto: TaskShortDto
    {
        public string GoalTitle { get; set; } = null!;
        public IEnumerable<GetTaskCommentDto> TaskComments { get; set; } = new List<GetTaskCommentDto>();
    }
}
