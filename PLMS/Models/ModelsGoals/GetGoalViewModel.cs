using PLMS.API.Models.ModelsGoalComments;

namespace PLMS.API.Models.ModelsGoals
{
    public class GetGoalViewModel : GoalBaseModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CategoryTitle { get; set; } = string.Empty;
        public string StatusTitle { get; set; } = string.Empty;
        public string PriorityTitle { get; set; } = string.Empty;

        public int TaskCount { get; set; }

        public List<GetGoalCommentViewModel> GoalComments { get; set; } = new List<GetGoalCommentViewModel>();
    }
}
