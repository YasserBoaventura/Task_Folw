using System;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces.Repository;
using TaskFlow.Domain.Entites;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository{

    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Project?> GetByIdAsync(
        Guid projectId,
        CancellationToken cancellationToken = default){  
        return await _context.Projects
            .Include(project => project.Members)
            .FirstOrDefaultAsync(
                project => project.Id == projectId,
                cancellationToken);
    }

    public async Task<List<Project>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default){  
        return await _context.Projects
            .AsNoTracking()
            .Include(project => project.Members)
            .Where(project =>
                project.Members.Any(member => member.UserId == userId))
            .OrderByDescending(project => project.CreatedAt)
            .ToListAsync(cancellationToken);
    }
       
    public async Task AddAsync(
        Project project,
        CancellationToken cancellationToken = default){  
        await _context.Projects.AddAsync(
            project,
            cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default){ 
        await _context.SaveChangesAsync(cancellationToken);
    }
}
