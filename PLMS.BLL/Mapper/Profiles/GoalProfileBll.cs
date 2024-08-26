using AutoMapper;
using PLMS.BLL.DTO.GoalsDto;
using PLMS.DAL.Entities;

namespace PLMS.BLL.Mapper.Profiles
{
    public class GoalProfileBll : Profile
    {
        public GoalProfileBll() 
        {
            CreateMap<AddGoalDto, Goal>()
                .ForMember(dest => dest.Tasks, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Priority, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.GoalComments, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Goal, GetGoalDto>()
                .ForMember(dest => dest.CategoryTitle, source => source.MapFrom(s => s.Category.Title))
                .ForMember(dest => dest.PriorityTitle, source => source.MapFrom(s => s.Priority.Title))
                .ForMember(dest => dest.StatusTitle, source => source.MapFrom(s => s.Status.Title))
                .ForMember(dest => dest.TaskTitles, source => source.MapFrom(s => s.Tasks.Select(t => t.Title)));
        }
    }
}
