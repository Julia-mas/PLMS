using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PLMS.BLL.DTO.GoalsDto;
using PLMS.BLL.Filters;
using PLMS.BLL.ServicesInterfaces;
using PLMS.Common.Enums;
using PLMS.Common.Exceptions;
using PLMS.DAL.Entities;
using PLMS.DAL.Interfaces;
using System.Linq.Expressions;
using Task = System.Threading.Tasks.Task;

namespace PLMS.BLL.ServicesImplementation
{
    public class GoalService : IGoalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Goal> _goalRepository;
        private readonly IPermissionService _permissionService;

        public GoalService(IUnitOfWork unitOfWork, IMapper mapper, IPermissionService permissionService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _goalRepository = _unitOfWork.GetRepository<Goal>();
            _permissionService = permissionService;
        }

        public async Task<int> AddGoalAsync(AddGoalDto goalDto)
        {
            var goal = await _goalRepository.GetByPredicateAsync(g => g.Title == goalDto.Title && g.UserId == goalDto.UserId);
            if (goal != null)
            {
                return goal.Id;
            }

            if (!await _permissionService.HasPermissionToCategory(goalDto.CategoryId, goalDto.UserId))
            {
                throw new NotFoundException("Category was not found.");
            }

            goal = _mapper.Map<Goal>(goalDto);
            await _goalRepository.CreateAsync(goal);
            await _unitOfWork.CommitChangesToDatabaseAsync();

            return goal.Id;
        }

        public async Task EditGoalAsync(EditGoalDto goalDto, string userId)
        {
            var goal = await _goalRepository.GetByPredicateAsync(t => t.Id == goalDto.Id, t => t.Category);
            if (goal == null || !await _permissionService.HasPermissionToCategory(goal.CategoryId, userId))
            {
                throw new NotFoundException("Goal was not found.");
            }

            goal.Title = goalDto.Title != default ? goalDto.Title : goal.Title;
            goal.Description = goalDto.Description ?? goal.Description;
            goal.DueDate = goalDto.DueDate != default ? goalDto.DueDate : goal.DueDate;
            goal.StatusId = goalDto.StatusId != default ? goalDto.StatusId : goal.StatusId;
            goal.PriorityId = goalDto.PriorityId != default ? goalDto.PriorityId : goal.PriorityId;
            goal.CategoryId = goalDto.CategoryId != default ? goalDto.CategoryId : goal.CategoryId;

            _goalRepository.Update(goal);
            await _unitOfWork.CommitChangesToDatabaseAsync();
        }

        public async Task DeleteGoalAsync(int id, string userId)
        {
            Goal? goal = await _goalRepository.GetByPredicateAsync(g => g.Id == id);
            if (goal == null || !await _permissionService.HasPermissionToCategory(goal.CategoryId, userId))
            {
                return;
            }

            _goalRepository.Remove(goal);
            await _unitOfWork.CommitChangesToDatabaseAsync();
        }

        public async Task<GetGoalDto> GetGoalByIdAsync(int id, string userId)
        {
            var goal = await _goalRepository.GetByPredicateAsync(g => g.Id == id, g => g.Category, g => g.Status, g => g.Priority, g => g.Tasks);
            if (goal == null || !_permissionService.HasPermissionToCategory(goal.Category, userId))
            {
                throw new NotFoundException("Goal was not found.");
            }

            var goalDto = _mapper.Map<GetGoalDto>(goal);

            return goalDto;
        }

        public async Task<GoalCompletionInfoDto> GetTaskCompletionPercentageAsync(string userId)
        {
            var taskCompletionPercentage = await _goalRepository.GetAll().Where(g => g.UserId == userId).Select(g => new 
            GoalCompletionInfoDto.GoalInfoDto
            {
                GoalId = g.Id,
                GoalTitle = g.Title,
                CompletionPercentage = g.Tasks.Any() ? (g.Tasks.Count(t => t.StatusId == (int)StatusEnum.Completed) * 100.0) / g.Tasks.Count() : 0.0
            }).ToListAsync();

            return new GoalCompletionInfoDto { Goals = taskCompletionPercentage };
        }

        public async Task<List<GetGoalDto>> GetFilteredGoalsAsync(GoalFilter filters)
        {
            var filterExpression = ApplyFilters(filters);
            var orderFunction = ApplyOrder(filters);

            // Apply filtation and sorting on the DB level.
            var query = _goalRepository.GetAll()
                                       .Include(t => t.Category)
                                       .Include(g => g.GoalComments)
                                       .Include(t => t.Priority)
                                       .Include(t => t.Status)
                                       .Where(filterExpression);

            var orderedQuery = orderFunction(query);

            // Pagination
            var paginatedQuery = orderedQuery.Skip((filters.PageNumber - 1) * filters.ItemsPerPageCount)
                                             .Take(filters.ItemsPerPageCount);

            // Call to the DB
            var goals = await paginatedQuery.ToListAsync();

            var goalsDto = _mapper.Map<List<GetGoalDto>>(goals);
            goalsDto.ForEach(g => g.GoalComments.Sort());

            return goalsDto;
        }


        private static Func<IQueryable<Goal>, IOrderedQueryable<Goal>> ApplyOrder(GoalFilter filters)
        {
            return q =>
            {
                switch (filters.SortField)
                {
                    case GoalFilter.GoalSortFields.Priority:
                        return filters.SortOrder == BaseFilter.SortOrders.Asc ? q.OrderBy(g => g.Priority.Title) : q.OrderByDescending(g => g.Priority.Title);
                    case GoalFilter.GoalSortFields.Status:
                        return filters.SortOrder == BaseFilter.SortOrders.Asc ? q.OrderBy(g => g.Status.Title) : q.OrderByDescending(g => g.Status.Title);
                    case GoalFilter.GoalSortFields.DueDate:
                        return filters.SortOrder == BaseFilter.SortOrders.Asc ? q.OrderBy(g => g.DueDate) : q.OrderByDescending(g => g.DueDate);
                    case GoalFilter.GoalSortFields.CreatedAt:
                        return filters.SortOrder == BaseFilter.SortOrders.Asc ? q.OrderBy(g => g.CreatedAt) : q.OrderByDescending(g => g.CreatedAt);
                    case GoalFilter.GoalSortFields.Category:
                        return filters.SortOrder == BaseFilter.SortOrders.Asc ? q.OrderBy(g => g.CategoryId) : q.OrderByDescending(g => g.CategoryId);
                    default:
                        return q.OrderByDescending(g => g.Priority.Title);
                }
            };
        }

        private static Expression<Func<Goal, bool>> ApplyFilters(GoalFilter filters)
        {
            return g => (g.UserId.Contains(filters.UserId))
                && (filters.PriorityIds == null || filters.PriorityIds.Contains(g.PriorityId))
                && (filters.StatusIds == null || filters.StatusIds.Contains(g.StatusId))
                && (filters.CategoryIds == null || filters.CategoryIds.Contains(g.CategoryId.ToString()));
        }
    }
}
