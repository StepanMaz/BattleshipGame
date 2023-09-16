using BattleShipGame.Database;
using BattleShipGame.Game;
using BattleShipGame.Game.Models;
using BattleShipGame.Hubs;
using BattleShipGame.Profiles;
using BattleShipGame.Repositories;
using BattleShipGame.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddDbContext<BattleShipGameContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("postgre")));
//builder.Services.AddDbContext<BattleShipGameContext>(o => o.UseInMemoryDatabase("test db"));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddSingleton<IGameRepository, GameRepository>();
builder.Services.AddScoped<IAuthService>(o => new AuthProxy(new AuthService(o.GetService<IUserRepository>()!, o.GetService<ILogger<AuthService>>()!)));
builder.Services.AddAutoMapper(o => o.AddProfile<UserProfile>());

builder.Services.AddSingleton<GameRules, SimpleRules>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<GameHub>("game");

app.Run();
