namespace PLMS.DAL.Entities
{
    public class Goal
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public int StatusId { get; set; }
        public int PriorityId { get; set; }
        public string UserId { get; set; } = null!;
        public int CategoryId { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public Status Status { get; set; } = null!;
        public Priority Priority { get; set; } = null!;
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public ICollection<GoalComment> GoalComments { get; set; } = new List<GoalComment>();
    }
}
