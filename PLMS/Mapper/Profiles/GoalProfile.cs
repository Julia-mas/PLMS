using AutoMapper;
using PLMS.API.Models.ModelsGoals;
using PLMS.BLL.DTO.GoalsDto;

namespace PLMS.API.Mapper.Profiles
{
    public class GoalProfile: Profile
    {
        public GoalProfile() 
        {
            CreateMap<GoalBaseModel, GoalBaseDto>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<GoalBaseModel, AddGoalDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
        }
    }
}
