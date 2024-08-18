namespace PLMS.BLL.DTO.TasksDto
{
    public class AddTaskDto: TaskBaseDto
    {
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
