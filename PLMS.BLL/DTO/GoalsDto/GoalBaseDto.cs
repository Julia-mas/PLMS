namespace PLMS.BLL.DTO.GoalsDto
{
    public class GoalBaseDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public string UserId { get; set; } = null!;
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public int CategoryId { get; set; }
    }
}
