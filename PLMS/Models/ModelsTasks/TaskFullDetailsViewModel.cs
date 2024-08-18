namespace PLMS.API.Models.ModelsTasks
{
    public class TaskFullDetailsViewModel: TaskShortWithCommentsViewModel
    {
        public string Description { get; set; } = string.Empty;
        public string CategoryTitle { get; set; } = null!;
        public string PriorityTitle { get; set; } = null!;
        public string StatusTitle { get; set; } = null!;
    }
}
