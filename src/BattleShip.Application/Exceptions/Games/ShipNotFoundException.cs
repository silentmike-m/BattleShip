namespace BattleShip.Application.Exceptions.Games;

using System.Runtime.Serialization;
using BattleShip.Application.Commons;
using BattleShip.Application.Commons.Constants;

[Serializable]
public sealed class ShipNotFoundException : ApplicationException
{
    public override string Code => ErrorCodes.SHIP_NOT_FOUND;

    public ShipNotFoundException(Guid id)
        : base($"Ship with id '{id}' has not been found")
    {
    }

    public ShipNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
