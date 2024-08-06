namespace PLMS.BLL.DTO
{
    public class AddTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public int GoalId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public IEnumerable<TaskCommentDto>? TaskComments { get; set; }
        public AddGoalDto? Goal { get; set; }
    }
}
