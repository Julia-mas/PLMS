using PLMS.BLL.DTO.CategoriesDto;
using PLMS.BLL.Filters;

namespace PLMS.BLL.ServicesInterfaces
{
    public interface ICategoryService
    {
        Task<int> AddCategoryAsync(AddCategoryDto categoryDto);

        Task EditCategoryAsync(EditCategoryDto categoryDto, string userId);

        Task DeleteCategoryAsync(int id, string userId);
 
        Task<GetCategoryDto> GetCategoryByIdAsync(int id, string userId);


        Task<List<GetCategoryDto>> GetCategorieFilteredAsync(CategoryFilter filters);
    }
}
