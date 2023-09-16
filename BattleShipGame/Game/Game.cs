using BattleShipGame.Game.Models;

namespace BattleShipGame.Game;

public abstract class Game<TPlayer>
{
    protected GameRules gameRules;
    public Game(GameRules gameRules)
    {
        this.gameRules = gameRules;
    }

    public abstract void AddPlayer(TPlayer playerData);

    public abstract HitResult Hit(TPlayer playerData, Point data);

    public abstract void SetField(TPlayer playerData, FieldData field);

    public abstract bool CanBeJoined { get; }
}