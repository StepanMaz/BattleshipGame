using BattleShipGame.Database.Models;

namespace BattleShipGame.Services;

public interface IAuthService
{
    public Task<User?> Login(string login, string password);
    public Task<User> Register(string login, string password);
}