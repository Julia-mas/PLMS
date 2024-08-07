namespace PLMS.API.Models.ModelsTasks
{
    public class TaskShortWithCommentsModel
    {
        public string Title { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string GoalTitle { get; set; } = null!;
        public IEnumerable<TaskCommentModel> TaskComments { get; set; } = new List<TaskCommentModel>();
    }
}
