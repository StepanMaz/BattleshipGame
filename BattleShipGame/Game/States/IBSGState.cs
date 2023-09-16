using BattleShipGame.Game.Models;

namespace BattleShipGame.Game;

public partial class BattleShipGame<TPlayer>
{
    interface IBSGState
    {
        void AddPlayer(TPlayer playerData);

        HitResult Hit(TPlayer playerData, Point data);

        void SetField(TPlayer playerData, FieldData field);
    }
}