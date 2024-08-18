namespace PLMS.API.Models.ModelsComments
{
    public class GoalCommentModel
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int GoalId { get; set; }
    }
}
