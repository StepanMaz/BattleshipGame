using BSGameClient.Services;

namespace BattleShipGameClient.Commands;

public class LogInCommand : IResultCommand<string, string, Task>
{
    private readonly IAccountController _accountController;
    public LogInCommand(IAccountController accountController)
    {
        _accountController = accountController;
    }

    private ICommand? _onSuccessCommand;

    public void SetOnSuccess(ICommand command)
    {
        _onSuccessCommand = command;
    }

    public async Task Execute(string arg1, string arg2)
    {
        var res = await _accountController.Login(arg1, arg2);

        if(res is not null)
        {
            _onSuccessCommand?.Execute();
        }
        else
        {
            throw new Exception("Incorrect login or password");
        }
    }
}