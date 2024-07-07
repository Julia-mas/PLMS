namespace PLMS.DAL
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? Email { get; set; }

        // Navigation property
        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
