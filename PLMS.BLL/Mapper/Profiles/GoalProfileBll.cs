using AutoMapper;
using PLMS.BLL.DTO;
using PLMS.DAL.Entities;

namespace PLMS.BLL.Mapper.Profiles
{
    public class GoalProfileBll : Profile
    {
        public GoalProfileBll() 
        {
            CreateMap<AddGoalDto, Goal>()
                .ForMember(dest => dest.Tasks, source => source.MapFrom(s => s.Tasks))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Priority, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.GoalComments, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore()).ReverseMap();

            CreateMap<EditGoalDto, Goal>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Priority, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore()).ReverseMap();
        }
    }
}
