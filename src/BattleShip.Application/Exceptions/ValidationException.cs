namespace BattleShip.Application.Exceptions;

using System.Runtime.Serialization;
using BattleShip.Application.Commons;
using BattleShip.Application.Commons.Constants;
using FluentValidation.Results;

[Serializable]
public sealed class ValidationException : ApplicationException
{
    public override string Code => ErrorCodes.VALIDATION_FAILED;
    public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this("Errors have occurred during validation process")
    {
        var errors = failures
            .GroupBy(e => e.ErrorCode, e => e.ErrorMessage)
            .ToDictionary(
                failureGroup => failureGroup.Key,
                failureGroup => failureGroup.ToArray()
            );

        this.Errors = errors;
    }

    public ValidationException(string message, Exception? innerException = null) : base(message, innerException)
    {
    }

    public ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
