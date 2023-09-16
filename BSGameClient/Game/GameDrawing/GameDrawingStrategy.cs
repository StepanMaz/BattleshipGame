using BattleShipGameClient.Models;
using BattleShipGameClientGame;
using System.Diagnostics;
using Terminal.Gui;
using Point = BattleShipGameClient.Models.Point;

namespace BattleShipGameClient.Game;

public class GameDrawingStrategy : IGameDrawingStrategy
{
    ColorScheme shipScheme = new ColorScheme()
    {
        Normal = new Terminal.Gui.Attribute(Color.DarkGray)
    };

    ColorScheme hitshipScheme = new ColorScheme()
    {
        Normal = new Terminal.Gui.Attribute(Color.Red)
    };

    public void Draw(FieldData fieldData, View[,] cells)
    {
        foreach (var ship in fieldData.Ships)
        {
            foreach (var point in GetShipPoints(ship))
            {
                if (point.X < 0 || point.Y < 0 || point.X >= cells.GetLength(0) || point.Y >= cells.GetLength(1))
                    throw new Exception();
                var cell = cells[point.X, point.Y];

                cell.ColorScheme = shipScheme;
            }
        }

        foreach (var hit in fieldData.Hits)
        {
            cells[hit.Y, hit.X].Text = "X";
        }
    }

    private IEnumerable<Point> GetShipPoints(ShipData shipData)
    {
        (int x, int y) dir = shipData.Direction switch
        {
            Direction.BottomToTop => (-1, 0),
            Direction.LeftToRight => (0, 1),
            Direction.TopToBottom => (1, 0),
            Direction.RightToLeft => (0, -1),
            _ => (-1, 0)
        };

        for (int i = 0; i < shipData.Length; i++)
        {
            yield return new(shipData.Place.X + dir.x * i, shipData.Place.Y + dir.y * i);
        }
    }

    public void Draw(HitResult hitResult, View[,] cells)
    {
        foreach (var point in hitResult.Changes)
        {
            cells[point.X, point.Y].Text = "X";
            if (hitResult.HitType != HitType.Miss)
            {
                cells[point.X, point.Y].ColorScheme = hitshipScheme;
            }
        }

    }
}