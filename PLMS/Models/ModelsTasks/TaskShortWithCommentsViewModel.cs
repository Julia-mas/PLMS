using PLMS.API.Models.ModelsTaskComments;

namespace PLMS.API.Models.ModelsTasks
{
    public class TaskShortWithCommentsViewModel: TaskShortViewModel
    {
        public string GoalTitle { get; set; } = null!;
        public IEnumerable<GetTaskCommentViewModel> TaskComments { get; set; } = new List<GetTaskCommentViewModel>();
    }
}
