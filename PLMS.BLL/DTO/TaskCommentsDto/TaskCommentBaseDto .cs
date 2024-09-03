namespace PLMS.BLL.DTO.TaskCommentsDto
{
    public class TaskCommentBaseDto
    {
        public string Comment { get; set; } = string.Empty;
        public int TaskId { get; set; }
    }
}
