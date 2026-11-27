using System;
using TaskFlow.Application.DTOs.Projects;
using TaskFlow.Domain.Entites;

namespace TaskFlow.Application.Interfaces.Services;

public interface IProjectService{
    Task<ProjectResponse> CreateAsync(
        Guid userId,
        CreateProjectRequest request,
        CancellationToken cancellationToken = default);

    Task<List<ProjectResponse>> GetMyProjectsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<ProjectResponse> GetByIdAsync(
        Guid projectId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<ProjectResponse> UpdateAsync(
        Guid projectId,
        Guid userId,
        UpdateProjectRequest request,
        CancellationToken cancellationToken = default);

    Task ArchiveAsync(
        Guid projectId,
        Guid userId,
        CancellationToken cancellationToken = default);
}





