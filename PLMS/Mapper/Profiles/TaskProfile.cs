using AutoMapper;
using PLMS.API.Models.ModelsTasks;
using PLMS.BLL.DTO;

namespace PLMS.API.Mapper.Profiles
{
    public class TaskProfile: Profile
    {
        public TaskProfile() 
        {
            CreateMap<EditTaskDto, EditTaskModel>().ReverseMap();
            CreateMap<AddTaskModel, AddTaskDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<TaskShortDto, TaskShortModel>().ReverseMap();
            CreateMap<TaskFullDetailsDto, TaskFullDetailsModel>().ReverseMap();
            CreateMap<TaskShortWithCommentsDto, TaskShortWithCommentsModel>().ReverseMap();
            CreateMap<GetTaskDto, GetTaskModel>().ReverseMap();
        }
    }
}
