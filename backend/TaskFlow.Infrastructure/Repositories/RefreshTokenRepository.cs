using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entites;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories;

public class RefreshTokenRepository: IRefreshTokenRepository{ 
    private readonly ApplicationDbContext _context;

    public RefreshTokenRepository(ApplicationDbContext context) {
        _context = context;  
    }
  
    public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
    {
        return await _context.refreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);
    }
 
    public async Task AddAsync(RefreshToken refreshToken){
        await _context.refreshTokens.AddAsync(refreshToken);
    }

    public async Task SaveChangesAsync(){
        await _context.SaveChangesAsync();
    }
}

