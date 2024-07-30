using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PLMS.DAL.Implementation;
using PLMS.DAL.Interfaces;
using PLMS.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace PLMS.DI.Modules
{
    public class DalModule : Module
    {
        private readonly IConfiguration _configuration;
        public DalModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            builder.Register(c => new DbContextOptionsBuilder<LearningDbContext>()
                .UseSqlServer(connectionString)
                .Options)
                .As<DbContextOptions<LearningDbContext>>()
                .SingleInstance();

            builder.RegisterType<LearningDbContext>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<UserStore<User>>().As<IUserStore<User>>()
                .WithParameter((pi, ctx) => pi.ParameterType == typeof(DbContext), (pi, ctx) => ctx.Resolve<LearningDbContext>())
                .InstancePerLifetimeScope();
            builder.RegisterType<UserManager<User>>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<SignInManager<User>>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
        }
    }
}
