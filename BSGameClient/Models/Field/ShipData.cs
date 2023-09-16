namespace BattleShipGameClient.Models;

public class ShipData
{
    public Point Place { get; set; } = new Point();
    public Direction Direction { get; set; }
    public int Length { get; set; }
}