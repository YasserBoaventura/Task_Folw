using System;
using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entites;

public class RefreshToken : BaseEntity{ 

    public Guid UserId { get; set; }

    public string TokenHash { get; set; } = string.Empty;

    public DateTimeOffset ExpiresAt { get; set; }

    public DateTimeOffset? RevokedAt { get; set; }

    public string? ReplacedByTokenHash { get; set; }

    public ApplicationUser User { get; set; } = null!;

    public bool IsActive =>
        RevokedAt == null &&
        ExpiresAt > DateTimeOffset.UtcNow;



}
