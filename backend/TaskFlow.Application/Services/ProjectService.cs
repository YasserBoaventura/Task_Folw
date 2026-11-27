using System;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs.Projects;
using TaskFlow.Application.Exceptions;
using TaskFlow.Application.Interfaces.Repository;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Entites;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Services;

public sealed class ProjectService : IProjectService{

    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository){
        _projectRepository = projectRepository;
    }

    public async Task<ProjectResponse> CreateAsync(
        Guid userId,
        CreateProjectRequest request,
        CancellationToken cancellationToken = default){  
        ValidateDates(request.StartDate, request.DueDate);

        var project = new Project{  
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            StartDate = request.StartDate,
            DueDate = request.DueDate,
            IsArchived = false
        };

        var ownerMembership = new ProjectMember
        {
            UserId = userId,
            Role = ProjectRole.Owner,
            JoinedAt = DateTimeOffset.UtcNow,
            Project = project
        };

        project.Members.Add(ownerMembership);

        await _projectRepository.AddAsync(   
            project,
            cancellationToken);

        await _projectRepository.SaveChangesAsync(
            cancellationToken); 

        return MapToResponse(project, userId);
    }

    public async Task<List<ProjectResponse>> GetMyProjectsAsync(
        Guid userId,
        CancellationToken cancellationToken = default){ 
        var projects = await _projectRepository.GetByUserIdAsync(
            userId,     
            cancellationToken);  
            if(projects is null)
            throw new ProjectNotFoundException();

        return projects
            .Select(project => MapToResponse(project, userId))
            .ToList();
    }
  
 public async Task<ProjectResponse> GetByIdAsync(
    Guid projectId,
    Guid userId,
    CancellationToken cancellationToken = default){   
    var project = await GetProjectOrThrowAsync(
        projectId,
        cancellationToken);
        
        
        if(project is null)
    throw new ProjectNotFoundException(); 

    EnsureMembership(project, userId);

    return MapToResponse(project, userId);
}
    public async Task<ProjectResponse> UpdateAsync(
        Guid projectId,
        Guid userId,
        UpdateProjectRequest request,
        CancellationToken cancellationToken = default){ 
        var project = await GetProjectOrThrowAsync(
            projectId,
            cancellationToken);

            if(project is null)
              throw new ProjectNotFoundException(); 

        EnsureOwnerOrAdmin(project, userId);

        if (project.IsArchived)
            throw new InvalidOperationException(
                "Não é possível atualizar um projeto arquivado.");

        ValidateDates(request.StartDate, request.DueDate);

        project.Name = request.Name.Trim();
        project.Description = request.Description?.Trim();
        project.StartDate = request.StartDate;
        project.DueDate = request.DueDate;
        project.UpdatedAt = DateTimeOffset.UtcNow;

        await _projectRepository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(project, userId);
    }

    public async Task ArchiveAsync(
        Guid projectId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var project = await GetProjectOrThrowAsync(
            projectId,
            cancellationToken);

        EnsureOwnerOrAdmin(project, userId);

        if (project.IsArchived)
            throw new InvalidOperationException(
                "O projeto já está arquivado.");

        project.IsArchived = true;
        project.UpdatedAt = DateTimeOffset.UtcNow;

        await _projectRepository.SaveChangesAsync(
            cancellationToken);
    }

 private async Task<Project> GetProjectOrThrowAsync(
    Guid projectId,
    CancellationToken cancellationToken)
{
    var project = await _projectRepository.GetByIdAsync(
        projectId,
        cancellationToken);

    if (project is null)
        throw new ProjectNotFoundException();

    return project;
}
    private static void EnsureMembership(
        Project project,
        Guid userId)
    {
        var isMember = project.Members.Any(
            member => member.UserId == userId);

        if (!isMember)
            throw new ProjectForbiddenException();
    }

    private static void EnsureOwnerOrAdmin(
        Project project,
        Guid userId)
    {
        var membership = project.Members.FirstOrDefault(
            member => member.UserId == userId);

        if (membership is null ||
            membership.Role is not (ProjectRole.Owner or ProjectRole.Admin))
        {
            throw new ProjectForbiddenException();
        }
    }

    private static void ValidateDates(
        DateOnly? startDate,
        DateOnly? dueDate){   
        if (startDate.HasValue &&
            dueDate.HasValue &&
            dueDate.Value < startDate.Value)
        {
            throw new ArgumentException(
                "A data de conclusão não pode ser anterior à data de início.");
        }
    }

    private static ProjectResponse MapToResponse(
        Project project,
        Guid userId)
    {
        var membership = project.Members.First(
            member => member.UserId == userId);

        return new ProjectResponse
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            StartDate = project.StartDate,
            DueDate = project.DueDate,
            IsArchived = project.IsArchived,
            MyRole = membership.Role,
            MemberCount = project.Members.Count,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt
        };
    }
}



