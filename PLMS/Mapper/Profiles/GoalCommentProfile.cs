using AutoMapper;
using PLMS.API.Models;
using PLMS.BLL.DTO;

namespace PLMS.API.Mapper.Profiles
{
    public class GoalCommentProfile: Profile
    {
        public GoalCommentProfile() 
        {
            CreateMap<GoalCommentModel, GoalCommentDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
