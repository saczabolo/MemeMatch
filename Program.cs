using MemeMatch.Data;
using MemeMatch.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(option =>
option.UseSqlite("Data Source=memematch.db"));

builder.Services.AddScoped<GameService>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
