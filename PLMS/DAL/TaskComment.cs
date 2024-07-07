namespace PLMS.DAL
{
    public class TaskComment
    {
        public int Id { get; set; }
        public string Comment { get; set; } = null!;
        public int TaskId { get; set; }

        // Navigation property
        public Task Task { get; set; } = null!;
    }
}
