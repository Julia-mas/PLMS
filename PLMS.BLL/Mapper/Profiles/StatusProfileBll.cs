using AutoMapper;
using PLMS.BLL.DTO;
using PLMS.DAL.Entities;

namespace PLMS.BLL.Mapper.Profiles
{
    public class StatusProfileBll: Profile
    {
        public StatusProfileBll() 
        {
            CreateMap<StatusDto, Status>()
                .ForMember(dest => dest.Goals, opt => opt.Ignore()) 
                .ForMember(dest => dest.Tasks, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
