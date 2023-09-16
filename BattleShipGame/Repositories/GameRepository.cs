using BattleShipGame.Game;
using BattleShipGame.Game.Models;

namespace BattleShipGame.Repositories;

public class GameRepository : IGameRepository
{
    private readonly Dictionary<Guid, Game<PlayerData>> storage = new ();

    public void Add(Guid guid, Game<PlayerData> game)
    {
        storage.Add(guid, game);
    }

    public Game<PlayerData>? Get(Guid guid)
    {
        return storage.TryGetValue(guid, out var res) ? res : null;
    }

    public IEnumerable<Guid> GetJoinable()
    {
        return storage.Where(t => t.Value.CanBeJoined).Select(t => t.Key);
    }

    public bool Remove(Guid guid)
    {
        return storage.Remove(guid);
    }
}