using PLMS.BLL.DTO;
using PLMS.BLL.Filters;

namespace PLMS.BLL.ServicesInterfaces
{
    public interface ITaskService
    {
        Task<TaskDto> GetTaskByIdAsync(int id);
        Task EditTaskAsync(TaskDto taskDto);
        Task DeleteTaskAsync(int id);
        Task AddTaskAsync(TaskDto taskDto);
        Task<IEnumerable<TaskDto>> GetFilteredShortTasksAsync(MyItemFilter filters, string sortField, string includeColumns);
        Task<IEnumerable<TaskDto>> GetFilteredShortWithCommentsAsync(MyItemFilter filters);
        Task<IEnumerable<TaskDto>> GetFilteredFullAsync(MyItemFilter filters);
    }
}
