using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using portfolioApi.Data;
using portfolioApi.Services;
using portfolioApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injeção de dependências dos serviços da aplicação
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IAboutMeService, AboutMeService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IUserService, UserService>();

// Configuração de Autenticação JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "PortfolioSuperSecretKey2025WithHighEntropyAnd32BytesMin!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "portfolio-api";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "portfolio-client";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

if (!builder.Environment.IsEnvironment("Testing"))
{
    var connectString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseNpgsql(connectString);
    });
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
