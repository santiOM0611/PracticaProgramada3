using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TiquetesApp.Constants;
using TiquetesApp.Data;
using TiquetesApp.Models;
using TiquetesApp.Repositories;
using TiquetesApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Base de datos 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    )
);

// Identity 
builder.Services.AddIdentity<Usuario, IdentityRole<int>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// Cookie de Identity 
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/auth/login";
    options.AccessDeniedPath = "/auth/login";
    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        }
        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});

// Repositories 
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<ICompraRepository, CompraRepository>();

// Services
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<ICompraService, CompraService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Roles + Admin 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userManager = services.GetRequiredService<UserManager<Usuario>>();

    foreach (var role in new[] { Roles.Admin, Roles.Usuario })
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole<int> { Name = role });
    }

    const string adminEmail = "admin@tiquetes.com";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new Usuario
        {
            UserName = adminEmail,
            Email = adminEmail,
            NombreCompleto = "Administrador",
            EmailConfirmed = true
        };
        await userManager.CreateAsync(admin, "Admin123!");
        await userManager.AddToRoleAsync(admin, Roles.Admin);
    }
}

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

app.UseStatusCodePagesWithReExecute("/Home/Error/{0}"); 

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();