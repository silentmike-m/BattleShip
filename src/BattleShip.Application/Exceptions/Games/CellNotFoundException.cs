namespace BattleShip.Application.Exceptions.Games;

using System.Runtime.Serialization;
using BattleShip.Application.Commons;
using BattleShip.Application.Commons.Constants;

[Serializable]
public sealed class CellNotFoundException : ApplicationException
{
    public override string Code => ErrorCodes.CELL_NOT_FOUND;

    public CellNotFoundException(int column, char row)
        : base($"Cell with column '{column}' and row '{row}' has been finished")
    {
    }

    public CellNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
