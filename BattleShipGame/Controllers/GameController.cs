using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BattleShipGame.Repositories;
using BattleShipGame.Game.Models;
using BattleShipGame.Game;
using BattleShipGame.Services;
using BattleShipGame.DTO;

namespace BattleShipGame.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameService gameService;
    private readonly IMapper mapper;
    private readonly ILogger<GameController> logger;
    public GameController(
        IGameService gameService,
        IMapper mapper,
        ILogger<GameController> logger)
    {
        this.gameService = gameService;
        this.mapper = mapper;
        this.logger = logger;
    }

    [HttpGet("lobbies")]
    public IEnumerable<Guid> GetLobbiesList()
    {
        return gameService.GetActiveLobbies();
    }

    [HttpPost("lobbies")]
    public LobbyDataDTO CreateLobby()
    {
        var guid = gameService.RegisterNewGame();

        logger.LogInformation("New game created: {Guid}", guid);

        return new LobbyDataDTO() { Id = guid };
    }
}