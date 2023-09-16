using BattleShipGameClient.Game;
using BattleShipGameClient.Models;
using BSGameClient.Game.FieldFiller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;
using System.Reactive.Linq;
using BSGameClient.Game.GameDrawing;
using BattleShipGameClient.Commands;

namespace BSGameClient.Windows
{
    internal class GameWindow : Window
    {
        private FieldChangedObserver current_player_observer, enemy_player_observer;
        private Label errorLable;
        private readonly IFieldFillingStrategy fieldFillingStrategy;
        private readonly IGameDataSource gameDataSource;

        private static ICommand? returnCommand;
        public static void SetReturnCommand(ICommand returnCommand)
        {
            GameWindow.returnCommand = returnCommand;
        }

        public GameWindow(IGameDataSource gameDataSource, IFieldFillingStrategy fieldFillingStrategy)
        {
            this.gameDataSource = gameDataSource;
            this.fieldFillingStrategy = fieldFillingStrategy;

            current_player_observer = new FieldChangedObserver(new GameDrawingStrategy(), 10);
            enemy_player_observer = new FieldChangedObserver(new EnemyFieldDrawingStrategy(), 10);

            gameDataSource.Hit.Subscribe(enemy_player_observer);
            gameDataSource.EnemyHit.Subscribe(current_player_observer);

            gameDataSource.Hit.Where(h => h.PlayerChanged).Subscribe(_ => enemy_player_observer.Enabled = false);
            gameDataSource.EnemyHit.Where(h => h.PlayerChanged).Subscribe(_ => enemy_player_observer.Enabled = true);

            enemy_player_observer.Click += gameDataSource.ProcessHit;

            gameDataSource.Error.Subscribe(async (m) =>
            {
                if(errorLable != null)
                {
                    errorLable.Text = "Error: " + m;
                    await Task.Delay(3000);
                    errorLable.Text = "";
                }
            });

            gameDataSource.Hit.Where(g => g.GameEnded).Subscribe(async _ =>
            {
                if (errorLable != null)
                    errorLable.Text = "You won";
                await Task.Delay(5000);
                returnCommand?.Execute();
                gameDataSource.Dispose();
                return;
            });

            gameDataSource.EnemyHit.Where(g => g.GameEnded).Subscribe(async _ =>
            {
                if (errorLable != null)
                    errorLable.Text = "You lost";
                await Task.Delay(5000);
                returnCommand?.Execute();
                gameDataSource.Dispose();
                return;
            });

            gameDataSource.OnPlayerLeft.Subscribe(async p =>
            {
                returnCommand?.Execute();
                gameDataSource.Dispose();
                await Task.Delay(5000);
                if (errorLable != null)
                    errorLable.Text = "You won";
                return;
            });

            InitComponents();
        }

        private void InitComponents()
        {
            Title = "Game";

            var returnButton = new Button("<-")
            {
                X = 1,
                Y = 1
            };
            returnButton.Clicked += () =>
            {
                gameDataSource.Dispose();
                returnCommand?.Execute();
            };

            errorLable = new Label()
            {
                X = Pos.Center(),
                Y = 5,
                Width = 22,
                Height = 1
            };

            var frame = new FrameView()
            {
                X = 15,
                Y = Pos.Center(),
                AutoSize = true,
                Width = 22,
                Height = 16,
                Border = new Border() { BorderStyle = BorderStyle.Rounded }
            };

            var playerLable = new Label("Your field")
            {
                X = Pos.Left(frame) + 2,
                Y = Pos.Top(frame) - 1,
            };

            var submitButton = new Button("Select")
            {
                X = Pos.Left(current_player_observer) + 1,
                Y = Pos.Bottom(current_player_observer) + 1,
                Enabled = false
            };

            var randomizeButton = new Button("Randomize")
            {
                X = Pos.Left(current_player_observer) + 1,
                Y = Pos.Bottom(submitButton) + 1
            };

            var fielddata = FieldData.Empty;

            randomizeButton.Clicked += () =>
            {
                current_player_observer.Set(fielddata = new FieldData() { Ships = fieldFillingStrategy.Generate().ToList() });
                submitButton.Enabled = true;
            };

            submitButton.Clicked += () =>
            {
                frame.Remove(randomizeButton);
                frame.Remove(submitButton);

                frame.Height = 12;

                gameDataSource.SubmitField(fielddata);
            };

            current_player_observer.Width = 20;
            current_player_observer.Height = 10;

            frame.Add(current_player_observer, randomizeButton, submitButton);

            var enemyFrame = new FrameView()
            {
                X = Pos.AnchorEnd() - 35,
                Y = Pos.Center(),
                AutoSize = true,
                Width = 22,
                Height = 12,
                Border = new Border() { BorderStyle = BorderStyle.Rounded }
            };

            var enemyLable = new Label("Enemy field")
            {
                X = Pos.Left(enemyFrame) + 2,
                Y = Pos.Top(enemyFrame) - 1,
            };

            enemy_player_observer.Width = 20;
            enemy_player_observer.Height = 10;

            enemyFrame.Add(enemy_player_observer);

            Add(frame, playerLable, enemyFrame, enemyLable, errorLable, returnButton);
        }
    }
}
