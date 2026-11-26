
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskFlow.API.Exceptions.Handler;
using TaskFlow.Application.Options;
using TaskFlow.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Application
builder.Services.AddApplication();

// Infrastructure: MySQL, DbContext e Identity
builder.Services.AddInfrastructure(builder.Configuration);

//GlobalExceptionHandler 
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// JWT settings
var jwtOptions = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtOptions>()
    ?? throw new InvalidOperationException(
        "Configuração JWT não encontrada.");

if (string.IsNullOrWhiteSpace(jwtOptions.SecretKey) ||
    Encoding.UTF8.GetByteCount(jwtOptions.SecretKey) < 32)
{
    throw new InvalidOperationException(
        "A chave JWT deve ter pelo menos 32 bytes.");
}

if (string.IsNullOrWhiteSpace(jwtOptions.Issuer) ||
    string.IsNullOrWhiteSpace(jwtOptions.Audience))
{
    throw new InvalidOperationException(
        "Issuer e Audience JWT são obrigatórios.");
}

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

// Authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddOpenApi();


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
