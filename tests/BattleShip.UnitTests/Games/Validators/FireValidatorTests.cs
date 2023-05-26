namespace BattleShip.UnitTests.Games.Validators;

using BattleShip.Application.Commons.Constants;
using BattleShip.Application.Games.Commands;
using BattleShip.Application.Games.Validators;

[TestClass]
public sealed class FireValidatorTests
{
    private readonly FireValidator validator = new();

    [TestMethod]
    public async Task Should_Not_Pass_Validation_When_Fire_Column_Is_To_Low()
    {
//GIVEN
        var request = new Fire
        {
            Column = 0,
            Row = 'A',
        };

        //WHEN
        var result = await this.validator.ValidateAsync(request);

        //THEN
        result.IsValid.Should()
            .BeFalse()
            ;

        result.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(error =>
                error.ErrorCode == ValidationErrorCodes.FIRE_INVALID_COLUMN
                && error.ErrorMessage == ValidationErrorCodes.FIRE_INVALID_COLUMN_MESSAGE
            )
            ;
    }

    [TestMethod]
    public async Task Should_Not_Pass_Validation_When_Fire_Row_And_Column_Are_Not_Correct()
    {
        //GIVEN
        var request = new Fire
        {
            Column = 11,
            Row = 'Z',
        };

        //WHEN
        var result = await this.validator.ValidateAsync(request);

        //THEN
        result.IsValid.Should()
            .BeFalse()
            ;

        result.Errors.Should()
            .HaveCount(2)
            .And
            .Contain(error =>
                error.ErrorCode == ValidationErrorCodes.FIRE_INVALID_COLUMN
                && error.ErrorMessage == ValidationErrorCodes.FIRE_INVALID_COLUMN_MESSAGE
            )
            .And
            .Contain(error =>
                error.ErrorCode == ValidationErrorCodes.FIRE_INVALID_ROW
                && error.ErrorMessage == ValidationErrorCodes.FIRE_INVALID_ROW_MESSAGE
            )
            ;
    }

    [TestMethod]
    public async Task Should_Pass_Validation_When_Fire_Row_And_Column_Are_Correct()
    {
        //GIVEN
        var request = new Fire
        {
            Column = 1,
            Row = 'A',
        };

        //WHEN
        var result = await this.validator.ValidateAsync(request);

        //THEN
        result.IsValid.Should()
            .BeTrue()
            ;

        result.Errors.Should()
            .BeEmpty()
            ;
    }

    [TestMethod]
    public async Task Should_Pass_Validation_When_Fire_Row_Is_Lower()
    {
        //GIVEN
        var request = new Fire
        {
            Column = 1,
            Row = 'a',
        };

        //WHEN
        var result = await this.validator.ValidateAsync(request);

        //THEN
        result.IsValid.Should()
            .BeTrue()
            ;

        result.Errors.Should()
            .BeEmpty()
            ;
    }
}
