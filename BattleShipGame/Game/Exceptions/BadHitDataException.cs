namespace BattleShipGame.Game.Exceptions;

public class BadHitDataException<THit> : Exception
{
    public THit hit { get; }
    public BadHitDataException(THit hit)
    {
        this.hit = hit;
    }
}