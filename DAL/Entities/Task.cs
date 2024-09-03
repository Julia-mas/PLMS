namespace PLMS.DAL.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int GoalId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public int StatusId { get; set; }
        public int PriorityId { get; set; }

        public Goal Goal { get; set; } = null!;
        public Status Status { get; set; } =  null!;
        public Priority Priority { get; set; } = null!;
        public List<TaskComment> TaskComments { get; set; } = new List<TaskComment>();
    }
}
