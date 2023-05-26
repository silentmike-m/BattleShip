namespace BattleShip.Domain.Entities;

using BattleShip.Domain.Enums;

public sealed class CellEntity
{
    public int Column { get; init; } = 0;
    public int Row { get; init; } = 0;
    public Guid? ShipId { get; set; } = default;
    public ShootType ShootType { get; set; } = ShootType.Unfired;
}
