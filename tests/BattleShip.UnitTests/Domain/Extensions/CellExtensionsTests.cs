namespace BattleShip.UnitTests.Domain.Extensions;

using BattleShip.Domain.Entities;
using BattleShip.Domain.Extensions;

[TestClass]
public sealed class CellExtensionsTests
{
    private static readonly CellEntity CELL_1_1 = new()
    {
        Column = 1,
        Row = 1,
    };

    private static readonly CellEntity CELL_1_2 = new()
    {
        Column = 2,
        Row = 1,
    };

    private static readonly CellEntity CELL_1_3 = new()
    {
        Column = 3,
        Row = 1,
    };

    private static readonly CellEntity CELL_2_1 = new()
    {
        Column = 1,
        Row = 2,
    };

    private static readonly CellEntity CELL_2_2 = new()
    {
        Column = 2,
        Row = 2,
    };

    private static readonly CellEntity CELL_2_3 = new()
    {
        Column = 3,
        Row = 2,
    };

    private static readonly CellEntity CELL_3_1 = new()
    {
        Column = 1,
        Row = 3,
    };

    private static readonly CellEntity CELL_3_2 = new()
    {
        Column = 2,
        Row = 3,
    };

    private static readonly CellEntity CELL_3_3 = new()
    {
        Column = 3,
        Row = 3,
    };

    private static readonly List<CellEntity> CELLS = new()
    {
        CELL_1_1,
        CELL_1_2,
        CELL_1_3,
        CELL_2_1,
        CELL_2_2,
        CELL_2_3,
        CELL_3_1,
        CELL_3_2,
        CELL_3_3,
    };

    [DataTestMethod, DataRow(data1: 3, 3, 1, 1), DataRow(data1: 4, 4, 1, 1), DataRow(data1: 3, 3, 0, 0)]
    public void Should_Return_All_Cell_On_Get_Range(int endColumn, int endRow, int startColumn, int startRow)
    {
        //GIVEN

        //WHEN
        var result = CELLS.GetRange(endColumn, endRow, startColumn, startRow);

        //THEN
        result.Should()
            .BeEquivalentTo(CELLS)
            ;
    }

    [TestMethod]
    public void Should_Return_Cell_From_Square_On_Get_Range()
    {
        //GIVEN
        const int endColumn = 2;
        const int endRow = 2;
        const int startColumn = 1;
        const int startRow = 1;

        //WHEN
        var result = CELLS.GetRange(endColumn, endRow, startColumn, startRow);

        //THEN
        var expectedCells = new List<CellEntity>
        {
            CELL_1_1,
            CELL_1_2,
            CELL_2_1,
            CELL_2_2,
        };

        result.Should()
            .BeEquivalentTo(expectedCells)
            ;
    }

    [TestMethod]
    public void Should_Return_Cells_From_Column_On_Get_Range()
    {
        //GIVEN
        const int endColumn = 1;
        const int endRow = 3;
        const int startColumn = 1;
        const int startRow = 1;

        //WHEN
        var result = CELLS.GetRange(endColumn, endRow, startColumn, startRow);

        //THEN
        var expectedCells = new List<CellEntity>
        {
            CELL_1_1,
            CELL_2_1,
            CELL_3_1,
        };

        result.Should()
            .BeEquivalentTo(expectedCells)
            ;
    }

    [TestMethod]
    public void Should_Return_Cells_From_Row_On_Get_Range()
    {
        //GIVEN
        const int endColumn = 3;
        const int endRow = 1;
        const int startColumn = 1;
        const int startRow = 1;

        //WHEN
        var result = CELLS.GetRange(endColumn, endRow, startColumn, startRow);

        //THEN
        var expectedCells = new List<CellEntity>
        {
            CELL_1_1,
            CELL_1_2,
            CELL_1_3,
        };

        result.Should()
            .BeEquivalentTo(expectedCells)
            ;
    }

    [DataTestMethod, DataRow(data1: 'A', 1), DataRow(data1: 'a', 1), DataRow(data1: 'Z', moreData: null), DataRow(data1: 'b', 2)]
    public void Should_Return_Row_Number_On_Get_Row_Number(char row, int? rowNumber)
    {
        //GIVEN

        //WHEN
        var result = row.GetRowNumber();

        //THEN
        result.Should()
            .Be(rowNumber)
            ;
    }
}
