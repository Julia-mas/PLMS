namespace PLMS.API.Models.ModelsComments
{
    public class TaskCommentModel
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int TaskId { get; set; }
    }
}
