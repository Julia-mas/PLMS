using AutoMapper;
using PLMS.API.Models;
using PLMS.BLL.DTO;

namespace PLMS.API.Mapper.Profiles
{
    public class TaskCommentProfile: Profile
    {
        public TaskCommentProfile() 
        {
            CreateMap<TaskCommentModel, TaskCommentDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
