using BattleShipGameClient.Commands;
using BSGameClient.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSGameClient.Commands
{
    internal class CreateLobbyCommand : IResultCommand<Task<Guid>>
    {
        private readonly IGameController gameController;

        public CreateLobbyCommand(IGameController gameController)
        {
            this.gameController = gameController;
        }

        public Task<Guid> Execute()
        {
            Debug.WriteLine(nameof(CreateLobbyCommand) + " was executed");
            return gameController.CreateLobby();
        }
    }
}
