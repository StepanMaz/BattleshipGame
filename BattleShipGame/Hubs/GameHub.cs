using BattleShipGame.Game;
using BattleShipGame.Game.Models;
using BattleShipGame.Services;
using Microsoft.AspNetCore.SignalR;

namespace BattleShipGame.Hubs;

public interface IHubClient
{
    Task PlayerLobbyJoin(PlayerData playerData);
    Task PlayerLobbyLeave(PlayerData playerData);
    Task SendResult(HitResult result);
    Task SendHit(HitResult result);
    Task Error(string message);
}

public class GameHub : Hub<IHubClient>
{
    private readonly IGameService gameService;
    private readonly ILogger<GameHub> logger;

    public GameHub(IGameService gameService, ILogger<GameHub> logger)
    {
        this.gameService = gameService;
        this.logger = logger; 
    }

    #region lobby
    public async Task RemoveFromLobby(Guid guid)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, guid.ToString());

        await Clients.Group(guid.ToString()).PlayerLobbyLeave(new PlayerData(Context.ConnectionId));

        gameService.RemoveLobby(guid);
    }

    public async Task AddToLobby(Guid guid)
    {
        try
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, guid.ToString());

            gameService.GetGame(guid)?.AddPlayer(new PlayerData(Context.ConnectionId));

            await Clients.Group(guid.ToString()).PlayerLobbyJoin(new PlayerData(Context.ConnectionId));
        }
        catch (Exception e)
        {
            await Clients.Caller.Error(e.Message);
            logger.LogError(e.ToString());
        }
    }
    #endregion

    public async Task Hit(GameData<Point> hit)
    {
        try
        {
            var game = gameService.GetGame(hit.Guid);

            if(game is not null && hit.Data is not null)
            {
                var res = game.Hit(new PlayerData(Context.ConnectionId), hit.Data);

                await Clients.Caller.SendResult(res);
                await Clients.GroupExcept(hit.Guid.ToString(), Context.ConnectionId).SendHit(res);
            }
        }
        catch (Exception e)
        {
            await Clients.Caller.Error(e.Message);
            logger.LogError(e.ToString());
        }
    }

    public async Task SubmitField(GameData<FieldData> fieldData)
    {
        try
        {
            var game = gameService.GetGame(fieldData.Guid);

            if(game is not null && fieldData.Data is not null)
            {
                game.SetField(new PlayerData(Context.ConnectionId), fieldData.Data);
            }
        }
        catch (Exception e)
        {
            await Clients.Caller.Error(e.Message);
            logger.LogError(e.ToString());
        }
    }
}

public class GameData<TData>
{
    public Guid Guid { get; set; }
    public TData? Data { get; set; } = default!;
}