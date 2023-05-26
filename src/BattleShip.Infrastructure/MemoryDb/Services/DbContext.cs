namespace BattleShip.Infrastructure.MemoryDb.Services;

using BattleShip.Infrastructure.MemoryDb.Interfaces;

internal sealed class DbContext : IDbContext
{
    public string? Game { get; set; } = default;
}
