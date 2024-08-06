using AutoMapper;
using PLMS.BLL.DTO;
using PLMS.DAL.Entities;

namespace PLMS.BLL.Mapper.Profiles
{
    public class TaskCommentProfileBll : Profile
    {
        public TaskCommentProfileBll() 
        {
            CreateMap<TaskCommentDto, TaskComment>()
                 .ForMember(dest => dest.Task, opt => opt.Ignore()).ReverseMap();
        }
    }
}
