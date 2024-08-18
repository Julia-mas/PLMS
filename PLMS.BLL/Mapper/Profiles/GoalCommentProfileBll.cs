using AutoMapper;
using PLMS.BLL.DTO.CommentsDto;
using PLMS.DAL.Entities;

namespace PLMS.BLL.Mapper.Profiles
{
    public class GoalCommentProfileBll : Profile
    {
        public GoalCommentProfileBll() 
        {
            CreateMap<GoalCommentDto, GoalComment>()
                .ForMember(dest => dest.Goal, opt => opt.Ignore()).ReverseMap();
        }
    }
}
