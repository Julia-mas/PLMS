
namespace PLMS.BLL.DTO
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string GoalTitle { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public IEnumerable<TaskCommentDto> Comments { get; set; } = new List<TaskCommentDto>();
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public int CategoryId { get; set; }
    }
}
