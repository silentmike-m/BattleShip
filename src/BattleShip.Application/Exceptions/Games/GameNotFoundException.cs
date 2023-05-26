namespace BattleShip.Application.Exceptions.Games;

using System.Runtime.Serialization;
using BattleShip.Application.Commons;
using BattleShip.Application.Commons.Constants;

[Serializable]
public sealed class GameNotFoundException : ApplicationException
{
    public override string Code => ErrorCodes.GAME_NOT_FOUND;

    public GameNotFoundException()
        : base("Game has not been found")
    {
    }

    public GameNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
