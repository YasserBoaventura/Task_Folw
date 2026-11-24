namespace TaskFlow.Domain.Entites;
using Microsoft.AspNetCore.Identity;


public class ApplicationUser : IdentityUser<Guid>{


    public string FullName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; } =
        DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAt { get; set; }

    public ICollection<ProjectMember> ProjectMemberships { get; set; }
        = new List<ProjectMember>();

    public ICollection<TaskItem> CreatedTasks { get; set; }
        = new List<TaskItem>();

    public ICollection<TaskItem> AssignedTasks { get; set; }
        = new List<TaskItem>();

    public ICollection<Comment> Comments { get; set; }
        = new List<Comment>();

    public ICollection<Notification> Notifications { get; set; }
        = new List<Notification>();


}
