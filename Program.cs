using MemeMatch.Data;
using MemeMatch.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MemeMatch.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(option =>
option.UseSqlite("Data Source=memematch.db"));

builder.Services.AddControllersWithViews();

builder.Services.AddSession();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<GameService>();

var app = builder.Build();

app.UseAuthentication();
app.UseStaticFiles();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
