namespace PLMS.BLL.DTO
{
    public class EditTaskDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public int GoalId { get; set; }
        public EditGoalDto Goal { get; set; } = new EditGoalDto();
        public int PriorityId { get; set; }
        public int StatusId { get; set; }

        public IEnumerable<TaskCommentDto>? TaskComments { get; set; }
    }
}
