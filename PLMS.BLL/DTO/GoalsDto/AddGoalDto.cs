namespace PLMS.BLL.DTO.GoalsDto
{
    public class AddGoalDto : GoalBaseDto
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
