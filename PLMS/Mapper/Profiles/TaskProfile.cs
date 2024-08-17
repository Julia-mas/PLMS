using AutoMapper;
using PLMS.API.Models.ModelsTasks;
using PLMS.BLL.DTO.TasksDto;

namespace PLMS.API.Mapper.Profiles
{
    public class TaskProfile: Profile
    {
        public TaskProfile() 
        {
            CreateMap<TaskBaseModel, TaskBaseDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<TaskBaseModel, AddTaskDto>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<TaskShortDto, TaskShortViewModel>();

            CreateMap<TaskFullDetailsDto, TaskFullDetailsViewModel>();

            CreateMap<TaskShortWithCommentsDto, TaskShortWithCommentsViewModel>();

            CreateMap<GetTaskDto, GetTaskViewModel>();
        }
    }
}
