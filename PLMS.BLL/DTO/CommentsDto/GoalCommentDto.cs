namespace PLMS.BLL.DTO.CommentsDto
{
    public class GoalCommentDto
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int GoalId { get; set; }
    }
}
