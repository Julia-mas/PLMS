namespace PLMS.API.Models.ModelsTasks
{
    public class TaskFullDetailsModel
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public string GoalTitle { get; set; } = null!;
        public string CategoryTitle { get; set; } = null!;
        public string PriorityTitle { get; set; } = null!;
        public string StatusTitle { get; set; } = null!;
        public IEnumerable<TaskCommentModel>? TaskComments { get; set; }
    }
}
