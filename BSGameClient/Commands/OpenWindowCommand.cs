using Terminal.Gui;

namespace BattleShipGameClient.Commands;

public class OpenWindowCommand : ICommand
{
    private readonly Window window;
    public OpenWindowCommand(Window window)
    {
        this.window = window;
    }

    public void Execute()
    {
        Application.Top.RemoveAll();
        Application.Top.Add(window);
    }
}

public class OpenWindowFuncCommand : ICommand
{
    private readonly Func<Window> window;
    public OpenWindowFuncCommand(Func<Window> window)
    {
        this.window = window;
    }

    public void Execute()
    {
        Application.Top.RemoveAll();
        Application.Top.Add(window());
    }
}