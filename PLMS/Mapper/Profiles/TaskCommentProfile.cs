using AutoMapper;
using PLMS.API.Models.ModelsTaskComments;
using PLMS.BLL.DTO.TaskCommentsDto;

namespace PLMS.API.Mapper.Profiles
{
    public class TaskCommentProfile: Profile
    {
        public TaskCommentProfile() 
        {
            CreateMap<TaskCommentModel, EditTaskCommentDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<TaskCommentModel, AddTaskCommentDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<GetTaskCommentDto, GetTaskCommentViewModel>();
        }
    }
}
