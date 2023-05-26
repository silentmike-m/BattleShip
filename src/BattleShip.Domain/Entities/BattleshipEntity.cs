namespace BattleShip.Domain.Entities;

using BattleShip.Domain.Enums;
using BattleShip.Domain.Interfaces;

public sealed class BattleshipEntity : ShipEntity
{
    public BattleshipEntity()
    {
        this.Name = "Battleship";
        this.Size = 5;
        this.Type = ShipType.Battleship;
    }
}
