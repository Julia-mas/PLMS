using AutoMapper;
using PLMS.BLL.DTO;
using PLMS.DAL.Entities;

namespace PLMS.BLL.Mapper.Profiles
{
    public class CategoryProfileBll: Profile
    {
        public CategoryProfileBll() 
        {
            CreateMap<AddCategoryDto, Category>()
                .ForMember(dest => dest.User, opt => opt.Ignore()).ReverseMap(); ;
            CreateMap<CategoryDto, Category>()
                .ForMember(dest => dest.User, opt => opt.Ignore()).ReverseMap();
        }
    }
}
