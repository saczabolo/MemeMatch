using MemeMatch.Data;
using MemeMatch.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(option =>
option.UseSqlite("Data Source=memematch.db"));

builder.Services.AddControllersWithViews();

var app = builder.Build();


app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
