
using Microsoft.EntityFrameworkCore;
using TicketsGO.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. BUILDER: preparamos la aplicación y registramos sus servicios.
builder.Services.AddControllersWithViews();

// Registramos el contexto de EF Core y configuramos la conexión a MySQL.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    )
);

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicia la aplicación y comienza a escuchar solicitudes HTTP.
app.Run();
