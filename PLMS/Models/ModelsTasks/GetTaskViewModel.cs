namespace PLMS.API.Models.ModelsTasks
{
    public class GetTaskViewModel: TaskBaseModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string GoalTitle { get; set; } = string.Empty;
        public string StatusTitle { get; set; } = string.Empty;
        public string PriorityTitle { get; set; } = string.Empty;
    }
}

