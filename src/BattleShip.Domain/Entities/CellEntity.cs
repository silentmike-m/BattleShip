namespace BattleShip.Domain.Entities;

using BattleShip.Domain.Enums;

public sealed class CellEntity
{
    public int Column { get; init; } = 0;
    public int Row { get; init; } = default;
    public Guid? ShipId { get; set; } = default;
    public CellStatus Status { get; set; } = CellStatus.Unfired;
}
