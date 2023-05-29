namespace BattleShip.UnitTests.Domain.Constants;

using BattleShip.Domain.Common.Constants;

[TestClass]
public sealed class CellRowNamesTests
{
    [TestMethod]
    public void Cell_Row_Names_Should_Between_Game_Min_And_Max_Size()
    {
        //GIVEN

        //WHEN

        //THEN
        CellRowNames.CELL_ROW_MAPPING.Keys.Should()
            .HaveCount(GameSize.GAME_MAX_SIZE)
            ;
    }

    [TestMethod]
    public void Cell_Row_Names_Should_Contain_Unique_Values()
    {
        //GIVEN

        //WHEN

        //THEN
        CellRowNames.CELL_ROW_MAPPING.Values.Should()
            .OnlyHaveUniqueItems()
            ;
    }
}
