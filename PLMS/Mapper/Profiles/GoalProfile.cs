using AutoMapper;
using PLMS.API.Models.ModelsGoals;
using PLMS.BLL.DTO.GoalsDto;

namespace PLMS.API.Mapper.Profiles
{
    public class GoalProfile: Profile
    {
        public GoalProfile() 
        {
            CreateMap<GoalBaseModel, AddGoalDto>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<GoalBaseModel, EditGoalDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<GetGoalDto, GetGoalViewModel>();

            CreateMap<GoalCompletionInfoDto, GoalCompletionInfoModel>();

            CreateMap<GoalCompletionInfoDto.GoalInfoDto, GoalCompletionInfoModel.GoalInfoModel>();
        }
    }
}
