using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PMLS.DAL.Entities;


namespace PLMS.DAL
{
    public class LearningDbContextFactory: IDesignTimeDbContextFactory<LearningDbContext>
    {
        public LearningDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LearningDbContext>();
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../PLMS");

            // Create configuration
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new LearningDbContext(optionsBuilder.Options);
        }
    }
}
