namespace PMLS.DAL.Entities
{
    public class TaskComment
    {
        public int Id { get; set; }
        public string Comment { get; set; } = null!;
        public int TaskId { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation property
        public Task Task { get; set; } = null!;
    }
}
