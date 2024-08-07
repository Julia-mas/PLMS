using AutoMapper;
using PLMS.API.Models;
using PLMS.BLL.DTO;

namespace PLMS.API.Mapper.Profiles
{
    public class CategoryProfile: Profile
    {
        public CategoryProfile()
        {
            CreateMap<EditCategoryModel, CategoryDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<AddCategoryModel, AddCategoryDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
