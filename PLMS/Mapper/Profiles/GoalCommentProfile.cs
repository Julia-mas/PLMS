using AutoMapper;
using PLMS.API.Models.ModelsComments;
using PLMS.BLL.DTO.CommentsDto;

namespace PLMS.API.Mapper.Profiles
{
    public class GoalCommentProfile: Profile
    {
        public GoalCommentProfile() 
        {
            CreateMap<GoalCommentModel, GoalCommentDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
