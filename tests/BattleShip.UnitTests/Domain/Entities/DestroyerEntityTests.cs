namespace BattleShip.UnitTests.Domain.Entities;

using BattleShip.Domain.Entities;
using BattleShip.Domain.Enums;

[TestClass]
public sealed class DestroyerEntityTests
{
    private const string EXPECTED_NAME = "Destroyer";
    private const int EXPECTED_SIZE = 4;
    private const ShipType EXPECTED_TYPE = ShipType.Destroyer;

    [TestMethod]
    public void Should_Create_Destroyer_With_Default_Values()
    {
        //GIVEN

        //WHEN
        var result = new DestroyerEntity();

        //THEN
        var expectedResult = new DestroyerEntity
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
