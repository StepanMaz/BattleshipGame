using BattleShipGame.Game.Models;
using BattleShipGame.Game.Exceptions;

namespace BattleShipGame.Game;

public partial class BattleShipGame<TPlayer>
{
    class PreparationState : IBSGState
    {
        private readonly BattleShipGame<TPlayer> game;

        public PreparationState(BattleShipGame<TPlayer> game)
        {
            this.game = game;
        }

        public void AddPlayer(TPlayer playerData)
        {
            if(game.players.Count <= BattleShipGame<TPlayer>.MAX_PLAYERS_SIZE)
            {
                game.players.Add(playerData, FieldData.Empty);
                return;
            }

            throw new Exception("The game is already full");
        }

        public HitResult Hit(TPlayer playerData, Point data)
        {
            throw new WrongStateException<GameState>(expected: GameState.Active, actual: GameState.PlayersPreparation);
        }

        public void SetField(TPlayer playerData, FieldData field)
        {
            if(game.players.ContainsKey(playerData))
            {
                game.players[playerData] = field;
            }
            else
            {
                game.players.Add(playerData, FieldData.Empty);
            }

            if(game.players.Count == BattleShipGame<TPlayer>.MAX_PLAYERS_SIZE && game.players.All(t => t.Value != FieldData.Empty)) {
                game.current_state = new ActiveState(game);
            }
        }
    }
}