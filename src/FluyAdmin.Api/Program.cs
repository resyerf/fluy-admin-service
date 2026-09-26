using System.Text;
using FluyAdmin.Api.Middlewares;
using FluyAdmin.Application;
using FluyAdmin.Infrastructure;
using FluyAdmin.Infrastructure.Persistence;
using Fluy.SharedKernel.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>() ?? new JwtSettings();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "FLUY Admin Service", Version = "v1" });

    var bearerScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };
    options.AddSecurityDefinition("Bearer", bearerScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement { { bearerScheme, Array.Empty<string>() } });
});

// Permite que fluy-admin-web (dev server local y el dominio real desplegado) llame a esta API
// desde el navegador — mismo criterio que fluy-service (CODE.md §4.6).
const string CorsPolicy = "FluyAdminWebCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy => policy
        .WithOrigins("http://localhost:4201", "https://www.fluyadmin.resyerf.com", "https://fluyadmin.resyerf.com")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// A diferencia del tenant demo de abajo, esto corre en todo entorno: sin un PlatformUser inicial,
// nadie puede entrar nunca a fluy-admin-web (CODE.md §9.7).
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<PlatformDbContextInitializer>();
    await initializer.InitialiseAsync();
    await initializer.SeedAsync();
}

// Tenant demo (subdominio "demo", login usuario@demo.com/clavedemo123) — solo en Development,
// y solo después de que el catálogo de planes de arriba ya exista (ProvisionTenant necesita "BUSINESS").
if (app.Environment.IsDevelopment())
{
    using var demoScope = app.Services.CreateScope();
    var demoSeeder = demoScope.ServiceProvider.GetRequiredService<DemoTenantSeeder>();
    await demoSeeder.SeedAsync();
}

app.UseHttpsRedirection();

app.UseCors(CorsPolicy);

app.UseMiddleware<ValidationExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
