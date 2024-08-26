using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PLMS.BLL.DTO.TasksDto;
using PLMS.BLL.Filters;
using PLMS.BLL.ServicesInterfaces;
using PLMS.Common.Exceptions;
using PLMS.DAL.Interfaces;
using System.Linq.Expressions;
using Task = PLMS.DAL.Entities.Task;

namespace PLMS.BLL.ServicesImplementation
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        private readonly IRepository<Task> _taskRepository;
        private readonly IPermissionService _permissionService;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper, IPermissionService permissionService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _taskRepository = _unitOfWork.GetRepository<Task>();
            _permissionService = permissionService;
        }

        public async Task<int> AddTaskAsync(AddTaskDto taskDto)
        {
            if (!await _permissionService.HasPermissionToGoal(taskDto.GoalId, taskDto.UserId))
            {
                throw new NotFoundException("Goal was not found.");
            }

            var task = _mapper.Map<Task>(taskDto);
            await _taskRepository.CreateAsync(task);
            await _unitOfWork.CommitChangesToDatabaseAsync();

            return task.Id;
        }

        public async System.Threading.Tasks.Task EditTaskAsync(EditTaskDto taskDto, string userId)
        {
            var task = await _taskRepository.GetByPredicateAsync(t => t.Id == taskDto.Id, t => t.Goal) ?? throw new NotFoundException("Task was not found");

            if (!_permissionService.HasPermissionToGoal(task.Goal, userId))
            {
                throw new NotFoundException("Task was not found.");
            }

            task.Title = taskDto.Title != default ? taskDto.Title : task.Title;
            task.Description = taskDto.Description ?? task.Description;
            task.DueDate = taskDto.DueDate != default ? taskDto.DueDate : task.DueDate;
            task.StatusId = taskDto.StatusId != default ? taskDto.StatusId : task.StatusId;
            task.PriorityId = taskDto.PriorityId != default ? taskDto.PriorityId : task.PriorityId;
            task.GoalId = taskDto.GoalId != default ? taskDto.GoalId : task.GoalId;

            _taskRepository.Update(task);
            await _unitOfWork.CommitChangesToDatabaseAsync();
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(int id, string userId)
        {
            Task? task = await _taskRepository.GetByPredicateAsync(t => t.Id == id, t => t.Goal);

            if (task == null)
            {
                return;
            }

            if (!_permissionService.HasPermissionToGoal(task.Goal, userId))
            {
                throw new NotFoundException("Goal was not found.");
            }

            _taskRepository.Remove(task);
            await _unitOfWork.CommitChangesToDatabaseAsync();
        }

        public async Task<IEnumerable<TaskFullDetailsDto>> GetFilteredFullAsync(TaskFilter filters)
        {
            var tasks = await GetFilteredTasksAsync(filters);

            if (tasks == null || !tasks.Any())
            {
                return Enumerable.Empty<TaskFullDetailsDto>();
            }

            var fullTasks = _mapper.Map<Task[], TaskFullDetailsDto[]>(tasks.ToArray());

            return fullTasks;
        }

        public async Task<IEnumerable<TaskShortDto>> GetFilteredShortTasksAsync(TaskFilter filters)
        {
            var tasks = await GetFilteredTasksAsync(filters);

            if (tasks == null || !tasks.Any())
            {
                return Enumerable.Empty<TaskShortDto>();
            }

            var shortTasks = _mapper.Map<Task[], TaskShortDto[]>(tasks.ToArray());
            
            return shortTasks;
        }

        public async Task<IEnumerable<TaskShortWithCommentsDto>> GetFilteredShortWithCommentsAsync(TaskFilter filters)
        {
            var tasks = await GetFilteredTasksAsync(filters);

            if (tasks == null || !tasks.Any())
            {
                return Enumerable.Empty<TaskShortWithCommentsDto>();
            }

            var shortTasksWithComments = _mapper.Map<Task[], TaskShortWithCommentsDto[]>(tasks.ToArray());

            return shortTasksWithComments;
        }

        public async Task<GetTaskDto> GetTaskByIdAsync(int id, string userId)
        {
            var task = await _taskRepository.GetByPredicateAsync(t => t.Id == id, t => t.Goal, t => t.Status, t => t.Priority) ?? throw new NotFoundException("Task was not found");

            if (!_permissionService.HasPermissionToGoal(task.Goal, userId))
            {
                throw new NotFoundException("Task was not found.");
            }

            var taskDto = _mapper.Map<GetTaskDto>(task);

            return taskDto;
        }

        public async Task<AddTaskDto> GetTaskByIncludeObjectsIdAsync(int id)
        {
            var task = await _taskRepository.GetByPredicateAsync(t => t.Id.Equals(id),
                t => t.Priority,
                t => t.Status,
                t => t.Goal,
                t => t.Goal.Priority,
                t => t.Goal.Status,
                t => t.Goal.Category,
                t => t.TaskComments);

            var taskDto = _mapper.Map<AddTaskDto>(task);
            return taskDto;
        }

        private async Task<IEnumerable<Task>> GetFilteredTasksAsync(TaskFilter filters)
        {
            var filterExpression = ApplyFilters(filters);
            var orderFunction = ApplyOrder(filters);

            // Apply filtation and sorting on the DB level.
            var query = _taskRepository.GetAll()
                                       .Include(t => t.Goal)
                                       .ThenInclude(g => g.Category)
                                       .Include(t => t.Goal)
                                       .ThenInclude(g => g.GoalComments)
                                       .Include(t => t.Priority)
                                       .Include(t => t.Status)
                                       .Include(t => t.TaskComments)
                                       .Where(filterExpression);

            var orderedQuery = orderFunction(query);

            // Pagination
            var paginatedQuery = orderedQuery.Skip((filters.PageNumber - 1) * filters.ItemsPerPageCount)
                                             .Take(filters.ItemsPerPageCount);

            // Call to the DB
            var tasks = await paginatedQuery.ToListAsync();

            return tasks;
        }

        private static Func<IQueryable<Task>, IOrderedQueryable<Task>> ApplyOrder(TaskFilter filters)
        {
            return q =>
            {
                switch (filters.SortField)
                {
                    case TaskFilter.TaskSortFields.Priority:
                        return filters.SortOrder == BaseFilter.SortOrders.Asc ? q.OrderBy(t => t.Priority.Title) : q.OrderByDescending(t => t.Priority.Title);
                    case TaskFilter.TaskSortFields.Status:
                        return filters.SortOrder == BaseFilter.SortOrders.Asc ? q.OrderBy(t => t.Status.Title) : q.OrderByDescending(t => t.Status.Title);
                    case TaskFilter.TaskSortFields.DueDate:
                        return filters.SortOrder == BaseFilter.SortOrders.Asc ? q.OrderBy(t => t.DueDate) : q.OrderByDescending(t => t.DueDate);
                    case TaskFilter.TaskSortFields.CreatedAt:
                        return filters.SortOrder == BaseFilter.SortOrders.Asc ? q.OrderBy(t => t.CreatedAt) : q.OrderByDescending(t => t.CreatedAt);
                    case TaskFilter.TaskSortFields.Category:
                        return filters.SortOrder == BaseFilter.SortOrders.Asc ? q.OrderBy(t => t.Goal.CategoryId) : q.OrderByDescending(t => t.Goal.CategoryId);
                    default:
                        return q.OrderByDescending(t => t.Priority.Title);
                }
            };
        }

        private static Expression<Func<Task, bool>> ApplyFilters(TaskFilter filters)
        {
            return t => (t.Goal.UserId.Contains(filters.UserId))
                && (filters.PriorityIds == null || filters.PriorityIds.Contains(t.PriorityId))
                && (filters.StatusIds == null || filters.StatusIds.Contains(t.StatusId))
                && (filters.GoalIds == null || filters.GoalIds.Contains(t.GoalId.ToString()))
                && (filters.CategoryIds == null || filters.CategoryIds.Contains(t.Goal.CategoryId.ToString())); ;
        }
    }
}
