
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs.Projects;
using TaskFlow.Application.Interfaces.Services;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/projects")]

public sealed class ProjectsController : ControllerBase{  
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService){  
        _projectService = projectService;
    }

    [HttpPost]
    public async Task<ActionResult<ProjectResponse>> Create(
        [FromBody] CreateProjectRequest request,
        CancellationToken cancellationToken){ 
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        var result = await _projectService.CreateAsync(
            userId,
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

  
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectResponse>>> GetMyProjects(
        CancellationToken cancellationToken){ 
        if (!TryGetUserId(out var userId))
            return Unauthorized();
   
        var projects = await _projectService.GetMyProjectsAsync(
            userId,
            cancellationToken);

        return Ok(projects);
    }  

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProjectResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        var project = await _projectService.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        return Ok(project);
    }

  
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProjectResponse>> Update(
        Guid id,
        [FromBody] UpdateProjectRequest request,
        CancellationToken cancellationToken){
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        var project = await _projectService.UpdateAsync(
            id,
            userId,
            request,
            cancellationToken);

        return Ok(project);
    }

 
    

    // PATCH: api/projects/{id}/archive
    [HttpPatch("{id:guid}/archive")]
    public async Task<IActionResult> Archive(
        Guid id,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        await _projectService.ArchiveAsync(
            id,
            userId,
            cancellationToken);

        return NoContent();
    }

    private bool TryGetUserId(out Guid userId){ 
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        return Guid.TryParse(userIdValue, out userId);
    }


}
