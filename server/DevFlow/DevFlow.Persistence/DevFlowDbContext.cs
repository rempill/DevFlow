using DevFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Persistence;

public class DevFlowDbContext : DbContext
{
    public DevFlowDbContext(DbContextOptions<DevFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<Developer> Developers => Set<Developer>();
    public DbSet<LeadDeveloper> LeadDevelopers => Set<LeadDeveloper>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Domain.Entities.Task> Tasks => Set<Domain.Entities.Task>();
    public DbSet<TimeLog> TimeLogs => Set<TimeLog>();
    public DbSet<Commit> Commits => Set<Commit>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<UMLDiagram> UMLDiagrams => Set<UMLDiagram>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Developer>()
            .HasDiscriminator<string>("DeveloperType")
            .HasValue<Developer>("Developer")
            .HasValue<LeadDeveloper>("LeadDeveloper");

        modelBuilder.Entity<Project>()
            .HasOne(project => project.Owner)
            .WithMany()
            .HasForeignKey(project => project.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Project>()
            .HasOne(project => project.UMLDiagram)
            .WithOne(diagram => diagram.Project)
            .HasForeignKey<UMLDiagram>(diagram => diagram.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Domain.Entities.Task>()
            .HasOne(task => task.Project)
            .WithMany(project => project.Tasks)
            .HasForeignKey(task => task.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Domain.Entities.Task>()
            .HasMany(task => task.TimeLogs)
            .WithOne(timeLog => timeLog.Task)
            .HasForeignKey(timeLog => timeLog.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Domain.Entities.Task>()
            .HasMany(task => task.Commits)
            .WithOne(commit => commit.Task)
            .HasForeignKey(commit => commit.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Domain.Entities.Task>()
            .HasMany(task => task.Precursors)
            .WithMany(task => task.Successors)
            .UsingEntity<Dictionary<string, object>>(
                "TaskDependencies",
                join => join
                    .HasOne<Domain.Entities.Task>()
                    .WithMany()
                    .HasForeignKey("PrecursorId")
                    .OnDelete(DeleteBehavior.Restrict),
                join => join
                    .HasOne<Domain.Entities.Task>()
                    .WithMany()
                    .HasForeignKey("SuccessorId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.HasKey("PrecursorId", "SuccessorId");
                });

        modelBuilder.Entity<Commit>()
            .HasKey(commit => commit.Sha);

        modelBuilder.Entity<Commit>()
            .OwnsOne(commit => commit.Version, version =>
            {
                version.Property(v => v.Major).HasColumnName("VersionMajor");
                version.Property(v => v.Minor).HasColumnName("VersionMinor");
                version.Property(v => v.Patch).HasColumnName("VersionPatch");
            });

        modelBuilder.Entity<Review>()
            .HasOne(review => review.Commit)
            .WithMany(commit => commit.Reviews)
            .HasForeignKey(review => review.CommitSha)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Review>()
            .HasOne(review => review.Reviewer)
            .WithMany()
            .HasForeignKey(review => review.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}


