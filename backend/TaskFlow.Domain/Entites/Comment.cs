using System;
using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entites;

public class Comment  : BaseEntity{
     public Guid TaskId { get; set; }

    public Guid UserId { get; set; }

    public string Content { get; set; } = string.Empty;

    public TaskItem Task { get; set; } = null!;

    public ApplicationUser User { get; set; } = null!;
}




