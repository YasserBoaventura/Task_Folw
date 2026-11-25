using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs.Auth;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase{

  private readonly  IAuthService _authService; 
   

  public AuthController(IAuthService authService){
    _authService = authService;
    
  }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest request){ 
        try{ 
            var response = await _authService.RegisterAsync(request);

            return Ok(response);
        }
        catch (InvalidOperationException ex){ 
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request){
        try{
            var response = await _authService.LoginAsync(request);
             return Ok(response);
        }
        catch (UnauthorizedAccessException ex){
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request){
        try  {
            var response = await _authService.RefreshAsync(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex){
            return Unauthorized(new { message = ex.Message }); 
        }
    }

    [HttpPost("logout")] 
    [Authorize]
    public async Task<IActionResult> Logout(
        RefreshTokenRequest request){
        await _authService.LogoutAsync(request);

        return NoContent();
    }
}


