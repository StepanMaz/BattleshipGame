using BattleShipGame.Database.Models;

namespace BattleShipGame.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetUser(string login, string password);
}