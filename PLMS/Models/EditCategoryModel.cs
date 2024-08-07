namespace PLMS.API.Models
{
    public class EditCategoryModel
    {
        public string? Title { get; set; }
        public ICollection<EditGoalModel>? Goals { get; set; }
    }
}
