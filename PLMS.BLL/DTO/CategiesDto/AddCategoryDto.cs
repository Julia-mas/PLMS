using PLMS.BLL.DTO.CategiesDto;

namespace PLMS.BLL.DTO.CategoriesDto
{
    public class AddCategoryDto : CategoryBaseDto
    {
        public string UserId { get; set; } = null!;
    }
}
