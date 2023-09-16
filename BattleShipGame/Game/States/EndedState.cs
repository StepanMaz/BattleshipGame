using BattleShipGame.Game.Exceptions;
using BattleShipGame.Game.Models;

namespace BattleShipGame.Game;

public partial class BattleShipGame<TPlayer>
{
    class EndedState : IBSGState
    {
        private EndedState() {}

        public void AddPlayer(TPlayer playerData)
        {
            throw new WrongStateException<GameState>(expected: GameState.PlayersPreparation, actual: GameState.Ended);
        }

        public HitResult Hit(TPlayer playerData, Point data)
        {
            throw new WrongStateException<GameState>(expected: GameState.Active, actual: GameState.Ended);
        }

        public void SetField(TPlayer playerData, FieldData field)
        {
            throw new WrongStateException<GameState>(expected: GameState.FieldsPreparation, actual: GameState.Active);
        }

        public static EndedState Instance = new EndedState();
    }
}