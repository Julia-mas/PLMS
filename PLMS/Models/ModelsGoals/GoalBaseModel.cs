namespace PLMS.API.Models.ModelsGoals
{
    public class GoalBaseModel
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public int CategoryId { get; set; }
    }
}
