using System;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entites;

public class Notification: BaseEntity{
      public Guid UserId { get; set; }

      public string Title { get; set; } = string.Empty;

      public string Message { get; set; } = string.Empty;

      public NotificationType Type { get; set; }

      public bool IsRead { get; set; }  

       public DateTimeOffset? ReadAt { get; set; }

      public ApplicationUser User { get; set; } = null!;
  



}
