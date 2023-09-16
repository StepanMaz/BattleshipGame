using BattleShipGame.Database;
using BattleShipGame.Database.Models;

namespace BattleShipGame.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(BattleShipGameContext context) : base(context)
    {

    }

    public Task<User?> GetUser(string login, string password)
    {
        return Task.FromResult(set.FirstOrDefault(user => user.Login == login && user.Password == password));
    }
}