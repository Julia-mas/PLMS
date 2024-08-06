namespace PLMS.API.Models.ModelsTasks
{
    public class AddTaskModel
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public int GoalId { get; set; }
        public DateTime DueDate { get; set; }
        public IEnumerable<TaskCommentModel> TaskComments { get; set; } = new List<TaskCommentModel>();
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public AddGoalModel Goal { get; set; } = new AddGoalModel();
    }
}
