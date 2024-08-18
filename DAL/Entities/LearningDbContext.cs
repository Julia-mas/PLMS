using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PLMS.Common.Enums;
using PLMS.Common.Extensions;

namespace PLMS.DAL.Entities
{
    public class LearningDbContext : IdentityDbContext<User>
    {
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
        {
#if DEBUG
            builder.AddConsole();
#endif 
        });

        public LearningDbContext(DbContextOptions<LearningDbContext> options) : base(options)
        {
        }

        public DbSet<Goal> Goals => Set<Goal>();
        public DbSet<Task> Tasks => Set<Task>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<GoalComment> GoalComments => Set<GoalComment>();
        public DbSet<TaskComment> TaskComments => Set<TaskComment>();
        public DbSet<Status> Statuses => Set<Status>();
        public DbSet<Priority> Priorities => Set<Priority>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Categories)
                .WithOne(c => c.User)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Goal>()
                .HasMany(u => u.Tasks)
                .WithOne(g => g.Goal)
                .HasForeignKey(g => g.GoalId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Goal>()
                .HasMany(u => u.GoalComments)
                .WithOne(g => g.Goal)
                .HasForeignKey(g => g.GoalId);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Goals)
                .WithOne(g => g.Category)
                .HasForeignKey(g => g.CategoryId);

            modelBuilder.Entity<Task>()
                .HasMany(t => t.TaskComments)
                .WithOne(c => c.Task)
                .HasForeignKey(c => c.TaskId);

            modelBuilder.Entity<Status>()
                .HasMany(s => s.Tasks)
                .WithOne(t => t.Status)
                .HasForeignKey(t => t.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Status>()
                .HasMany(s => s.Goals)
                .WithOne(g => g.Status)
                .HasForeignKey(g => g.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Priority>()
                .HasMany(s => s.Tasks)
                .WithOne(t => t.Priority)
                .HasForeignKey(t => t.PriorityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Priority>()
                .HasMany(s => s.Goals)
                .WithOne(g => g.Priority)
                .HasForeignKey(g => g.PriorityId)
                .OnDelete(DeleteBehavior.Restrict);


            var statusValues = EnumExtensions.GetEnumValuesAsEnum<StatusEnum>().Select(e => new Status
            {
                Id = (int)e,
                Title = e.ToString()
            });

            var priorityValues = EnumExtensions.GetEnumValuesAsEnum<PriorityEnum>().Select(e => new Priority
            {
                Id = (int)e,
                Title = e.ToString()
            });

            modelBuilder.Entity<Status>().HasData(statusValues);

            modelBuilder.Entity<Priority>().HasData(priorityValues);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
            optionsBuilder.EnableSensitiveDataLogging(); 
#endif
            base.OnConfiguring(optionsBuilder);
        }
    }
}
