using BattleShipGame.Game;
using BattleShipGame.Game.Models;
using BattleShipGame.Repositories;

namespace BattleShipGame.Services;

public class GameService : IGameService
{
    private readonly IGameRepository gameRepository;
    private readonly GameRules gameRules;

    public GameService(IGameRepository gameRepository, GameRules gameRules)
    {
       this.gameRepository = gameRepository;
       this.gameRules = gameRules;
    }

    public IEnumerable<Guid> GetActiveLobbies()
    {
        return gameRepository.GetJoinable();
    }

    public Game<PlayerData>? GetGame(Guid guid)
    {
        return gameRepository.Get(guid);
    }

    public Guid RegisterNewGame()
    {
        var guid = Guid.NewGuid();
        var lobby = gameRules.CreateGame<PlayerData>();
        
        gameRepository.Add(guid, lobby);

        return guid;
    }

    public bool RemoveLobby(Guid guid)
    {
        return gameRepository.Remove(guid);
    }
}