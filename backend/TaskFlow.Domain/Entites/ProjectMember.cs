using System;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entites;

public class ProjectMember : BaseEntity{
    public Guid ProjectId { get; set; }

    public Guid UserId { get; set; }

    public ProjectRole Role { get; set; } = ProjectRole.Member;

    public DateTimeOffset JoinedAt { get; set; } =
        DateTimeOffset.UtcNow;
 
    public Project Project { get; set; } = null!;

    public ApplicationUser User { get; set; } = null!;
}