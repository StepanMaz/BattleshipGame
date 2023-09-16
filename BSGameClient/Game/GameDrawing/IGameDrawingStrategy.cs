using BattleShipGameClient.Game;
using BattleShipGameClient.Models;
using BattleShipGameClientGame;
using Terminal.Gui;

namespace BattleShipGameClient.Game;

public interface IGameDrawingStrategy
{
    void Draw(FieldData fieldData, View[,] cells);
    void Draw(HitResult hitResult, View[,] cells);
}