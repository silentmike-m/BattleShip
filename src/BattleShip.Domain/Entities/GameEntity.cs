namespace BattleShip.Domain.Entities;

using BattleShip.Domain.Enums;
using BattleShip.Domain.Interfaces;
using ShipId = Guid;

public sealed class GameEntity
{
    public IReadOnlyList<CellEntity> Cells { get; init; } = new List<CellEntity>();
    public IReadOnlyDictionary<ShipId, Ship> Fleet { get; init; } = new Dictionary<Guid, Ship>();
    public Guid Id { get; init; } = Guid.NewGuid();
    public GameStatus Status { get; set; } = GameStatus.Active;
}
