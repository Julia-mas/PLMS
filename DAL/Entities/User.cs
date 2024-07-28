using Microsoft.AspNetCore.Identity;

namespace PMLS.DAL.Entities
{
    public class User: IdentityUser
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;

        // Navigation property
        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
