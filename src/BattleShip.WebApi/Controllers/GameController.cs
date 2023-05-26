namespace BattleShip.WebApi.Controllers;

using BattleShip.Application.Games.Commands;
using BattleShip.Application.Games.Queries;
using BattleShip.Application.Games.ViewModel;
using BattleShip.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

[ApiController, Route("[controller]/[action]")]
public sealed class GameController : ApiControllerBase
{
    [HttpGet(Name = "GetGame")]
    public async Task<BaseResponse<Game>> GetGame(CancellationToken cancellationToken = default)
    {
        var request = new GetGame();

        var game = await this.Mediator.Send(request, cancellationToken);

        return new BaseResponse<Game>
        {
            Response = game,
        };
    }

    [HttpPost]
    public async Task<IActionResult> StartGame(CancellationToken cancellationToken = default)
    {
        var request = new StartGame();

        await this.Mediator.Send(request, cancellationToken);

        return this.Ok();
    }
}
