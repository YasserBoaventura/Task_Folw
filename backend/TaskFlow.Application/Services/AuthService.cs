using System;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskFlow.Application.DTOs.Auth;
using TaskFlow.Application.Options;
using TaskFlow.Domain.Entites;

public class AuthService : IAuthService{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly JwtOptions _jwtOptions;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IRefreshTokenRepository refreshTokenRepository,
        IOptions<JwtOptions> jwtOptions){ 
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtOptions = jwtOptions.Value;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email.Trim();

        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser != null)
            throw new InvalidOperationException(
                "Já existe uma conta com este email.");

        var user = new ApplicationUser 
        { 
            UserName = email,
            Email = email,
            FullName = request.FullName.Trim(),
            IsActive = true,
            EmailConfirmed = false
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                "; ",
                result.Errors.Select(x => x.Description));

            throw new InvalidOperationException(errors);
        }

        return await CreateAuthResponseAsync(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request){
        var user = await _userManager.FindByEmailAsync(
            request.Email.Trim());

        if (user == null || 
            !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedAccessException(
                "Email ou password inválidos.");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException(
                "Esta conta encontra-se desativada.");
        }

        return await CreateAuthResponseAsync(user);
    }

    public async Task<AuthResponse> RefreshAsync(
        RefreshTokenRequest request){ 

        var tokenHash = HashToken(request.RefreshToken);

        var storedToken =
            await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);

        if (storedToken == null || !storedToken.IsActive)
        {
            throw new UnauthorizedAccessException(
                "Refresh Token inválido ou expirado.");
        }

        if (!storedToken.User.IsActive)
        {
            throw new UnauthorizedAccessException(
                "Esta conta encontra-se desativada.");
        }

        storedToken.RevokedAt = DateTimeOffset.UtcNow;

        var response = await CreateAuthResponseAsync(storedToken.User);

        // Indica qual token substituiu o anterior.
        storedToken.ReplacedByTokenHash = HashToken(response.RefreshToken);

        await _refreshTokenRepository.SaveChangesAsync();

        return response;
    }

    public async Task LogoutAsync(RefreshTokenRequest request){ 
        var tokenHash = HashToken(request.RefreshToken);

        var storedToken =
            await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);

        if (storedToken == null || storedToken.RevokedAt != null)
            return;

        storedToken.RevokedAt = DateTimeOffset.UtcNow;

        await _refreshTokenRepository.SaveChangesAsync();
    }


    private async Task<AuthResponse> CreateAuthResponseAsync(
        ApplicationUser user){ 
        var now = DateTimeOffset.UtcNow;

        var accessTokenExpiresAt =
            now.AddMinutes(_jwtOptions.AccessTokenMinutes);

        var accessToken = GenerateAccessToken(
            user,
            accessTokenExpiresAt);

        var refreshToken = GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = HashToken(refreshToken),
            ExpiresAt = now.AddDays(_jwtOptions.RefreshTokenDays)
        };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);
        await _refreshTokenRepository.SaveChangesAsync();

        return new AuthResponse{ 
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email!,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = accessTokenExpiresAt
        };
    }

    private string GenerateAccessToken(ApplicationUser user,DateTimeOffset expiresAt){
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(ClaimTypes.Name, user.FullName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken(){
        var randomBytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(randomBytes);
    }

    private static string HashToken(string token){
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));

        return Convert.ToHexString(bytes).ToLowerInvariant();
    }



}

  


