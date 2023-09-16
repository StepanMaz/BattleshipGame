using BattleShipGame.Game.Models;

namespace BattleShipGame.Game;

public abstract class GameRules
{
    public abstract HitResult HandleHit(Point hit, FieldData field);
    public abstract bool ValidateHit(Point hit, FieldData field);
    public abstract bool ValidateField(IEnumerable<ShipData> ships);

    public HitResult ProcessHit(Point hit, FieldData field)
    {
        if (!ValidateHit(hit, field))
            throw new Exception("Invalid request");
        return HandleHit(hit, field);
    }

    public abstract Game<T> CreateGame<T>() where T : notnull;
}