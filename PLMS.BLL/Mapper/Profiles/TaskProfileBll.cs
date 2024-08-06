using AutoMapper;
using PLMS.BLL.DTO;
using Task = PLMS.DAL.Entities.Task;

namespace PLMS.BLL.Mapper.Profiles
{
    public class TaskProfileBll : Profile
    {
        public TaskProfileBll() 
        {
            CreateMap<AddTaskDto, Task>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Priority, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore()).ReverseMap();
            CreateMap<EditTaskDto, Task>()
                .ForMember(dest => dest.Title, opt => opt.Condition(src => src.Title != null))
                .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
                .ForMember(dest => dest.DueDate, opt => opt.Condition(src => src.DueDate != default))
                .ForMember(dest => dest.PriorityId, opt => opt.Condition(src => src.PriorityId != default))
                .ForMember(dest => dest.StatusId, opt => opt.Condition(src => src.StatusId != default))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Priority, opt => opt.Ignore()).ReverseMap();
        }
    }
}
