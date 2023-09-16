namespace BattleShipGame.Game;

public class GameConfig
{
    public int SizeX { get; set; }
    public int SizeY { get; set; }
    public ShipConfig[] ShipsLength = Array.Empty<ShipConfig>();

    public class ShipConfig
    {
        public int Length { get; set; }
        public int Amount { get; set;  }
    }
}