using System;

namespace TaskFlow.Domain.Entites;

public class Comment{
    public Guid TaskId { get; set; }

    public Guid UserId { get; set; }

    public string Content { get; set; } = string.Empty;

    public TaskItem Task { get; set; } = null!;

    public ApplicationUser User { get; set; } = null!;
}




