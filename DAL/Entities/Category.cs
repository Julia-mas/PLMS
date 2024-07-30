namespace PLMS.DAL.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string UserId { get; set; } = null!;

        // Navigation property
        public User User { get; set; } = null!;
        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
    }
}
