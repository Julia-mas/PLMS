namespace PLMS.BLL.DTO
{
    public class GetTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int GoalId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public int StatusId { get; set; }
        public int PriorityId { get; set; }
    }
}

