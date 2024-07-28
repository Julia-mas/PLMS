using Autofac;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PMLS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace PLMS.DI.Modules
{
    public class ApiModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserStore<User>>().As<IUserStore<User>>()
                .WithParameter((pi, ctx) => pi.ParameterType == typeof(DbContext), (pi, ctx) => ctx.Resolve<LearningDbContext>())
                .InstancePerLifetimeScope();
            builder.RegisterType<UserManager<User>>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<SignInManager<User>>().AsSelf().InstancePerLifetimeScope();
        }
    }
}