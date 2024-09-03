using AutoMapper;
using PLMS.BLL.DTO.TaskCommentsDto;
using PLMS.DAL.Entities;

namespace PLMS.BLL.Mapper.Profiles
{
    public class TaskCommentProfileBll : Profile
    {
        public TaskCommentProfileBll() 
        {
            CreateMap<AddTaskCommentDto, TaskComment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Task, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<TaskComment, GetTaskCommentDto>();
        }
    }
}
