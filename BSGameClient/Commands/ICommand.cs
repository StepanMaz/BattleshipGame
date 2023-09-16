namespace BattleShipGameClient.Commands;

public interface ICommand
{
    public void Execute();
}

public interface ICommand<in T>
{
    public void Execute(T arg);
}

public interface ICommand<in T1, in T2>
{
    public void Execute(T1 arg1, T2 arg2);
}

public interface ICommand<in T1, in T2, in T3>
{
    public void Execute(T1 arg1, T2 arg2, T3 arg3);
}

public interface IResultCommand<out TRes>
{
    public TRes Execute();
}

public interface IResultCommand<in T, out TRes>
{
    public TRes Execute(T arg);
}

public interface IResultCommand<in T1, in T2, out TRes>
{
    public TRes Execute(T1 arg1, T2 arg2);
}

public interface IResultCommand<in T1, in T2, in T3, out TRes>
{
    public TRes Execute(T1 arg1, T2 arg2, T3 arg3);
}