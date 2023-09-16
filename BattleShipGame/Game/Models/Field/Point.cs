namespace BattleShipGame.Game.Models;

public record Point(int X, int Y)
{                   
    public Point() : this(0, 0)
    {

    }
}