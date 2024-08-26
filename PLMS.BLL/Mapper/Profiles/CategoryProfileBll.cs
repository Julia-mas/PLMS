using AutoMapper;
using PLMS.BLL.DTO.CategoriesDto;
using PLMS.DAL.Entities;

namespace PLMS.BLL.Mapper.Profiles
{
    public class CategoryProfileBll: Profile
    {
        public CategoryProfileBll() 
        {
            CreateMap<AddCategoryDto, Category>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Goals, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Category, GetCategoryDto>();
            
            CreateMap<Goal, GetCategoryDto.Goal>()
                .ForMember(dest => dest.Title, source => source.MapFrom(s => s.Title))
                .ForMember(dest => dest.Id, source => source.MapFrom(s => s.Id));
        }
    }
}
