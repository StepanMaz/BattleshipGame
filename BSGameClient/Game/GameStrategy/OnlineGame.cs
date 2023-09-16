using System.Reactive;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using Microsoft.AspNetCore.SignalR.Client;
using BattleShipGameClientGame;
using BattleShipGameClient.Models;
using System.Diagnostics;
using Newtonsoft.Json;

namespace BattleShipGameClient.Game;

public class OnlineGame : IGameDataSource
{
    public IObservable<HitResult> Hit => hit_subject;

    public IObservable<PlayerData> OnPlayerJoined => player_added_subject;

    public IObservable<PlayerData> OnPlayerLeft => player_left_subject.Where(p => p.connectionId != ConnectionId);

    public IObservable<HitResult> EnemyHit => enemy_hit_subject;

    public IObservable<string> Error => error_subject;


    private readonly HubConnection connection;

    private readonly Guid gameGuid;

    private Subject<HitResult> hit_subject = new (), enemy_hit_subject = new();
    private Subject<PlayerData> player_added_subject = new (), player_left_subject = new ();
    private Subject<string> error_subject = new ();

    public string ConnectionId => connection.ConnectionId ?? "";

    public OnlineGame(Guid guid, string url)
    {
        this.gameGuid = guid;
        connection = new HubConnectionBuilder().WithUrl(url).Build();

        Debug.WriteLine("Connecting to hub with url: " + url);

        InitConnection().GetAwaiter();
 
        async Task InitConnection()
        {
            connection.On<HitResult>(RESULT, SetOnNextFor(hit_subject));
            connection.On<PlayerData>(LOBBY_PLAYER_JOIN, SetOnNextFor(player_added_subject));
            connection.On<PlayerData>(LOBBY_PLAYER_LEAVE, SetOnNextFor(player_left_subject));
            connection.On<HitResult>(ENEMY_HIT, SetOnNextFor(enemy_hit_subject));
            connection.On<string>(ERROR, SetOnNextFor(error_subject));

            await connection.StartAsync();

            Debug.WriteLine("Connected to hub");

            await connection.SendAsync(LOBBY_JOIN, guid);

            Debug.WriteLine("Lobby joined");
        }

        hit_subject.Subscribe((h) => Debug.WriteLine("My hit: " + JsonConvert.SerializeObject(h)));
        enemy_hit_subject.Subscribe((h) => Debug.WriteLine("Enemy hit: " + JsonConvert.SerializeObject(h)));
    }

    private const string
        RESULT = "SendResult",
        ERROR = "Error",
        ENEMY_HIT = "SendHit",
        LOBBY_PLAYER_JOIN = "PlayerLobbyJoin",
        LOBBY_PLAYER_LEAVE = "PlayerLobbyLeave",
        LOBBY_JOIN = "AddToLobby",
        LOBBY_LEAVE = "RemoveFromLobby",
        PROCESS_HIT = "Hit",
        SUBMIT_FIELD = "SubmitField";

    private Action<T> SetOnNextFor<T>(Subject<T> subject)
    {
        return (T value) => subject.OnNext(value);
    }

    public void ProcessHit(int x, int y)
    {
        connection.SendAsync(PROCESS_HIT, new { Guid = gameGuid, Data = new Point(x, y) }).GetAwaiter();
    }

    public void SubmitField(FieldData field)
    {
        connection.SendAsync(SUBMIT_FIELD, new { Guid = gameGuid, Data = field }).GetAwaiter();
    }

    public async void Dispose()
    {
        await connection.SendAsync(LOBBY_LEAVE, gameGuid);
        await connection.DisposeAsync();
    }

    ~OnlineGame()
    {
        Dispose();
    }
}