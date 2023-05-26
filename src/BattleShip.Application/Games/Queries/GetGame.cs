namespace BattleShip.Application.Games.Queries;

using BattleShip.Application.Games.ViewModel;

public sealed record GetGame : IRequest<Game>
{
}
