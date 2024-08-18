using AutoMapper;
using PLMS.API.Models.ModelsComments;
using PLMS.BLL.DTO.CommentsDto;

namespace PLMS.API.Mapper.Profiles
{
    public class TaskCommentProfile: Profile
    {
        public TaskCommentProfile() 
        {
            CreateMap<TaskCommentModel, TaskCommentDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
