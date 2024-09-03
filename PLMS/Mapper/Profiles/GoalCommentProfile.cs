using AutoMapper;
using PLMS.API.Models.ModelsGoalComments;
using PLMS.BLL.DTO.GoalCommentsDto;

namespace PLMS.API.Mapper.Profiles
{
    public class GoalCommentProfile: Profile
    {
        public GoalCommentProfile() 
        {
            CreateMap<GoalCommentModel, EditGoalCommentDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<GoalCommentModel, AddGoalCommentDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<GetGoalCommentDto, GetGoalCommentViewModel>();

        }
    }
}
