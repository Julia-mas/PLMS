namespace PLMS.BLL.DTO.CommentsDto
{
    public class TaskCommentDto
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int TaskId { get; set; }
    }
}
