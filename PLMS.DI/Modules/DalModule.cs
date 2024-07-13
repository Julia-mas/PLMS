using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PMLS.DAL.Entities;

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
        }
    }
}
