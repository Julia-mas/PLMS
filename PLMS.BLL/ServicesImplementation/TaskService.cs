using PLMS.BLL.DTO;
using PLMS.BLL.ServicesInterfaces;
using PLMS.DAL.Entities;
using PLMS.BLL.Filters;
using PLMS.DAL.Interfaces;
using Task = PLMS.DAL.Entities.Task;

namespace PLMS.BLL.ServicesImplementation
{
    public class TaskService : ITaskService
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IRepository<Task> _taskRepository;

        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _taskRepository = _unitOfWork.GetRepository<Task>();
        }
        public async System.Threading.Tasks.Task AddTaskAsync(TaskDto taskDto)
        {
            Task task = new()
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                //Goal = taskDto.GoalDto,
                //Category = taskDto.CategoryDto,
                //Priority = taskDto.PriorityDto
                CreatedAt = taskDto.CreatedAt,
                DueDate = taskDto.DueDate,
                TaskComments = taskDto.Comments.Select(x => new TaskComment { Comment = x.Comment, CreatedAt = x.CreatedAt}).ToArray()
            };
            _taskRepository.Create(task);
            await _unitOfWork.CommitChangesToDatabaseAsync();
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
        {
            Task task = await _taskRepository.GetByIdAsync(id);

            if (task == null) 
            {
                return;
            }

            _taskRepository.Remove(task);
            await _unitOfWork.CommitChangesToDatabaseAsync();
        }

        public async System.Threading.Tasks.Task EditTaskAsync(TaskDto taskDto)
        {
            Task task = await _taskRepository.GetByIdAsync(taskDto.Id);
            _taskRepository.Update(task);
            await _unitOfWork.CommitChangesToDatabaseAsync();
        }

        public async Task<IEnumerable<TaskDto>> GetFilteredFullAsync(MyItemFilter filters)
        {
            IEnumerable<Task> tasks = await _taskRepository.GetFilteredAsync(
                t => (filters.PriorityIds == null || filters.PriorityIds.Contains(t.PriorityId)) &&
                (filters.StatusIds == null || filters.StatusIds.Contains(t.StatusId)) &&
                (filters.GoalIds == null || filters.GoalIds.Contains(t.GoalId.ToString())) &&
                (filters.CategoryIds == null || filters.CategoryIds.Contains(t.GoalId.ToString())),
                q => q.OrderBy(t => t.DueDate),
                "TaskComments,Goal,Priority,Status");

            if (!tasks.Any())
            {
                return Enumerable.Empty<TaskDto>();
            }

            if (tasks.Count() < filters.ItemsPerPageCount)
            {
                filters.ItemsPerPageCount = tasks.Count();
            }

            var results = tasks.Take(filters.ItemsPerPageCount).Select(t =>
            new TaskDto
            {
                Title = t.Title,
                Description = t.Description,
                GoalTitle = t.Goal.Title,
                CategoryId = t.Goal.Category.Id,
                Comments = t.TaskComments.Select(tс => new  TaskCommentDto
                { 
                    Comment = tс.Comment, 
                    CreatedAt = tс.CreatedAt 
                }).ToArray(),
                PriorityId = t.Priority.Id,
                StatusId = t.Status.Id,
                CreatedAt = t.CreatedAt,
                DueDate = t.DueDate
            }).ToArray();

            return results;
        }

        public async Task<IEnumerable<TaskDto>> GetFilteredShortTasksAsync(MyItemFilter filters, string sortField, string includeColumns)
        {
            Func<IQueryable<Task>, IOrderedQueryable<Task>>? orderBy = null;
            if (sortField == "CreatedAt")
            {
                orderBy = q => q.OrderByDescending(t => t.CreatedAt);
            }
            else
            {
                orderBy = q => q.OrderBy(t => t.DueDate);
            }

            IEnumerable<Task> tasks = await _taskRepository.GetFilteredAsync(
                t => (filters.PriorityIds == null || filters.PriorityIds.Contains(t.PriorityId)) &&
                (filters.StatusIds == null || filters.StatusIds.Contains(t.StatusId)),
                orderBy,
                "");

            if (!tasks.Any()) 
            {
                return Enumerable.Empty<TaskDto>();
            }

            if (tasks.Count() < filters.ItemsPerPageCount)
            {
                filters.ItemsPerPageCount = tasks.Count();
            }

            IEnumerable<TaskDto> results = tasks.Take(filters.ItemsPerPageCount).Select(t =>
            new TaskDto
            {
                Title = t.Title,
                DueDate = t.DueDate,
                CreatedAt = t.CreatedAt
            }).ToArray(); ;

            return results;
        }

        public async Task<IEnumerable<TaskDto>> GetFilteredShortWithCommentsAsync(MyItemFilter filters)
        {
            IEnumerable<Task> tasks = await _taskRepository.GetFilteredAsync(
                t => (filters.PriorityIds == null || filters.PriorityIds.Contains(t.PriorityId)) &&
                (filters.StatusIds == null || filters.StatusIds.Contains(t.StatusId)),
                q => q.OrderByDescending(t => t.TaskComments.Select(tc => tc.CreatedAt)),
                "TaskComments,Goal");

            if (!tasks.Any())
            {
                return Enumerable.Empty<TaskDto>();
            }

            if (tasks.Count() < filters.ItemsPerPageCount)
            {
                filters.ItemsPerPageCount = tasks.Count();
            }

            var results = tasks.Take(filters.ItemsPerPageCount).Select(t =>
            new TaskDto
            {
                Title = t.Title,
                GoalTitle = t.Goal.Title,
                Comments = t.TaskComments.Select(tс => new TaskCommentDto 
                { 
                    Comment = tс.Comment, 
                    CreatedAt = tс.CreatedAt 
                }).ToArray()

            }).ToArray();

            return results;
        }

        public async Task<TaskDto> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            TaskDto taskDto = new()
            {
                Title = task.Title,
                Description = task.Description,
                GoalTitle = task.Goal.Title,
                Comments = task.TaskComments.Select(
                    t => new TaskCommentDto
                    {
                        Comment = t.Comment,
                        CreatedAt = t.CreatedAt
                    }),
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate,
                PriorityId = task.Priority.Id,
                StatusId = task.Status.Id,
                CategoryId = task.Goal.Category.Id
            };

            return taskDto;
        }
    }
}
