using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entites;

namespace TaskFlow.Infrastructure.Configuration;

public class ProjectMemberConfiguration: IEntityTypeConfiguration<ProjectMember>
{


    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.ToTable("ProjectMembers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Role)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.JoinedAt) 
            .IsRequired();

        // Impede que o mesmo utilizador seja adicionado
        // mais de uma vez ao mesmo projeto.
        builder.HasAlternateKey(x => new
        {
            x.ProjectId,
            x.UserId
        });

        builder.HasIndex(x => x.UserId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.ProjectMemberships)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

