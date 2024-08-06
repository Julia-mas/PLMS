using PLMS.BLL.DTO;
using PLMS.BLL.Filters;

namespace PLMS.BLL.ServicesInterfaces
{
    public interface ITaskService
    {
        Task<GetTaskDto> GetTaskByIdAsync(int id, string userId);
        Task<AddTaskDto> GetTaskByIncludeObjectsIdAsync(int id);
        Task EditTaskAsync(EditTaskDto taskDto, int id, string userId);
        Task DeleteTaskAsync(int id);
        Task AddTaskAsync(AddTaskDto taskDto, string userId);
        Task<IEnumerable<TaskShortDto>> GetFilteredShortTasksAsync(TaskItemFilter filters);
        Task<IEnumerable<TaskShortWithCommentsDto>> GetFilteredShortWithCommentsAsync(TaskItemFilter filters);
        Task<IEnumerable<TaskFullDetailsDto>> GetFilteredFullAsync(TaskItemFilter filters);
    }
}
