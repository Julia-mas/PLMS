using PLMS.BLL.ServicesInterfaces;
using PLMS.DAL.Entities;
using PLMS.DAL.Interfaces;

namespace PLMS.BLL.ServicesImplementation
{
    public class PermissionService : IPermissionService
    {
        private readonly IRepository<DAL.Entities.Task> _taskRepository;
        private readonly IRepository<Goal> _goalRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _taskRepository = _unitOfWork.GetRepository<DAL.Entities.Task>();
            _goalRepository = _unitOfWork.GetRepository<Goal>();
            _categoryRepository = _unitOfWork.GetRepository<Category>();
        }

        public async Task<bool> HasPermissionToGoal(int goalId, string userId)
        {
            var goal = await _goalRepository.GetByIdAsync(goalId);

            return HasPermissionToGoal(goal, userId);
        }

        public bool HasPermissionToGoal(Goal? goal, string userId)
        {
            return goal != null && goal.UserId == userId;
        }

        public async Task<bool> HasPermissionToCategory(int categoryId, string userId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            return HasPermissionToCategory(category, userId);
        }

        public bool HasPermissionToCategory(Category? category, string userId)
        {
            return category != null && category.UserId == userId;
        }

        public async Task<bool> HasPermissionToGoalAndTask(int taskId, string userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) 
            {
                return false;
            }

            return await HasPermissionToGoal(task.GoalId, userId);
        }
    }
}
