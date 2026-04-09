using BugArena.API.Middleware;
using BugArena.Application.Interfaces;
using BugArena.Application.Services;
using BugArena.Application.Validators;
using BugArena.Domain.Entities;
using BugArena.Infrastructure;
using BugArena.Infrastructure.Data;
using BugArena.Infrastructure.Repositories;
using BugArena.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

// Main entry point for the BugArena API application. Configures services, middleware, and the HTTP request pipeline.
var builder = WebApplication.CreateBuilder(args);

// ── JWT Config ─────────────────────────────────────────────────────────────
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(JwtSettings.SectionName));

var jwtSettings = builder.Configuration
    .GetSection(JwtSettings.SectionName)
    .Get<JwtSettings>()
    ?? throw new InvalidOperationException("JWT settings are missing.");

// ── Database ───────────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Services ───────────────────────────────────────────────────────────────
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ISolutionRepository, SolutionRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IChallengeRepository, ChallengeRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<ILeaderboardRepository, LeaderboardRepository>();
builder.Services.AddScoped<ChallengeService>();
builder.Services.AddScoped<SolutionService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<LeaderboardService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactDev", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ── Authentication ─────────────────────────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<CreateChallengeValidator>();
builder.Services.AddEndpointsApiExplorer();

// ── Swagger ────────────────────────────────────────────────────────────────
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Bug Arena", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Klistra in ditt token här"
    });

    // NY syntax i Swashbuckle 10.x — tar emot document som parameter
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

// ── Build App ───────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Pipeline ───────────────────────────────────────────────────────────────
app.UseMiddleware<ExceptionMiddleware>();

// Aktivera Swagger och Swagger UI endast i utvecklingsmiljön för att underlätta API-dokumentation och testning.
if (app.Environment.IsDevelopment())
{
    // Aktivera Swagger-mellanvaran för att generera API-dokumentation och Swagger UI för att tillhandahålla en interaktiv gränssnitt för att testa API-endpoints.
    app.UseSwagger();
    
    // Aktivera Swagger UI-mellanvaran för att tillhandahålla en användarvänlig gränssnitt för att utforska och testa API-endpoints.
    app.UseSwaggerUI();
}

app.UseCors("AllowReactDev");

// Omdirigera alla HTTP-förfrågningar till HTTPS för att säkerställa att kommunikationen är krypterad.

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Aktivera autentisering och auktorisering i middleware-pipelinen för att skydda API-endpoints.
app.UseAuthentication();

// Aktivera auktorisering så att endast autentiserade användare kan få åtkomst till skyddade resurser.
app.UseAuthorization();

// Mappa controller-rutter så att API-endpoints kan nås via HTTP-förfrågningar.
app.MapControllers();

// ── Run App ───────────────────────────────────────────────────────────────
app.Run();