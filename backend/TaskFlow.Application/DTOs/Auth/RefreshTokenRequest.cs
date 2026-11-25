using System;
using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Application.DTOs.Auth;

public class RefreshTokenRequest{



    [Required]
    public string RefreshToken { get; set; } = string.Empty;


}
