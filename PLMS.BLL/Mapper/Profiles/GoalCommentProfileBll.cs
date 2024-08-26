using AutoMapper;
using PLMS.BLL.DTO.GoalCommentsDto;
using PLMS.DAL.Entities;

namespace PLMS.BLL.Mapper.Profiles
{
    public class GoalCommentProfileBll : Profile
    {
        public GoalCommentProfileBll() 
        {
            CreateMap<AddGoalCommentDto, GoalComment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Goal, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<GoalComment, GetGoalCommentDto>();
        }
    }
}
