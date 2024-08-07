using AutoMapper;
using PLMS.BLL.DTO;
using PLMS.DAL.Entities;

namespace PLMS.BLL.Mapper.Profiles
{
    public class PriorityProfileBll : Profile
    {
        public PriorityProfileBll() 
        {
            CreateMap<PriorityDto, Priority>()
                .ForMember(dest => dest.Goals, opt => opt.Ignore())
                .ForMember(dest => dest.Tasks, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
