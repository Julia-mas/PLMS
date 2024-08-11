namespace PLMS.API.Models.ModelsTasks
{
    public class TaskShortWithCommentsModel: TaskShortModel
    {
        public string GoalTitle { get; set; } = null!;
        public IEnumerable<TaskCommentModel> TaskComments { get; set; } = new List<TaskCommentModel>();
    }
}
