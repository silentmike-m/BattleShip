namespace BattleShip.Application.Exceptions;

using System.Runtime.Serialization;
using BattleShip.Application.Commons;
using BattleShip.Application.Commons.Constants;

[Serializable]
public sealed class UnhandledErrorException : ApplicationException
{
    public override string Code => ErrorCodes.UNHANDLED_ERROR;

    public UnhandledErrorException()
        : base("Unhandled error has occurred")
    {
    }

    private UnhandledErrorException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
