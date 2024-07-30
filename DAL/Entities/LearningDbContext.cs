using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PLMS.DAL.Entities
{
    public class LearningDbContext : IdentityDbContext<User>
    {
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public LearningDbContext(DbContextOptions<LearningDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
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

            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Title = "NotStarted" },
                new Status { Id = 2, Title = "InProgress" },
                new Status { Id = 3, Title = "Completed" },
                new Status { Id = 4, Title = "OnHold" },
                new Status { Id = 5, Title = "Cancelled" },
                new Status { Id = 6, Title = "Failed" }
            );

            modelBuilder.Entity<Priority>().HasData(
                new Priority { Id = 1, Title = "Low" },
                new Priority { Id = 2, Title = "Medium" },
                new Priority { Id = 3, Title = "High" }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLoggerFactory(MyLoggerFactory);
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}
