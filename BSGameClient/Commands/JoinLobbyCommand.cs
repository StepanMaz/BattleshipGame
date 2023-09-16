using BattleShipGameClient.Commands;
using BattleShipGameClient.Game;
using BSGameClient.Game.FieldFiller;
using BSGameClient.Services;
using BSGameClient.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSGameClient.Commands
{
    internal class JoinLobbyCommand : ICommand<Guid>
    {
        private readonly IFieldFillingStrategy fieldFillingStrategy;
        private readonly string hubUrl;

        public JoinLobbyCommand(string url, IFieldFillingStrategy fieldFillingStrategy)
        {
            hubUrl = url;
            this.fieldFillingStrategy = fieldFillingStrategy;
        }

        public void Execute(Guid guid)
        {
            Debug.WriteLine(nameof(JoinLobbyCommand) + " was executed");
            new OpenWindowCommand(new GameWindow(new OnlineGame(guid, hubUrl), fieldFillingStrategy)).Execute();
        }
    }
}
