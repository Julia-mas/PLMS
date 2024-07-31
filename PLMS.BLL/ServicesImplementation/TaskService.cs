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
        public async System.Threading.Tasks.Task AddTaskAsync(TaskDto taskDto)//ToDo: should be a separate DTO class for adding Task
        {
            Task task = new()
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                //GoalId = taskDto.GoalId,//you can use this for linking existing Goal
                //Goal = taskDto.GoalDto, //you can use this for creating a new Goal, don't forget to map it from DTO to EF entity
                //Category = taskDto.CategoryDto,//same
                //Priority = taskDto.PriorityDto//same
                CreatedAt = taskDto.CreatedAt,//TODO: User should not be able to set this field, it should not be present in the DTO, it should be autofilled here instead
                DueDate = taskDto.DueDate,
                TaskComments = taskDto.Comments.Select(x => new TaskComment { Comment = x.Comment, CreatedAt = x.CreatedAt}).ToArray()
            };
            await _taskRepository.CreateAsync(task);
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

        public async System.Threading.Tasks.Task EditTaskAsync(TaskDto taskDto)//ToDo: should be a separate DTO class for editing Task
        {
            Task task = await _taskRepository.GetByIdAsync(taskDto.Id);
            _taskRepository.Update(task);
            await _unitOfWork.CommitChangesToDatabaseAsync();
        }

        public async Task<IEnumerable<TaskDto>> GetFilteredFullAsync(MyItemFilter filters)
        {
            IEnumerable<Task> tasks = await _taskRepository.GetFilteredAsync(
                t => (filters.PriorityIds == null || filters.PriorityIds.Contains(t.PriorityId)) //todo: formatting + mb move it into a separate method
                    && (filters.StatusIds == null || filters.StatusIds.Contains(t.StatusId)) &&
                (filters.GoalIds == null || filters.GoalIds.Contains(t.GoalId.ToString())) &&
                (filters.CategoryIds == null || filters.CategoryIds.Contains(t.GoalId.ToString())),
                q => q.OrderBy(t => t.DueDate),//ToDo: add full implementation according to the filter & move it into a separate method; This method should work with and return IQueryable
                "TaskComments,Goal,Priority,Status");

            //TODO: now apply skip/take here; you should apply it on IQueryable, then you can apply ToArray()

            if (tasks == null || !tasks.Any())
            {
                return Enumerable.Empty<TaskDto>();
            }

            if (tasks.Count() < filters.ItemsPerPageCount)//Todo: simply remove this block
            {
                filters.ItemsPerPageCount = tasks.Count();//don't change input params inside a function
            }

            var results = tasks.Take(filters.ItemsPerPageCount).Select(t =>//ToDo: remove Take here, just leave mapping; mapping can be moved into a separate method for clarity
            new TaskDto
            {
                Title = t.Title,
                Description = t.Description,
                GoalTitle = t.Goal.Title,//ToDo: cover null-ref case (if task doesn't have a goal); you can use null propagation operator
                CategoryId = t.Goal.Category.Id,//ToDo: cover null-ref case (if task doesn't have a category
                Comments = t.TaskComments.Select(tс => new TaskCommentDto//ToDo: cover null-ref case (if task doesn't have the comments
                { 
                    Comment = tс.Comment, 
                    CreatedAt = tс.CreatedAt 
                }).ToArray(),
                PriorityId = t.Priority.Id,//ToDo: cover null-ref case (if task doesn't have a priority (if that's possible)
                StatusId = t.Status.Id,//ToDo: cover null-ref case (if task doesn't have a status (if that's possible)
                CreatedAt = t.CreatedAt,
                DueDate = t.DueDate
            }).ToArray();

            return results;
        }

        //ToDo: apply the same as in the method above, plus remove sortField, includeColumns;
        //you can reuse ApplyFilter, ApplySorting as in the method above
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

        //ToDo: apply the same as in the method above
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

            //ToDo: all null checking for entire task object as well as for some fields like Priority, Status, Goal, etc

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
