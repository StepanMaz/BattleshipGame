using BattleShipGame.Game.Models;

namespace BattleShipGame.Game;

public class HitResult
{
    public bool PlayerChanged {  get; set; }
    public bool GameEnded { get; set; }
    public List<Point> Changes { get; set; } = new List<Point>();
    public HitType HitType { get; set; }
    public ShipData? SunkenShip { get; set; }
}

public enum HitType
{
    Miss,
    Hit,
    Sunk
}