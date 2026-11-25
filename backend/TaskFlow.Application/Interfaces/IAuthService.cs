using System;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs.Auth;

namespace TaskFlow.Application.Interfaces;

public interface IAuthService{


   Task<AuthResponse> RegisterAsync(RegisterRequest request);

    Task<AuthResponse> LoginAsync(LoginRequest request);

    Task<AuthResponse> RefreshAsync(RefreshTokenRequest request);

    Task LogoutAsync(RefreshTokenRequest request);



}
