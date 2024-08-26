using PLMS.BLL.DTO.CategiesDto;

namespace PLMS.BLL.DTO.CategoriesDto
{
    public class GetCategoryDto : CategoryBaseDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;

        public List<string> GoalTitles { get; set; } = new List<string>();
    }
}
