namespace BattleShip.Infrastructure.MemoryDb.Interfaces;

internal interface IDbContext
{
    string? Game { get; set; }
}
