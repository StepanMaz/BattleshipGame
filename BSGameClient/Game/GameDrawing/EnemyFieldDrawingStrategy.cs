using BattleShipGameClient.Game;
using BattleShipGameClient.Models;
using BattleShipGameClientGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;
using Point = BattleShipGameClient.Models.Point;

namespace BSGameClient.Game.GameDrawing
{
    internal class EnemyFieldDrawingStrategy : IGameDrawingStrategy
    {
        ColorScheme shipScheme = new ColorScheme()
        {
            Normal = new Terminal.Gui.Attribute(Color.Brown)
        };

        public void Draw(FieldData fieldData, View[,] cells)
        {
            foreach (var ship in fieldData.Ships)
            {
                foreach (var point in GetShipPoints(ship))
                {
                    cells[point.X, point.Y].ColorScheme = shipScheme;
                }
            }

            foreach (var hit in fieldData.Hits)
            {
                cells[hit.Y, hit.X].Text = "XX";
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
                cells[point.X, point.Y].Text = "XX";
                cells[point.X, point.Y].Enabled = false;

                if (hitResult.HitType == HitType.Hit)
                {
                    cells[point.X, point.Y].ColorScheme = shipScheme;
                }
            }

            if(hitResult.SunkenShip is not null)
            {
                foreach (var point in GetShipPoints(hitResult.SunkenShip))
                {
                    cells[point.X, point.Y].ColorScheme = shipScheme;
                }
            }
        }
    }
}
