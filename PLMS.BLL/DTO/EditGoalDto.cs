namespace PLMS.BLL.DTO
{
    public class EditGoalDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public string UserId { get; set; } = null!;
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public int CategoryId { get; set; }
        public ICollection<EditTaskDto> Tasks { get; set; } = new List<EditTaskDto>();
        public ICollection<GoalCommentDto> GoalComments { get; set; } = new List<GoalCommentDto>();
    }
}
