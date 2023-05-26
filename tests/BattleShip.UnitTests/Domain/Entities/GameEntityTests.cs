namespace BattleShip.UnitTests.Domain.Entities;

using BattleShip.Domain.Entities;
using BattleShip.Domain.Enums;

[TestClass]
public sealed class GameEntityTests
{
    [TestMethod]
    public void Should_Create_Game_With_Default_Values()
    {
        //GIVEN

        //WHEN
        var result = new GameEntity();

        //THEN
        var expectedResult = new GameEntity
        {
            Status = GameStatus.Active,
        };

        result.Should()
            .BeEquivalentTo(expectedResult, options => options.Excluding(game => game.Id))
            ;
    }
}
