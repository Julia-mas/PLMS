namespace PLMS.API.Models.ModelsTasks
{
    public class TaskShortModel
    {
        public string Title { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
    }
}
