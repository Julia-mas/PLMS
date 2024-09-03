namespace PLMS.BLL.DTO.TaskCommentsDto
{
    public class GetTaskCommentDto : TaskCommentBaseDto
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
