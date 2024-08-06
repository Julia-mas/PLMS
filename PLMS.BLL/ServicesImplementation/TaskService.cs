using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PLMS.BLL.DTO;
using PLMS.BLL.Filters;
using PLMS.BLL.ServicesInterfaces;
using PLMS.DAL.Entities;
using PLMS.DAL.Interfaces;
using System.Linq.Expressions;
using Task = PLMS.DAL.Entities.Task;

namespace PLMS.BLL.ServicesImplementation
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async System.Threading.Tasks.Task AddTaskAsync(AddTaskDto taskDto, string userId)
        {
            var goalRepository = _unitOfWork.GetRepository<Goal>();
            var categoryRepository = _unitOfWork.GetRepository<Category>();
            var priorityRepository = _unitOfWork.GetRepository<Priority>();
            var statusRepository = _unitOfWork.GetRepository<Status>();
            var defaultPriority = await priorityRepository.GetByIdAsync(1);
            var defaultStatus = await statusRepository.GetByIdAsync(1);

            Goal goal;
            if (taskDto.GoalId != 0)
            {
                goal = await goalRepository.GetByIdAsync(taskDto.GoalId);
                goal ??= await MapToGoalAsync(taskDto, categoryRepository, priorityRepository, statusRepository, defaultPriority, defaultStatus, userId);
            }
            else
            {
                goal = await MapToGoalAsync(taskDto, categoryRepository, priorityRepository, statusRepository, defaultPriority, defaultStatus, userId);
            }

            var priority = await priorityRepository.GetByIdAsync(taskDto.PriorityId);
            priority ??= defaultPriority;

            var status = await statusRepository.GetByIdAsync(taskDto.StatusId);
            status ??= defaultStatus;

            Task task = new()
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                Goal = goal, 
                Priority = priority,
                Status = status,
                CreatedAt = DateTime.UtcNow,
                DueDate = taskDto.DueDate,
                TaskComments = taskDto.TaskComments?.Select(x => new TaskComment { Comment = x.Comment, CreatedAt = x.CreatedAt})?.ToArray()
                ?? Array.Empty<TaskComment>()
            };
            var taskRepository = _unitOfWork.GetRepository<Task>();
            await taskRepository.CreateAsync(task);
            await _unitOfWork.CommitChangesToDatabaseAsync();

            taskDto.Id = task.Id;
            taskDto.GoalId = goal.Id;
            taskDto.Goal.CategoryId = goal.Category.Id;
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
        {
            var taskRepository = _unitOfWork.GetRepository<Task>();
            Task task = await taskRepository.GetByIdAsync(id);

            if (task == null)
            {
                return;
            }

            taskRepository.Remove(task);
            await _unitOfWork.CommitChangesToDatabaseAsync();
        }

        public async System.Threading.Tasks.Task EditTaskAsync(EditTaskDto taskDto, int id, string userId)
        {
            var taskRepository = _unitOfWork.GetRepository<Task>();
            var goalRepository = _unitOfWork.GetRepository<Goal>();
            var task = await taskRepository.GetByIdAsync(id) ?? throw new Exception("Task not found");
            var goal = await goalRepository.GetByIdAsync(task.GoalId);

            if (goal.UserId != userId)
            {
                throw new UnauthorizedAccessException("This user is not unauthorized to access this task");
            }

            task.Title = taskDto.Title ?? task.Title;
            task.Description = taskDto.Description ?? task.Description;   
            task.DueDate = taskDto.DueDate != default ? taskDto.DueDate : task.DueDate;
            task.StatusId = taskDto.StatusId != default ? taskDto.StatusId : task.StatusId;
            task.PriorityId = taskDto.PriorityId != default ? taskDto.PriorityId : task.PriorityId;
            task.GoalId = taskDto.GoalId != default ? taskDto.GoalId : task.GoalId;

            if (taskDto.TaskComments != null && taskDto.TaskComments.Any())
            {
                foreach (var taskCommentDto in taskDto.TaskComments)
                {
                    var existingComment = task.TaskComments.FirstOrDefault(tc => tc.Id == taskCommentDto.Id);
                    if (existingComment == null)
                    {
                        task.TaskComments.Add(new TaskComment { Comment = taskCommentDto.Comment, CreatedAt = taskCommentDto.CreatedAt });
                    }
                }
            }

            taskRepository.Update(task);
            await _unitOfWork.CommitChangesToDatabaseAsync();
        }

        public async Task<IEnumerable<TaskFullDetailsDto>> GetFilteredFullAsync(TaskItemFilter filters)
        {
            var tasks = await GetFilteredTasksAsync(filters);

            if (tasks == null || !tasks.Any())
            {
                return Enumerable.Empty<TaskFullDetailsDto>();
            }

            var results = tasks.Select(t => 
            new TaskFullDetailsDto
            {
                Title = t.Title,
                Description = t.Description ?? string.Empty,
                GoalTitle = t.Goal?.Title ?? string.Empty,
                CategoryTitle = t.Goal?.Category?.Title ?? string.Empty,
                TaskComments = t.TaskComments?.Select(tс => new TaskCommentDto
                {
                    Comment = tс.Comment,
                    CreatedAt = tс.CreatedAt,
                    TaskId = tс.TaskId
                })?.ToArray() ?? Array.Empty<TaskCommentDto>(),
                PriorityTitle = t.Priority.Title,
                StatusTitle = t.Status.Title,
                CreatedAt = t.CreatedAt,
                DueDate = t.DueDate
            }).ToArray();

            return results;
        }

        public async Task<IEnumerable<TaskShortDto>> GetFilteredShortTasksAsync(TaskItemFilter filters)
        {
            var tasks = await GetFilteredTasksAsync(filters);

            if (tasks == null || !tasks.Any())
            {
                return Enumerable.Empty<TaskShortDto>();
            }

            IEnumerable<TaskShortDto> results = tasks.Select(t =>
            new TaskShortDto
            {
                Title = t.Title,
                DueDate = t.DueDate,
                CreatedAt = t.CreatedAt
            }).ToArray();

            return results;
        }

        public async Task<IEnumerable<TaskShortWithCommentsDto>> GetFilteredShortWithCommentsAsync(TaskItemFilter filters)
        {
            var tasks = await GetFilteredTasksAsync(filters);

            if (tasks == null || !tasks.Any())
            {
                return Enumerable.Empty<TaskShortWithCommentsDto>();
            }

            var results = tasks.Select(t => new TaskShortWithCommentsDto
            {
                Title = t.Title,
                GoalTitle = t.Goal?.Title ?? string.Empty,
                TaskComments = t.TaskComments?.Select(tс => new TaskCommentDto 
                { 
                    Comment = tс.Comment, 
                    CreatedAt = tс.CreatedAt
                })?
                .ToArray() ?? Array.Empty<TaskCommentDto>(),
                CreatedAt = t.CreatedAt
            }).ToArray();

            return results;
        }

        public async Task<GetTaskDto> GetTaskByIdAsync(int id, string userId)
        {
            var taskRepository = _unitOfWork.GetRepository<Task>();
            var goalRepository = _unitOfWork.GetRepository<Goal>();
            var task = await taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return new GetTaskDto();
            }

            var goal = await goalRepository.GetByIdAsync(task.GoalId);
            if (goal.UserId != userId)
            {
                throw new UnauthorizedAccessException("This user is not unauthorized to access this task");
            }

            var taskDto = new GetTaskDto()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                GoalId = task.GoalId,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate,
                PriorityId = task.PriorityId,
                StatusId = task.StatusId
            };

            return taskDto;
        }

        public async Task<AddTaskDto> GetTaskByIncludeObjectsIdAsync(int id)
        {
            var taskRepository = _unitOfWork.GetRepository<Task>();
            var task = await taskRepository.GetByPredicateAsync(t => t.Id.Equals(id),
                t => t.Priority,
                t => t.Status,
                t => t.Goal,
                t => t.Goal.Priority,
                t => t.Goal.Status,
                t => t.Goal.Category,
                t => t.TaskComments);

            AddTaskDto taskDto = new()
            {
                Title = task.Title,
                Description = task.Description ?? string.Empty,
                GoalId = task.Goal?.Id ?? 0,
                TaskComments = task.TaskComments?.Select(
                    t => new TaskCommentDto
                    {
                        Comment = t.Comment,
                        CreatedAt = t.CreatedAt
                    })?.ToArray() ?? Array.Empty<TaskCommentDto>(),
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate,
                PriorityId = task.Priority.Id,
                StatusId = task.Status.Id,
            };

            return taskDto;
        }

        private async Task<Goal> MapToGoalAsync(AddTaskDto taskDto, IRepository<Category> categoryRepository, IRepository<Priority> priorityRepository, IRepository<Status> statusRepository,
            Priority defaultPriority, Status defaultStatus, string userId)
        {
            taskDto.Goal.UserId = userId;
            Goal goal = _mapper.Map<Goal>(taskDto.Goal);
            var category = await categoryRepository.GetByIdAsync(taskDto.Goal.CategoryId);
            if (category == null)
            {
                taskDto.Goal.Category.UserId = userId;
                category = _mapper.Map<Category>(taskDto.Goal.Category);
            }
            goal.Category = category;
            goal.Priority = await priorityRepository.GetByIdAsync(taskDto.Goal.PriorityId) ?? defaultPriority;
            goal.Status = await statusRepository.GetByIdAsync(taskDto.Goal.StatusId) ?? defaultStatus;

            return goal;
        }

        private async Task<IEnumerable<Task>> GetFilteredTasksAsync(TaskItemFilter filters)
        {
            var filterExpression = ApplyFilters(filters);
            var orderFunction = ApplyOrder(filters);

            // Apply filtation and sorting on the DB level.
            var taskRepository = _unitOfWork.GetRepository<Task>();
            var query = taskRepository.GetAll()
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

        private static Func<IQueryable<Task>, IOrderedQueryable<Task>> ApplyOrder(TaskItemFilter filters)
        {
            return q =>
            {
                switch (filters.SortField)
                {
                    case TaskItemFilter.TaskItemSortFields.Priority:
                        return filters.SortOrder == BaseFilter.SortOrders.Asc ? q.OrderBy(t => t.Priority.Title) : q.OrderByDescending(t => t.Priority.Title);
                    case TaskItemFilter.TaskItemSortFields.Status:
                        return filters.SortOrder == BaseFilter.SortOrders.Asc ? q.OrderBy(t => t.Status.Title) : q.OrderByDescending(t => t.Status.Title);
                    case TaskItemFilter.TaskItemSortFields.DueDate:
                        return filters.SortOrder == BaseFilter.SortOrders.Asc ? q.OrderBy(t => t.DueDate) : q.OrderByDescending(t => t.DueDate);
                    case TaskItemFilter.TaskItemSortFields.CreatedAt:
                        return filters.SortOrder == BaseFilter.SortOrders.Asc ? q.OrderBy(t => t.CreatedAt) : q.OrderByDescending(t => t.CreatedAt);
                    case TaskItemFilter.TaskItemSortFields.Category:
                        return filters.SortOrder == BaseFilter.SortOrders.Asc ? q.OrderBy(t => t.Goal.CategoryId) : q.OrderByDescending(t => t.Goal.CategoryId);
                    default:
                        return q.OrderByDescending(t => t.Priority.Title);
                }
            };
        }

        private static Expression<Func<Task, bool>> ApplyFilters(TaskItemFilter filters)
        {
            return t => (t.Goal.UserId.Contains(filters.UserId))
                && (filters.PriorityIds == null || filters.PriorityIds.Contains(t.PriorityId))
                && (filters.StatusIds == null || filters.StatusIds.Contains(t.StatusId))
                && (filters.GoalIds == null || filters.GoalIds.Contains(t.GoalId.ToString()))
                && (filters.CategoryIds == null || filters.CategoryIds.Contains(t.Goal.CategoryId.ToString())); ;
        }
    }
}
