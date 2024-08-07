namespace PLMS.API.Models.ModelsTasks
{
    public class EditTaskModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public int GoalId { get; set; }
        public EditGoalModel? Goal { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public IEnumerable<TaskCommentModel>? TaskComments { get; set; }
    }
}
