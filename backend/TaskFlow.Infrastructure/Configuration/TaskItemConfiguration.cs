using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entites;

namespace TaskFlow.Infrastructure.Configuration;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>{  
    public void Configure(EntityTypeBuilder<TaskItem> builder){ 
        builder.ToTable("TaskItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(5000);

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Priority)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Position)
            .HasDefaultValue(0);

        builder.Property(x => x.Version)
            .IsConcurrencyToken()
            .HasDefaultValue(1);

        builder.HasIndex(x => new
        {
            x.ProjectId,
            x.Status
        });

        builder.HasIndex(x => new
        {
            x.ProjectId,
            x.Position
        });

        builder.HasIndex(x => x.AssignedToId);

        builder.HasIndex(x => x.DueDate);

        // Utilizador que criou a tarefa.
        builder.HasOne(x => x.CreatedBy)
            .WithMany(x => x.CreatedTasks)
            .HasForeignKey(x => x.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        // O utilizador responsável deve pertencer
        // ao mesmo projeto da tarefa.
        builder.HasOne<ProjectMember>()
            .WithMany()
            .HasForeignKey(x => new
            {
                x.ProjectId,
                x.AssignedToId
            })
            .HasPrincipalKey(x => new
            {
                x.ProjectId,
                x.UserId
            })
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        // Navegação para o utilizador responsável.
        builder.HasOne(x => x.AssignedTo)
            .WithMany(x => x.AssignedTasks)
            .HasForeignKey(x => x.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict);
    }
} 

