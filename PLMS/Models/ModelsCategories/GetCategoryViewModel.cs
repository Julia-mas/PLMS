namespace PLMS.API.Models.ModelsCategories
{
    public class GetCategoryViewModel: CategoryBaseModel
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;

        public List<Goal> Goals { get; set; } = new List<Goal>();

        public class Goal
        {
            public int Id { get; set; }

            public string Title { get; set; } = string.Empty;
        }
    }
}
