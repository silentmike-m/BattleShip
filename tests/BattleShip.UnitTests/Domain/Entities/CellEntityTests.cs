namespace BattleShip.UnitTests.Domain.Entities;

using BattleShip.Domain.Entities;
using BattleShip.Domain.Enums;

[TestClass]
public sealed class CellEntityTests
{
    [TestMethod]
    public void Should_Create_Cell_With_Default_Values()
    {
        //GIVEN

        //WHEN
        var result = new CellEntity();

        //THEN
        var expectedResult = new CellEntity
        {
            Column = 0,
            Row = 0,
            ShipId = null,
            ShootType = ShootType.Unfired,
        };

        result.Should()
            .BeEquivalentTo(expectedResult)
            ;
    }
}
