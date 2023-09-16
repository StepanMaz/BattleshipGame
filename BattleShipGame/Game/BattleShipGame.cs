using BattleShipGame.Game.Models;

namespace BattleShipGame.Game;

public partial class BattleShipGame<TPlayer> : Game<TPlayer> where TPlayer : notnull
{
    private const int MAX_PLAYERS_SIZE = 2;

    private IBSGState current_state;

    private readonly Dictionary<TPlayer, FieldData> players = new ();

    public override bool CanBeJoined => current_state.GetType() == typeof(PreparationState);

    public BattleShipGame(GameRules gameRules) : base(gameRules)
    {
        current_state = new PreparationState(this);
    }

    public override void AddPlayer(TPlayer playerData)
    {
        current_state.AddPlayer(playerData);
    }

    public override HitResult Hit(TPlayer playerData, Point data)
    {
        return current_state.Hit(playerData, data);
    }

    public override void SetField(TPlayer playerData, FieldData field)
    {
        current_state.SetField(playerData, field);
    }
}