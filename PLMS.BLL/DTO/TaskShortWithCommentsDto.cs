namespace PLMS.BLL.DTO
{
    public class TaskShortWithCommentsDto
    {
        public string Title { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string GoalTitle { get; set; } = null!;
        public IEnumerable<TaskCommentDto> TaskComments { get; set; } = new List<TaskCommentDto>();
    }
}
