namespace BattleShipGame.Game.Exceptions;

public class WrongStateException<TState> : Exception
{
    private const string FORMAT = "Expected: {0}. Actual: {1}";

    public TState expected { get; }
    public TState actual { get; }
    public WrongStateException(TState expected, TState actual) : base(string.Format(FORMAT, expected, actual))
    {
        this.expected = expected;
        this.actual = actual;
    }

    public override string ToString()
    {
        return string.Format(FORMAT, expected, actual);
    }
}