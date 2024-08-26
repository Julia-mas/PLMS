using PLMS.DAL.Entities;

namespace PLMS.BLL.ServicesInterfaces
{
    public interface IPermissionService
    {
        public Task<bool> HasPermissionToGoal(int goalId, string userId);

        public bool HasPermissionToGoal(Goal? goal, string userId);


        public Task<bool> HasPermissionToCategory(int categoryId, string userId);

        public bool HasPermissionToCategory(Category? category, string userId);


        public Task<bool> HasPermissionToGoalAndTask(int taskId, string userId);
    }
}
