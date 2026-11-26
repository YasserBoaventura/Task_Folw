using System;
using TaskFlow.Domain.Entites;

namespace TaskFlow.Application.Interfaces.Repository;



public  interface IProjectRepository
{
    Task<Project?> GetByIdAsync(
        Guid projectId,
        CancellationToken cancellationToken = default);

    Task<List<Project>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Project project,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}

