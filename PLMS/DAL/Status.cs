namespace PLMS.DAL
{
    public class Status
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
