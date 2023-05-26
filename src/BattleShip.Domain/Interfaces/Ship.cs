namespace BattleShip.Domain.Interfaces;

using BattleShip.Domain.Enums;

public abstract class Ship
{
    public int Hits { get; set; } = 0;
    public Guid Id { get; init; } = Guid.NewGuid();
    public bool IsHorizontalOrientation { get; set; } = default;
    public string Name { get; init; } = string.Empty;
    public int Size { get; init; } = default;
    public ShipType Type { get; init; } = ShipType.Battleship;
}
