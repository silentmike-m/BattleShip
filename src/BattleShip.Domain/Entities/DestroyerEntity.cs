﻿namespace BattleShip.Domain.Entities;

using BattleShip.Domain.Enums;
using BattleShip.Domain.Interfaces;

public sealed class DestroyerEntity : Ship
{
    public DestroyerEntity()
    {
        this.Name = "Destroyer";
        this.Size = 4;
        this.Type = ShipType.Destroyer;
    }
}
