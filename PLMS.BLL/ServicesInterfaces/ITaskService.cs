using PLMS.BLL.DTO;
using PLMS.BLL.Filters;

namespace PLMS.BLL.ServicesInterfaces
{
    public interface ITaskService
    {
        Task<GetTaskDto> GetTaskByIdAsync(int id, string userId);
        Task<AddTaskDto> GetTaskByIncludeObjectsIdAsync(int id);
        Task EditTaskAsync(EditTaskDto taskDto, int id, string userId);
        Task DeleteTaskAsync(int id, string userId);
        Task<int> AddTaskAsync(AddTaskDto taskDto);
        Task<IEnumerable<TaskShortDto>> GetFilteredShortTasksAsync(TaskFilter filters);
        Task<IEnumerable<TaskShortWithCommentsDto>> GetFilteredShortWithCommentsAsync(TaskFilter filters);
        Task<IEnumerable<TaskFullDetailsDto>> GetFilteredFullAsync(TaskFilter filters);
    }
}
