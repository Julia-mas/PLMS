namespace PLMS.BLL.DTO.TasksDto
{
    public class TaskShortDto
    {
        public string Title { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
    }
}
