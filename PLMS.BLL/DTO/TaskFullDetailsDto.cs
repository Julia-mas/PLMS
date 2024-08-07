namespace PLMS.BLL.DTO
{
    public class TaskFullDetailsDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public string GoalTitle { get; set; } = null!;
        public string CategoryTitle { get; set; } = null!;
        public string PriorityTitle { get; set; } = null!;
        public string StatusTitle { get; set; } = null!;
        public IEnumerable<TaskCommentDto> TaskComments { get; set; } = new List<TaskCommentDto>();
    }
}
