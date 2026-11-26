using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Entites;

namespace TaskFlow.Infrastructure.Data;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>{  
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

  
    public DbSet<Project> Projects => Set<Project>();

    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();

    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
 
    public DbSet<Comment> Comments => Set<Comment>();
 
    public DbSet<Notification> Notifications => Set<Notification>();

    public DbSet<RefreshToken> refreshTokens => Set<RefreshToken>(); 
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(x => x.FullName)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.IsActive)
                .HasDefaultValue(true);

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            entity.HasIndex(x => x.IsActive);
        });
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();

        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateAuditFields();

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();

        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();

        return base.SaveChangesAsync(
            acceptAllChangesOnSuccess,
            cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var now = DateTimeOffset.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;

                entry.Property(nameof(BaseEntity.CreatedAt))
                    .IsModified = false;
            }
        }

        foreach (var entry in ChangeTracker.Entries<ApplicationUser>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;

                entry.Property(nameof(ApplicationUser.CreatedAt))
                    .IsModified = false;
            }
        }
    }
}