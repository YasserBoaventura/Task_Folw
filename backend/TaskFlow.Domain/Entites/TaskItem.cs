using System;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Enums;


namespace TaskFlow.Domain.Entites;

public class TaskItem: BaseEntity{




    public Guid ProjectId { get; set; }

    public Guid CreatedById { get; set; }

    public Guid? AssignedToId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public StatusTask Status { get; set; } = StatusTask.ToDo;

    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public DateOnly? StartDate { get; set; }

    public DateOnly? DueDate { get; set; }

    public int Position { get; set; }

    public int Version { get; set; } = 1;

    public Project Project { get; set; } = null!;

    public ApplicationUser CreatedBy { get; set; } = null!;

    public ApplicationUser? AssignedTo { get; set; }

    public ICollection<Comment> Comments { get; set; }
        = new List<Comment>();





}
