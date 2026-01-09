using MemeMatch.Data;
using MemeMatch.Models;
using MemeMatch.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(option =>
option.UseSqlite("Data Source=memematch.db"));

builder.Services.AddScoped<GameService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("SUPER_SECRET_KEY_12345"))
        };
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!context.Memes.Any())
    {
        var memes = new List<Meme>
{
            new Meme { ImageUrl = "meme1.jpg", IsActive = true },
            new Meme { ImageUrl = "meme2.jpg", IsActive = true },
            new Meme { ImageUrl = "meme3.jpg", IsActive = true },
            new Meme { ImageUrl = "meme4.jpg", IsActive = true },
};

        context.Memes.AddRange(memes);
        context.SaveChanges();

        var prompts = new List<Prompt>
{
            new Prompt { Text = "Kiedy kod dzia³a, ale nie wiesz dlaczego", CorrectMemeId = memes[0].Id },
            new Prompt { Text = "Gdy projekt oddajesz o 23:59", CorrectMemeId = memes[1].Id }
        };

        context.Prompts.AddRange(prompts);
        context.SaveChanges();
    }

    if (!context.Users.Any())
    {
        context.Users.Add(new User
        {
            Username = "testuser",
            Email = "test@test.pl",
            Password = BCrypt.Net.BCrypt.HashPassword("Haslo123!"),
            Role = "User"
        });
        context.SaveChanges();
    }

    context.SaveChanges();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
