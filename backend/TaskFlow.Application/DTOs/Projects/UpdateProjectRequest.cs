using System;
using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Application.DTOs.Projects;

public sealed class UpdateProjectRequest{

      [Required]
      [StringLength(150, MinimumLength = 3)]
      public string Name { get; set; } = string.Empty;

      [StringLength(2000)]
      public string? Description { get; set; }

      public DateOnly? StartDate { get; set; }

      public DateOnly? DueDate { get; set; }
}



