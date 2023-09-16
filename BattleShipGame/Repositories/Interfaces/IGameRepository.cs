using BattleShipGame.Game;
using BattleShipGame.Game.Models;

namespace BattleShipGame.Repositories;

public interface IGameRepository
{
    public void Add(Guid guid, Game<PlayerData> game);
    public Game<PlayerData>? Get(Guid guid);
    public bool Remove(Guid guid);
    public IEnumerable<Guid> GetJoinable();
}