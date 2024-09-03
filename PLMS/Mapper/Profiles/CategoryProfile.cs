using AutoMapper;
using PLMS.API.Models.ModelsCategories;
using PLMS.BLL.DTO.CategoriesDto;

namespace PLMS.API.Mapper.Profiles
{
    public class CategoryProfile: Profile
    {
        public CategoryProfile()
        {
            CreateMap<GetCategoryDto, GetCategoryViewModel>();

            CreateMap<GetCategoryDto.Goal, GetCategoryViewModel.Goal>();

            CreateMap<CategoryBaseModel, AddCategoryDto>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<CategoryBaseModel, EditCategoryDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
