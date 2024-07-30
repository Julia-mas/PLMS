using Autofac;
using PLMS.BLL.ServicesImplementation;
using PLMS.BLL.ServicesInterfaces;

namespace PLMS.DI.Modules
{
    public class BllModule: Module
    {
        protected override void Load(ContainerBuilder builder) 
        {
            builder.RegisterType<TaskService>().As<ITaskService>().InstancePerLifetimeScope();
        }
    }
}
