namespace PLMS.BLL.DTO.TaskCommentsDto
{
    public class AddTaskCommentDto : TaskCommentBaseDto
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
