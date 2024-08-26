namespace PLMS.API.Models.ModelsCategories
{
    public class GetCategoryViewModel: CategoryBaseModel
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;

        public List<string> GoalTitles { get; set; } = new List<string>();
    }
}
