using Microsoft.EntityFrameworkCore;
using Revo.Domain.Entities;

namespace Revo.Infrastructure.Data;

public class RevoDbContext : DbContext
{
    public RevoDbContext(DbContextOptions<RevoDbContext> options) : base(options) { }

    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<TaskComment> TaskComments { get; set; }
    public DbSet<TaskAttachment> TaskAttachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Color).HasMaxLength(7);
            entity.HasMany(e => e.Tasks).WithOne(t => t.Project).HasForeignKey(t => t.ProjectId);
        });

        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(5000);
            entity.HasOne(e => e.Project).WithMany(p => p.Tasks).HasForeignKey(e => e.ProjectId);
            entity.HasMany(e => e.Comments).WithOne(c => c.Task).HasForeignKey(c => c.TaskId);
            entity.HasMany(e => e.Attachments).WithOne(a => a.Task).HasForeignKey(a => a.TaskId);
        });

        modelBuilder.Entity<TaskComment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(5000);
            entity.HasOne(e => e.Task).WithMany(t => t.Comments).HasForeignKey(e => e.TaskId);
        });

        modelBuilder.Entity<TaskAttachment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(500);
            entity.Property(e => e.FileUrl).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.FileType).HasMaxLength(100);
            entity.HasOne(e => e.Task).WithMany(t => t.Attachments).HasForeignKey(e => e.TaskId);
        });
    }
}
