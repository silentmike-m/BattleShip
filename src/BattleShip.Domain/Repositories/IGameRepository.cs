namespace BattleShip.Domain.Repositories;

using BattleShip.Domain.Entities;

public interface IGameRepository
{
    GameEntity? Get();
    void Save(GameEntity game);
}
