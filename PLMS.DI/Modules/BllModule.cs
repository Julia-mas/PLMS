using Autofac;
using PLMS.BLL.ServicesImplementation;
using PLMS.BLL.ServicesInterfaces;

namespace PLMS.DI.Modules
{
    public class BllModule: Module
    {
        protected override void Load(ContainerBuilder builder) 
        {
            builder.RegisterType<PermissionService>().As<IPermissionService>().InstancePerLifetimeScope();
            builder.RegisterType<TaskService>().As<ITaskService>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<GoalService>().As<IGoalService>().InstancePerLifetimeScope();
            builder.RegisterType<GoalCommentService>().As<IGoalCommentService>().InstancePerLifetimeScope();
            builder.RegisterType<TaskCommentService>().As<ITaskCommentService>().InstancePerLifetimeScope();
        }
    }
}
