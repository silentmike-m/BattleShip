namespace BattleShip.Domain.Extensions;

using BattleShip.Domain.Common.Constants;
using BattleShip.Domain.Entities;

public static class CellExtensions
{
    public static CellEntity? Get(this IEnumerable<CellEntity> self, int column, int row)
        => self.Where(cell => cell.Column == column).SingleOrDefault(cell => cell.Row == row);

    public static IEnumerable<CellEntity> GetRange(this IEnumerable<CellEntity> self, int endColumn, int endRow, int startColumn, int startRow)
    {
        var range = self
            .Where(cell => cell.Column >= startColumn)
            .Where(cell => cell.Column <= endColumn)
            .Where(cell => cell.Row >= startRow)
            .Where(cell => cell.Row <= endRow);

        return range;
    }

    public static int? GetRowNumber(this char row)
    {
        row = char.ToUpperInvariant(row);

        if (CellRowNames.CELL_ROW_MAPPING.TryGetValue(row, out var rowNumber))
        {
            return rowNumber;
        }

        return null;
    }
}
