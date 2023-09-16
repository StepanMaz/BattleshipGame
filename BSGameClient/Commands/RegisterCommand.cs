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
    internal class RegisterCommand : IResultCommand<string, string, Task>
    {
        private readonly IAccountController accountController;

        public RegisterCommand(IAccountController accountController)
        {
            this.accountController = accountController;
        }

        private ICommand? _onSuccessCommand;

        public void SetOnSuccess(ICommand command)
        {
            _onSuccessCommand = command;
        }

        public async Task Execute(string arg1, string arg2)
        {
            await accountController.Register(arg1, arg2);

            _onSuccessCommand?.Execute();
        }
    }
}
