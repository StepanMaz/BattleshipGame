using BattleShipGame.Game;
using BattleShipGame.Game.Models;

namespace BattleShipGame.Services;

public interface IGameService
{
    public Guid RegisterNewGame();
    public bool RemoveLobby(Guid guid);
    public IEnumerable<Guid> GetActiveLobbies();
    public Game<PlayerData>? GetGame(Guid guid);
}