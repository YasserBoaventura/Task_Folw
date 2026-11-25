using System;
using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entites;

public class Project: BaseEntity{  
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
  
    public DateOnly? StartDate { get; set; }

    public DateOnly? DueDate { get; set; }

    public bool IsArchived { get; set; }

    public ICollection<ProjectMember> Members { get; set; }
        = new List<ProjectMember>();

    public ICollection<TaskItem> Tasks { get; set; }
        = new List<TaskItem>();





}
