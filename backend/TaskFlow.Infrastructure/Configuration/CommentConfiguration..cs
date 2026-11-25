using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entites;

namespace TaskFlow.Infrastructure.Configuration;


public class CommentConfiguration : IEntityTypeConfiguration<Comment>{ 
    public void Configure(EntityTypeBuilder<Comment> builder){ 
        builder.ToTable("Comments");

        builder.HasKey(x => x.Id); 

        builder.Property(x => x.Content)
            .HasMaxLength(2000)
            .IsRequired();

        builder.HasIndex(x => x.TaskId);

        builder.HasIndex(x => x.UserId);

        builder.HasOne(x => x.Task)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
