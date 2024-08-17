namespace PLMS.API.Models.ModelsTasks
{
    public class TaskShortViewModel
    {
        public string Title { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
    }
}
