namespace PLMS.DAL
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int UserId { get; set; }

        // Navigation property
        public User User { get; set; } = null!;
        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
    }
}
