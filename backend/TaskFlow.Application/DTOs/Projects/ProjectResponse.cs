using System;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs.Projects;

public sealed class ProjectResponse{  

    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? DueDate { get; set; }
   
    public bool IsArchived { get; set; }

    public ProjectRole MyRole { get; set; }

    public int MemberCount { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
}



