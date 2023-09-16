using BattleShipGame.Game.Exceptions;
using BattleShipGame.Game.Models;

namespace BattleShipGame.Game;

public partial class BattleShipGame<TPlayer>
{
    enum GameState
    {
        PlayersPreparation,
        FieldsPreparation,
        Active,
        Ended
    }
}