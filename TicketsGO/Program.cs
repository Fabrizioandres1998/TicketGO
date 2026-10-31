using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TicketsGO.Data;
using TicketsGO.Models;
using TicketsGO.Services;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>() ?? new JwtSettings();
if (string.IsNullOrWhiteSpace(builder.Configuration["Jwt:SigningKey"]))
{
    if (!builder.Environment.IsDevelopment())
        throw new InvalidOperationException("Configure Jwt:SigningKey with a secret of at least 32 bytes outside Development.");

    // A process-local development key avoids committing a usable signing secret.
    jwtSettings.SigningKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}
else
{
    jwtSettings.SigningKey = builder.Configuration["Jwt:SigningKey"]!;
}

if (Encoding.UTF8.GetByteCount(jwtSettings.SigningKey) < 32)
    throw new InvalidOperationException("Jwt:SigningKey must contain at least 32 bytes.");

if (jwtSettings.ExpiresMinutes <= 0)
    throw new InvalidOperationException("Jwt:ExpiresMinutes must be greater than zero.");

// 1. BUILDER: preparamos la aplicación y registramos sus servicios.
builder.Services.AddControllersWithViews();

// Registramos el contexto de EF Core y configuramos la conexión a MySQL.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36))
    )
);

// REGISTRO SERVICIOS PROPIOS
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IHistorialService, HistorialService>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();
builder.Services.AddScoped<ICompraService, CompraService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.Configure<JwtSettings>(options =>
{
    options.Issuer = jwtSettings.Issuer;
    options.Audience = jwtSettings.Audience;
    options.SigningKey = jwtSettings.SigningKey;
    options.ExpiresMinutes = jwtSettings.ExpiresMinutes;
});

// REGISTRO HASHER
builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

// REGISTRO DE AUTENTICACIÓN POR COOKIES
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login";
        options.AccessDeniedPath = "/Usuario/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.MapInboundClaims = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey)),
            ValidateLifetime = true,
            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.Name,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiAuthentication", policy =>
    {
        // Keep cookie auth for the Vue UI while allowing Bearer JWTs for REST clients.
        policy.AddAuthenticationSchemes(
            CookieAuthenticationDefaults.AuthenticationScheme,
            JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
});

// 2. BUILD(): construye la aplicación utilizando los servicios registrados.
var app = builder.Build();

// 3. APP: configuramos cómo la aplicación procesa las solicitudes HTTP.

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // El HSTS indica al navegador que debe utilizar HTTPS durante un período.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// MIDDLEWARES DE SEGURIDAD (Van siempre antes de los endpoints)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicia la aplicación y comienza a escuchar solicitudes HTTP.
app.Run();
