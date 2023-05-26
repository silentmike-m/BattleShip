namespace BattleShip.Application.Exceptions.Games;

using System.Runtime.Serialization;
using BattleShip.Application.Commons;
using BattleShip.Application.Commons.Constants;

[Serializable]
public sealed class GameFinishedException : ApplicationException
{
    public override string Code => ErrorCodes.GAME_FINISHED;

    public GameFinishedException()
        : base("Game has been finished")
    {
    }

    public GameFinishedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
