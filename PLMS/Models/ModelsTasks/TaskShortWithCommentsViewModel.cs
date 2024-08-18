using PLMS.API.Models.ModelsComments;

namespace PLMS.API.Models.ModelsTasks
{
    public class TaskShortWithCommentsViewModel: TaskShortViewModel
    {
        public string GoalTitle { get; set; } = null!;
        public IEnumerable<TaskCommentModel> TaskComments { get; set; } = new List<TaskCommentModel>();
    }
}
