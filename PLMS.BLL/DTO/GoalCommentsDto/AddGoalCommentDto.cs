namespace PLMS.BLL.DTO.GoalCommentsDto
{
    public class AddGoalCommentDto : GoalCommentBaseDto
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
