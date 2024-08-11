namespace PLMS.BLL.DTO
{
    public class AddTaskDto: TaskBaseDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public AddGoalDto? Goal { get; set; }
    }
}
