using PLMS.API.Models.ModelsTasks;

namespace PLMS.API.Models
{
    public class EditGoalModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public ICollection<EditTaskModel>? Tasks { get; set; }
        public ICollection<GoalCommentModel>? GoalComments { get; set; }
        public int CategoryId { get; set; }
    }
}
