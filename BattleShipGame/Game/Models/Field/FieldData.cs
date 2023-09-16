namespace BattleShipGame.Game.Models;

public class FieldData
{
    public List<ShipData> Ships { get; set; } = new List<ShipData>();
    public List<Point> Hits  { get; set; } = new List<Point>();

    public static FieldData Empty { get; } = new FieldData();
}