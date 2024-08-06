using AutoMapper;
using PLMS.API.Models;
using PLMS.BLL.DTO;

namespace PLMS.API.Mapper.Profiles
{
    public class GoalProfile: Profile
    {
        public GoalProfile() 
        {
            CreateMap<EditGoalModel, EditGoalDto>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<AddGoalModel, AddGoalDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
