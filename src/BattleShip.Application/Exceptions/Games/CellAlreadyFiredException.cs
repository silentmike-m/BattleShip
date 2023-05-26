namespace BattleShip.Application.Exceptions.Games;

using System.Runtime.Serialization;
using BattleShip.Application.Commons;
using BattleShip.Application.Commons.Constants;

[Serializable]
public sealed class CellAlreadyFiredException : ApplicationException
{
    public override string Code => ErrorCodes.CELL_ALREADY_FIRED;

    public CellAlreadyFiredException(int column, char row, string status)
        : base($"Cell with column '{column}' and row '{row}' has been already fired with status '{status}'")
    {
    }

    public CellAlreadyFiredException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
