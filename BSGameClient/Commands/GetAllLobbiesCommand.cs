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
    internal class GetAllLobbiesCommand : IResultCommand<Task<IEnumerable<Guid>>>
    {
        private readonly IGameController gameController;
        public GetAllLobbiesCommand(IGameController gameController)
        {
            this.gameController = gameController;
        }

        public Task<IEnumerable<Guid>> Execute()
        {
            Debug.WriteLine(nameof(GetAllLobbiesCommand) + " was executed");
            return gameController.GetAllLobies();
        }
    }
}
