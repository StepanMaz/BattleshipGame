using BattleShipGameClient.Models;
using BattleShipGameClientGame;
using System.Reactive.Subjects;

namespace BattleShipGameClient.Game;

public interface IGameDataSource : IDisposable
{
    IObservable<HitResult> Hit { get;}
    IObservable<HitResult> EnemyHit { get; }
    IObservable<PlayerData> OnPlayerJoined { get;}
    IObservable<PlayerData> OnPlayerLeft { get;}
    IObservable<string> Error { get; }

    public void ProcessHit(int x, int y);
    public void SubmitField(FieldData field);
}