using BattleShipGameClient.Game;
using BattleShipGameClient.Models;
using BattleShipGameClientGame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace BSGameClient
{
    internal class FieldChangedObserver : View, IObserver<HitResult>
    {
        public IGameDrawingStrategy gameDrawingStrategy;
        private int size;

        View[,] views = null!;

        public FieldChangedObserver(IGameDrawingStrategy gameDrawingStrategy, int size)
        {
            this.size = size;
            this.gameDrawingStrategy = gameDrawingStrategy;

            views = new View[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int _i = i, _j = j;
                    var cell = views[i, j] = new Label("_-")
                    {
                        X = i * 2,
                        Y = j
                    };

                    cell.MouseClick += (x) =>
                    {
                        if (x.MouseEvent.Flags == MouseFlags.Button1Pressed)
                            Click?.Invoke(_i, _j);
                    };

                    Add(cell);
                }
            }

            Click += (x, y) => Debug.WriteLine($"Cell clicked ({x}, {y})");
        }

        public void Set(FieldData fieldData)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    views[i, j].Text = "_-";
                    views[i, j].ColorScheme = Colors.Base;
                }
            }
            gameDrawingStrategy.Draw(fieldData, views);
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            MessageBox.ErrorQuery("Error", error.Message, "Ok");
        }

        public void OnNext(HitResult value)
        {
            gameDrawingStrategy.Draw(value, views);
        }

        public event Action<int, int> Click = null!;
    }
}
