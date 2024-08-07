namespace PLMS.BLL.DTO
{
    public class AddGoalDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public int StatusId { get; set; }
        public int PriorityId { get; set; }
        public string UserId { get; set; } = null!;
        public int CategoryId { get; set; }

        public ICollection<AddTaskDto>? Tasks { get; set; }
        public ICollection<GoalCommentDto>? GoalComments { get; set; }

        public AddCategoryDto? Category { get; set; }
    }
}
