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

            CreateMap<Category, GetCategoryDto>()
                .ForMember(dest => dest.GoalTitles, source => source.MapFrom(s => s.Goals.Select(t => t.Title)));
        }
    }
}
