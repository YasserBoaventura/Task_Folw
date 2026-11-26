using System;
using TaskFlow.Domain.Entites;

namespace TaskFlow.Application.Interfaces;

public interface IRefreshTokenRepository
{

      Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);

      Task AddAsync(RefreshToken refreshToken);

      Task SaveChangesAsync();
}
