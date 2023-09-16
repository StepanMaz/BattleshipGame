using BattleShipGame.Game.Exceptions;
using BattleShipGame.Game.Models;

namespace BattleShipGame.Game;

public partial class BattleShipGame<TPlayer>
{
    class ActiveState : IBSGState
    {
        private TPlayer currentPlayer = default!;
        private TPlayer nextPlayer = default!;

        private readonly BattleShipGame<TPlayer> game;
        public ActiveState(BattleShipGame<TPlayer> game)
        {
            this.game = game;
        }

        public void AddPlayer(TPlayer playerData)
        {
            throw new WrongStateException<GameState>(expected: GameState.FieldsPreparation, actual: GameState.PlayersPreparation);
        }

        public HitResult Hit(TPlayer playerData, Point data)
        {
            if(currentPlayer is null) {
                currentPlayer = playerData;
            }

            if(playerData.Equals(currentPlayer))
            {
                var processing_result = game.gameRules.ProcessHit(data, game.players.First(t => !t.Key.Equals(playerData)).Value);
                
                if(processing_result.GameEnded)
                {
                    game.current_state = EndedState.Instance;
                }

                if(processing_result.PlayerChanged)
                {
                    (currentPlayer, nextPlayer) = (nextPlayer, currentPlayer);
                }

                return processing_result;
            }

            throw new Exception("Incorrect turn order");
        }

        public void SetField(TPlayer playerData, FieldData field)
        {
            throw new WrongStateException<GameState>(expected: GameState.FieldsPreparation, actual: GameState.Active);
        }
    }
}