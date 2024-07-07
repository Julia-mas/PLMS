namespace PLMS.DAL
{
    public class GoalComment
    {
        public int Id { get; set; }
        public string Comment { get; set; } = null!;
        public int GoalId { get; set; }

        // Navigation property
        public Goal Goal { get; set; } = null!;
    }
}
