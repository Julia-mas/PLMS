using AutoMapper;
using PLMS.API.Models.ModelsCategories;
using PLMS.BLL.DTO.CategoriesDto;

namespace PLMS.API.Mapper.Profiles
{
    public class CategoryProfile: Profile
    {
        public CategoryProfile()
        {
            CreateMap<EditCategoryModel, CategoryDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<AddCategoryModel, AddCategoryDto>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
        }
    }
}
