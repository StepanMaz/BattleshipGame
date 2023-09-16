using BattleShipGame.Game;
using BattleShipGame.Game.Models;

namespace BattleShipGame.Game;

public class SimpleRules : GameRules
{
    private readonly GameConfig gameConfig = new GameConfig()
    { 
        SizeX = 10,
        SizeY = 10,
        ShipsLength = new GameConfig.ShipConfig[] {
            new () { Length = 1, Amount = 4 },
            new () { Length = 4, Amount = 1 },
            new () { Length = 2, Amount = 3 },
            new () { Length = 3, Amount = 2 },
        }
    };

    public override HitResult HandleHit(Point hit, FieldData fieldData)
    {
        HitResult result = new HitResult();

        AddChange(hit);

        foreach (var ship in fieldData.Ships)
        {
            foreach (var point in GetShipPoints(ship))
            {
                if(point == hit)
                {
                    if(IsSunk(ship, fieldData.Hits))
                    {
                        result.SunkenShip = ship;
                        result.HitType = HitType.Sunk;
                    }
                    else
                    {
                        result.HitType = HitType.Hit;
                    }
                }
            }
        }

        result.GameEnded = IsEnded(fieldData);

        result.PlayerChanged = result.HitType == HitType.Miss;

        return result;

        void AddChange(Point hit)
        {
            result.Changes.Add(hit);
            fieldData.Hits.Add(hit);
        }
    }

    public override bool ValidateField(IEnumerable<ShipData> ships)
    {
        return true;
    }

    public override bool ValidateHit(Point hit, FieldData fieldData)
    {
        if (IsEnded(fieldData))
            return false;

        if(hit.X < 0 || hit.Y < 0 || hit.X >= gameConfig.SizeX || hit.Y >= gameConfig.SizeY)
            return false;
        if (fieldData.Hits.Contains(hit))
            return false;
        return true;
    }

    private bool IsSunk(ShipData ship, IEnumerable<Point> hits)
    {
        foreach (var point in GetShipPoints(ship))
        {
            if(!hits.Contains(point))
            {
                return false;
            }
        }
        return true;
    }

    private bool IsEnded(FieldData fieldData)
    {
        foreach (var ship in fieldData.Ships)
        {
            foreach (var point in GetShipPoints(ship))
            {
                if(!fieldData.Hits.Contains(point))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private IEnumerable<Point> GetShipPoints(ShipData shipData)
    {
        (int x, int y) dir = shipData.Direction switch
        {
            Direction.BottomToTop => (-1, 0),
            Direction.LeftToRight => (0, 1),
            Direction.TopToBottom => (1, 0),
            Direction.RightToLeft => (0, -1),
            _ => (-1, 0)
        };

        for (int i = 0; i < shipData.Length; i++)
        {
            yield return new(shipData.Place.X + dir.x * i, shipData.Place.Y + dir.y * i);
        }
    }

    public override Game<T> CreateGame<T>()
    {
        return new BattleShipGame<T>(this);
    }
}