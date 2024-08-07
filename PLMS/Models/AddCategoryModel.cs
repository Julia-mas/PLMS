namespace PLMS.API.Models
{
    public class AddCategoryModel
    {
        public string Title { get; set; } = null!;
        public ICollection<AddGoalModel>? Goals { get; set; }
    }
}
