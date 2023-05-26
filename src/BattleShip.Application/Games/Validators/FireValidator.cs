namespace BattleShip.Application.Games.Validators;

using BattleShip.Application.Commons.Constants;
using BattleShip.Application.Games.Commands;
using BattleShip.Domain.Common.Constants;
using BattleShip.Domain.Extensions;
using FluentValidation;

public sealed class FireValidator : AbstractValidator<Fire>
{
    public FireValidator()
    {
        this.RuleFor(request => request.Column)
            .InclusiveBetween(GameSize.GAME_MIN_SIZE, GameSize.GAME_MAX_SIZE)
            .WithErrorCode(ValidationErrorCodes.FIRE_INVALID_COLUMN)
            .WithMessage(ValidationErrorCodes.FIRE_INVALID_COLUMN_MESSAGE)
            ;

        this.RuleFor(request => request.Row)
            .Must(BeCellName)
            .WithErrorCode(ValidationErrorCodes.FIRE_INVALID_ROW)
            .WithMessage(ValidationErrorCodes.FIRE_INVALID_ROW_MESSAGE)
            ;
    }

    private static bool BeCellName(char row)
    {
        var columnNumber = row.GetRowNumber();

        return columnNumber is not null;
    }
}
