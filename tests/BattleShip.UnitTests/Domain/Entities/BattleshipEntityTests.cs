namespace BattleShip.UnitTests.Domain.Entities;

using BattleShip.Domain.Entities;
using BattleShip.Domain.Enums;

[TestClass]
public sealed class BattleshipEntityTests
{
    private const string EXPECTED_NAME = "Battleship";
    private const int EXPECTED_SIZE = 5;
    private const ShipType EXPECTED_TYPE = ShipType.Battleship;

    [TestMethod]
    public void Should_Create_Battleship_With_Default_Values()
    {
        //GIVEN

        //WHEN
        var result = new BattleshipEntity();

        //THEN
        var expectedResult = new BattleshipEntity
        {
            Hits = 0,
        };

        result.Should()
            .BeEquivalentTo(expectedResult, options => options.Excluding(ship => ship.Id))
            ;

        result.Id.Should()
            .NotBeEmpty()
            ;

        result.Name.Should()
            .Be(EXPECTED_NAME)
            ;

        result.Size.Should()
            .Be(EXPECTED_SIZE)
            ;

        result.Type.Should()
            .Be(EXPECTED_TYPE)
            ;
    }
}
